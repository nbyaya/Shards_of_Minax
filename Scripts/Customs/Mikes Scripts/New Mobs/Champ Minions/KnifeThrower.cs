using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a knife thrower")]
    public class KnifeThrower : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0); // time between knife thrower speech
        public DateTime m_NextSpeechTime;

        [Constructable]
        public KnifeThrower() : base(AIType.AI_Archer, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();
			Team = 2;

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Knife Thrower";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Knife Thrower";
            }

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            Item shirt = new Shirt(Utility.RandomNeutralHue());
            Item pants = new LongPants(Utility.RandomNeutralHue());
            Item boots = new Boots(Utility.RandomNeutralHue());
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;

            AddItem(hair);
            AddItem(shirt);
            AddItem(pants);
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
            SetDex(200, 300);
            SetInt(100, 200);

            SetHits(500, 700);

            SetDamage(10, 20);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 50, 70);
            SetResistance(ResistanceType.Fire, 40, 60);
            SetResistance(ResistanceType.Cold, 30, 50);
            SetResistance(ResistanceType.Poison, 20, 40);
            SetResistance(ResistanceType.Energy, 20, 40);

            SetSkill(SkillName.Anatomy, 75.1, 100.0);
            SetSkill(SkillName.Archery, 90.1, 120.0);
            SetSkill(SkillName.Tactics, 85.1, 100.0);
            SetSkill(SkillName.MagicResist, 80.1, 95.0);

            Fame = 5000;
            Karma = -5000;

            VirtualArmor = 40;

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
                        case 0: this.Say(true, "You can't dodge my knives!"); break;
                        case 1: this.Say(true, "Feel the sting of my blades!"); break;
                        case 2: this.Say(true, "You won't see the next one coming!"); break;
                        case 3: this.Say(true, "I never miss my mark!"); break;
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
                case 0: this.Say(true, "My knives... failed me..."); break;
                case 1: this.Say(true, "You'll... pay..."); break;
            }

            PackItem(new IronIngot(Utility.RandomMinMax(5, 15)));
        }

        public KnifeThrower(Serial serial) : base(serial)
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
