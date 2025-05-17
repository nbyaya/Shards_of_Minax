using System;
using Server.Items;
using Server.Targeting;
using Server.Mobiles;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("corpse of a satyr piper")]
    public class SatyrPiper : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0); // time between piper speech
        public DateTime m_NextSpeechTime;

        [Constructable]
        public SatyrPiper() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();
			Team = 2;

            Body = 0x190;
            Name = NameList.RandomName("satyr");
            Title = "the Piper";

            Item flute = new Item(0x2805); // Flute item ID
            flute.Layer = Layer.OneHanded;
            flute.Movable = false;
            AddItem(flute);

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;
            AddItem(hair);

            SetStr(400, 500);
            SetDex(150, 200);
            SetInt(100, 150);

            SetHits(300, 400);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 30, 40);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 20, 30);

            SetSkill(SkillName.Musicianship, 90.1, 100.0);
            SetSkill(SkillName.Provocation, 90.1, 100.0);
            SetSkill(SkillName.MagicResist, 75.0, 100.0);
            SetSkill(SkillName.Tactics, 60.0, 80.0);
            SetSkill(SkillName.Wrestling, 70.0, 90.0);

            Fame = 5000;
            Karma = -5000;

            VirtualArmor = 25;

            m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
        }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextSpeechTime)
            {
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
                {
                    int phrase = Utility.Random(3);

                    switch (phrase)
                    {
                        case 0: this.Say(true, "Dance to my tune!"); break;
                        case 1: this.Say(true, "Let the music guide you!"); break;
                        case 2: this.Say(true, "Fight for my amusement!"); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;

                }

                base.OnThink();
            }
        }

        public override void GenerateLoot()
        {
            PackGold(200, 300);
            AddLoot(LootPack.Average);
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        public SatyrPiper(Serial serial) : base(serial)
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
