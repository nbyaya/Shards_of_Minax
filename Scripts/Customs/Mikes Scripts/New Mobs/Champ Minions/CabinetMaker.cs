using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a cabinet maker")]
    public class CabinetMaker : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0); // time between speech
        public DateTime m_NextSpeechTime;

        [Constructable]
        public CabinetMaker() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Cabinet Maker";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Cabinet Maker";
            }

            Item shirt = new Shirt(Utility.RandomNeutralHue());
            Item pants = new LongPants(Utility.RandomNeutralHue());
            Item shoes = new Shoes(Utility.RandomNeutralHue());
            Item apron = new FullApron(Utility.RandomNeutralHue());
            
            AddItem(shirt);
            AddItem(pants);
            AddItem(shoes);
            AddItem(apron);

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

            SetStr(500, 700);
            SetDex(150, 250);
            SetInt(300, 400);

            SetHits(400, 600);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 30, 50);
            SetResistance(ResistanceType.Fire, 20, 40);
            SetResistance(ResistanceType.Cold, 20, 40);
            SetResistance(ResistanceType.Poison, 20, 40);
            SetResistance(ResistanceType.Energy, 20, 40);

            SetSkill(SkillName.Carpentry, 90.0, 100.0);
            SetSkill(SkillName.Tinkering, 80.0, 100.0);
            SetSkill(SkillName.Blacksmith, 70.0, 90.0);
            SetSkill(SkillName.Lumberjacking, 60.0, 80.0);
            SetSkill(SkillName.MagicResist, 75.0, 100.0);
            SetSkill(SkillName.Tactics, 60.0, 80.0);
            SetSkill(SkillName.Wrestling, 70.0, 90.0);

            Fame = 5000;
            Karma = -5000;

            VirtualArmor = 40;

            m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
        }

        public override bool AlwaysMurderer { get { return false; } }
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
                        case 0: this.Say(true, "I'll build your doom!"); break;
                        case 1: this.Say(true, "My craftsmanship is unmatched!"); break;
                        case 2: this.Say(true, "Prepare to be sieged!"); break;
                        case 3: this.Say(true, "You can't break my creations!"); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }

                base.OnThink();
            }
        }

        public override void GenerateLoot()
        {
            PackGem();
            PackGold(200, 250);
            AddLoot(LootPack.Rich);

            int phrase = Utility.Random(2);

            switch (phrase)
            {
                case 0: this.Say(true, "My work... unfinished..."); break;
                case 1: this.Say(true, "My tools... they'll find another..."); break;
            }

            PackItem(new Board(Utility.RandomMinMax(10, 20)));
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
                        case 0: this.Say(true, "You think that hurts?"); break;
                        case 1: this.Say(true, "I'll build a better defense!"); break;
                        case 2: this.Say(true, "My creations are tougher!"); break;
                        case 3: this.Say(true, "You'll need more than that!"); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }
            }

            return base.Damage(amount, from);
        }

        public CabinetMaker(Serial serial) : base(serial)
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
