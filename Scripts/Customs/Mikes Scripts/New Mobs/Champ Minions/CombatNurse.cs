using System;
using Server.Items;
using Server.Targeting;
using Server.Spells;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("corpse of a combat nurse")]
    public class CombatNurse : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0); // time between nurse speech
        public DateTime m_NextSpeechTime;

        [Constructable]
        public CombatNurse() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();
			Team = 2;

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Combat Nurse";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Combat Nurse";
            }

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            Item shirt = new FancyShirt();
            Item pants = new LongPants();
            Item shoes = new Shoes();

            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;

            AddItem(hair);
            AddItem(shirt);
            AddItem(pants);
            AddItem(shoes);

            if (!this.Female)
            {
                Item beard = new Item(Utility.RandomList(0x203E, 0x2041));
                beard.Hue = hair.Hue;
                beard.Layer = Layer.FacialHair;
                beard.Movable = false;
                AddItem(beard);
            }

            SetStr(600, 800);
            SetDex(150, 200);
            SetInt(300, 400);

            SetHits(400, 600);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 40, 60);
            SetResistance(ResistanceType.Fire, 20, 40);
            SetResistance(ResistanceType.Cold, 20, 40);
            SetResistance(ResistanceType.Poison, 20, 40);
            SetResistance(ResistanceType.Energy, 20, 40);

            SetSkill(SkillName.Anatomy, 75.0, 100.0);
            SetSkill(SkillName.Healing, 100.0, 120.0);
            SetSkill(SkillName.Magery, 80.0, 100.0);
            SetSkill(SkillName.MagicResist, 75.0, 100.0);
            SetSkill(SkillName.Tactics, 50.0, 75.0);
            SetSkill(SkillName.Wrestling, 50.0, 75.0);

            Fame = 4500;
            Karma = 4500;

            VirtualArmor = 50;

            m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
        }

        public override bool AlwaysMurderer { get { return false; } }
        public override bool CanRummageCorpses { get { return false; } }
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
                        case 0: this.Say(true, "Stay strong, I'm here to help!"); break;
                        case 1: this.Say(true, "You won't fall on my watch!"); break;
                        case 2: this.Say(true, "Let's patch you up!"); break;
                        case 3: this.Say(true, "Hold the line, I'll boost you!"); break;
                    }
                    
                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }

                HealAndBuffAllies();

                base.OnThink();
            }
        }

        private void HealAndBuffAllies()
        {
            IPooledEnumerable eable = this.GetMobilesInRange(8);
            foreach (Mobile m in eable)
            {
                if (m != null && m != this && m.Alive && !m.IsDeadBondedPet && m is BaseCreature && ((BaseCreature)m).Controlled)
                {
                    if (m.Hits < m.HitsMax)
                    {
                        m.Hits += 10; // Heal the ally
                        m.SendMessage("The Combat Nurse heals you.");
                    }


                }
            }
            eable.Free();
        }

        public override void GenerateLoot()
        {
            PackGem();
            PackGold(100, 150);
            AddLoot(LootPack.Rich);

            int phrase = Utility.Random(2);

            switch (phrase)
            {
                case 0: this.Say(true, "I did my best..."); break;
                case 1: this.Say(true, "Keep fighting... without me..."); break;
            }
        }
        
        public CombatNurse(Serial serial) : base(serial)
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
