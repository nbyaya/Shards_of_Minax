using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a flame welder")]
    public class FlameWelder : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0); // time between welder speech
        public DateTime m_NextSpeechTime;

        [Constructable]
        public FlameWelder() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();
			Team = 2;

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Flame Welder";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Flame Welder";
            }

            if (Utility.RandomBool())
            {
                Item weldingHelmet = new PlateHelm();
                weldingHelmet.Name = "welding helmet";
                AddItem(weldingHelmet);
            }

            Item weldingTorch = new Club();
            weldingTorch.Name = "welding torch";
            AddItem(weldingTorch);
            weldingTorch.Movable = false;

            Item gloves = new LeatherGloves();
            gloves.Name = "welding gloves";
            AddItem(gloves);

            Item boots = new ThighBoots(Utility.RandomNeutralHue());
            AddItem(boots);

            SetStr(700, 900);
            SetDex(150, 200);
            SetInt(100, 150);

            SetHits(500, 700);

            SetDamage(15, 25);

            SetDamageType(ResistanceType.Physical, 25);
            SetDamageType(ResistanceType.Fire, 75);

            SetResistance(ResistanceType.Physical, 60, 75);
            SetResistance(ResistanceType.Fire, 70, 85);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.Anatomy, 50.1, 75.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.MagicResist, 80.0, 100.0);
            SetSkill(SkillName.Swords, 90.1, 100.0);

            Fame = 5000;
            Karma = -5000;

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
                        case 1: this.Say(true, "I'll burn you to ashes!"); break;
                        case 2: this.Say(true, "You're about to get welded!"); break;
                        case 3: this.Say(true, "No one escapes the flame!"); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }

                base.OnThink();
            }
        }

        public override void GenerateLoot()
        {
            PackGem();
            PackGold(150, 200);
            AddLoot(LootPack.Rich);

            int phrase = Utility.Random(2);

            switch (phrase)
            {
                case 0: this.Say(true, "My flame... extinguished..."); break;
                case 1: this.Say(true, "You... will... burn..."); break;
            }

            PackItem(new SulfurousAsh(Utility.RandomMinMax(10, 20)));
        }

        public FlameWelder(Serial serial) : base(serial)
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
