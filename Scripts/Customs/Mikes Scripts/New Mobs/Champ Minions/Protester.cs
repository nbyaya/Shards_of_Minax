using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a protester")]
    public class Protester : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0); // time between protester speech
        public DateTime m_NextSpeechTime;

        [Constructable]
        public Protester() : base(AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Protester";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Protester";
            }

            Item shirt = new Shirt(Utility.RandomNeutralHue());
            Item pants = new LongPants(Utility.RandomNeutralHue());
            Item shoes = new Shoes(Utility.RandomNeutralHue());
            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;

            AddItem(shirt);
            AddItem(pants);
            AddItem(shoes);
            AddItem(hair);

            if (!this.Female)
            {
                Item beard = new Item(Utility.RandomList(0x203E, 0x2041));
                beard.Hue = hair.Hue;
                beard.Layer = Layer.FacialHair;
                beard.Movable = false;
                AddItem(beard);
            }

            SetStr(100, 150);
            SetDex(60, 80);
            SetInt(50, 70);

            SetHits(120, 160);

            SetDamage(1, 3);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 10, 20);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 10, 20);
            SetResistance(ResistanceType.Poison, 10, 20);
            SetResistance(ResistanceType.Energy, 10, 20);

            SetSkill(SkillName.Wrestling, 20.0, 40.0);
            SetSkill(SkillName.Tactics, 20.0, 40.0);

            Fame = 0;
            Karma = 0;

            VirtualArmor = 10;

            m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
        }

        public override bool CanRummageCorpses { get { return false; } }
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
                        case 0: this.Say(true, "No justice, no peace!"); break;
                        case 1: this.Say(true, "Stand your ground!"); break;
                        case 2: this.Say(true, "We won't be moved!"); break;
                        case 3: this.Say(true, "Fight the power!"); break;
                    }
                    
                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }

                base.OnThink();
            }
        }

        public override void GenerateLoot()
        {
            PackItem(new Bandage(Utility.RandomMinMax(1, 15)));
            PackGold(50, 100);
        }

        public Protester(Serial serial) : base(serial)
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
