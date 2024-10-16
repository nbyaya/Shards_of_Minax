using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a firestarter")]
    public class Firestarter : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0); // time between firestarter speech
        public DateTime m_NextSpeechTime;

        [Constructable]
        public Firestarter() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Firestarter";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Firestarter";
            }

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;

            Item robe = new Robe(Utility.RandomRedHue());
            Item boots = new ThighBoots(Utility.RandomNeutralHue());

            AddItem(hair);
            AddItem(robe);
            AddItem(boots);

            if (!this.Female)
            {
                Item beard = new Item(Utility.RandomList(0x203E, 0x2041));
                beard.Hue = hair.Hue;
                beard.Layer = Layer.FacialHair;
                beard.Movable = false;
                AddItem(beard);
            }

            SetStr(700, 900);
            SetDex(177, 255);
            SetInt(151, 250);

            SetHits(500, 750);

            SetDamage(8, 16);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 50);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.Meditation, 25.1, 50.0);
            SetSkill(SkillName.MagicResist, 100.5, 150.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);

            Fame = 6000;
            Karma = -6000;

            VirtualArmor = 50;

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
                        case 0: this.Say(true, "Feel the heat!"); break;
                        case 1: this.Say(true, "Burn! Burn! Burn!"); break;
                        case 2: this.Say(true, "Fire consumes all!"); break;
                        case 3: this.Say(true, "Turn to ashes!"); break;
                    }
                    
                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;

                    // Create a fire field effect
                    Point3D fireLocation = new Point3D(combatant.X + Utility.RandomMinMax(-1, 1), combatant.Y + Utility.RandomMinMax(-1, 1), combatant.Z);
                    Effects.SendLocationEffect(fireLocation, combatant.Map, 0x3709, 30, 10, 0, 0);
                }

                base.OnThink();
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
                case 0: this.Say(true, "The flames... they fade..."); break;
                case 1: this.Say(true, "I will rise from the ashes..."); break;
            }

            PackItem(new SulfurousAsh(Utility.RandomMinMax(10, 20)));
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
                        case 0: this.Say(true, "The fire within me burns stronger!"); break;
                        case 1: this.Say(true, "Is that the best you can do?"); break;
                        case 2: this.Say(true, "My flames will consume you!"); break;
                        case 3: this.Say(true, "You can't extinguish me!"); break;
                    }
                    
                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;

                    // Create a fire field effect
                    Point3D fireLocation = new Point3D(combatant.X + Utility.RandomMinMax(-1, 1), combatant.Y + Utility.RandomMinMax(-1, 1), combatant.Z);
                    Effects.SendLocationEffect(fireLocation, combatant.Map, 0x3709, 30, 10, 0, 0);

                }
            }

            return base.Damage(amount, from);
        }

        public Firestarter(Serial serial) : base(serial)
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
