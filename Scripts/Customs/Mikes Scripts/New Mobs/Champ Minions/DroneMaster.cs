using System;
using System.Collections.Generic;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a drone master")]
    public class Mechanic : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0); // time between drone master speech
        public DateTime m_NextSpeechTime;

        private List<Drone> m_DeployedDrones;

        [Constructable]
        public Mechanic() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();
			Team = 2;

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Drone Master";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Drone Master";
            }

            Item robe = new Robe();
            Item sandals = new Sandals();
            robe.Hue = Utility.RandomBlueHue();
            sandals.Hue = Utility.RandomNeutralHue();
            AddItem(robe);
            AddItem(sandals);

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;
            AddItem(hair);

            if (!this.Female)
            {
                Item beard = new Item(Utility.RandomList(0x203E, 0x2041));
                beard.Hue = hair.Hue;
                beard.Layer = Layer.FacialHair;
                beard.Movable = false;
                AddItem(beard);
            }

            SetStr(600, 800);
            SetDex(150, 250);
            SetInt(250, 400);

            SetHits(500, 700);

            SetDamage(15, 25);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 50, 70);
            SetResistance(ResistanceType.Fire, 50, 70);
            SetResistance(ResistanceType.Cold, 40, 60);
            SetResistance(ResistanceType.Poison, 60, 80);
            SetResistance(ResistanceType.Energy, 50, 70);

            SetSkill(SkillName.Anatomy, 50.0, 100.0);
            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.Meditation, 50.1, 75.0);
            SetSkill(SkillName.MagicResist, 100.5, 150.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);

            Fame = 7000;
            Karma = -7000;

            VirtualArmor = 60;

            m_DeployedDrones = new List<Drone>();

            m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextSpeechTime)
            {
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
                {
                    int phrase = Utility.Random(4);

                    switch (phrase)
                    {
                        case 0: this.Say(true, "Prepare to be overwhelmed by my drones!"); break;
                        case 1: this.Say(true, "My drones will tear you apart!"); break;
                        case 2: this.Say(true, "You cannot withstand the swarm!"); break;
                        case 3: this.Say(true, "Attack, my mechanical minions!"); break;
                    }
                    
                    DeployCombatDrone(combatant);

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }

                base.OnThink();
            }

            CleanUpDrones();
        }

        private void DeployCombatDrone(Mobile target)
        {
            if (m_DeployedDrones.Count < 5)
            {
                Drone drone = new Drone();
                drone.MoveToWorld(this.Location, this.Map);
                drone.Combatant = target;
                m_DeployedDrones.Add(drone);
            }
        }

        private void CleanUpDrones()
        {
            for (int i = m_DeployedDrones.Count - 1; i >= 0; i--)
            {
                if (m_DeployedDrones[i].Deleted)
                {
                    m_DeployedDrones.RemoveAt(i);
                }
            }
        }

        public override void GenerateLoot()
        {
            PackGem();
            PackGold(200, 250);
            AddLoot(LootPack.FilthyRich);

            int phrase = Utility.Random(2);

            switch (phrase)
            {
                case 0: this.Say(true, "My drones... have failed me..."); break;
                case 1: this.Say(true, "You... will regret this..."); break;
            }

            PackItem(new MandrakeRoot(Utility.RandomMinMax(10, 20)));
        }
        
        public override int Damage(int amount, Mobile from)
        {
            Mobile combatant = this.Combatant as Mobile;

            if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
            {
                if (Utility.RandomBool())
                {
                    int phrase = Utility.Random(4);

                    switch (phrase)
                    {
                        case 0: this.Say(true, "You think that'll stop me?!"); break;
                        case 1: this.Say(true, "Is that all you got?"); break;
                        case 2: this.Say(true, "I've taken on worse than you!"); break;
                        case 3: this.Say(true, "You're in over your head!"); break;
                    }
                    
                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }
            }
                
            return base.Damage(amount, from);
        }

        public Mechanic(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }

        public class Drone : BaseCreature
        {
            [Constructable]
            public Drone() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
            {
                Body = 0x33;
                Name = "Combat Drone";
                Hue = 0x835;

                SetStr(150, 200);
                SetDex(150, 200);
                SetInt(100, 150);

                SetHits(200, 300);

                SetDamage(10, 15);

                SetDamageType(ResistanceType.Physical, 100);

                SetResistance(ResistanceType.Physical, 40, 50);
                SetResistance(ResistanceType.Fire, 30, 40);
                SetResistance(ResistanceType.Cold, 30, 40);
                SetResistance(ResistanceType.Poison, 40, 50);
                SetResistance(ResistanceType.Energy, 40, 50);

                SetSkill(SkillName.MagicResist, 75.0);
                SetSkill(SkillName.Tactics, 75.0);
                SetSkill(SkillName.Wrestling, 75.0);

                VirtualArmor = 40;
            }

            public Drone(Serial serial) : base(serial)
            {
            }

            public override void Serialize(GenericWriter writer)
            {
                base.Serialize(writer);
                writer.Write((int)0);
            }

            public override void Deserialize(GenericReader reader)
            {
                base.Deserialize(reader);
                int version = reader.ReadInt();
            }
        }
    }
}
