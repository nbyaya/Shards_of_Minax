using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class EllieHearthsteadQuest : BaseQuest
    {
        public override object Title { get { return "A Farmer's Future"; } }

        public override object Description
        {
            get
            {
                return 
                    "Please, can you help me? I'm Ellie Hearthstead, and I've lost everything. The monsters came from the woods last night, burning our land and scaring off my kin. But I have these—my family's heirloom seeds. I need to reach my new farmland to start again. Will you escort me safely there?";
            }
        }

        public override object Refuse { get { return "I understand... not everyone can leave the road to help a humble farmer."; } }
        public override object Uncomplete { get { return "Please, let's not delay... these seeds won't last forever, and I fear the monsters might follow."; } }

        public EllieHearthsteadQuest() : base()
        {
            AddObjective(new EscortObjective("some Farmlands"));
            AddReward(new BaseReward(typeof(TacticalVest), "TacticalVest – Light armor offering protection and farming bonuses"));
        }

        public override void GiveRewards()
        {
            base.GiveRewards();
            Owner.SendMessage("Thank you, friend. You've given me hope. Take this vest—may it protect you as you've protected me.", null, 0x59B);
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

    public class EllieHearthsteadEscort : BaseEscort
    {
        public override Type[] Quests { get { return new Type[] { typeof(EllieHearthsteadQuest) }; } }
        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBFarmer());
        }

        private DateTime m_NextTalkTime;

        [Constructable]
        public EllieHearthsteadEscort() : base()
        {
            Name = "Ellie Hearthstead";
            Title = "the Displaced Farmer";
            NameHue = 0x83F;
        }

		public EllieHearthsteadEscort(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(45, 40, 35);
            Female = true;
            CantWalk = false;
            Race = Race.Human;
            Hue = 0x83EA;
            HairItemID = 0x2049;
            HairHue = 1153;
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 0x59C });
            AddItem(new LongPants() { Hue = 0x1BB });
            AddItem(new HalfApron() { Hue = 0x5E2 });
            AddItem(new StrawHat() { Hue = 0x47F });
            AddItem(new Sandals() { Hue = 0x1BB });
            AddItem(new Backpack());

            Item seeds = new HeirloomSeeds();
            seeds.Movable = false;
            AddItem(seeds);
        }

        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.Now >= m_NextTalkTime && this.Controlled)
            {
                if (Utility.RandomDouble() < 0.1)
                {
                    string[] lines = new string[]
                    {
                        "'These seeds... they’ve been in my family for generations.'",
                        "'I hope the soil at the new land is as kind as the old.'",
                        "*Ellie clutches her apron tightly* 'The night they came, I barely escaped with my life...'",
                        "'I’ll plant them in neat rows, just like my mother taught me.'",
                        "'Do you think monsters fear the sun? I do hope daylight keeps them at bay.'",
                        "'I heard whispers of other farmers settling nearby... maybe I won’t be alone.'",
                        "'Once I sow the first seeds, it will truly feel like home again.'"
                    };

                    Say(lines[Utility.Random(lines.Length)]);
                    m_NextTalkTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 30));
                }
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(m_NextTalkTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_NextTalkTime = reader.ReadDateTime();
        }
    }

    public class HeirloomSeeds : Item
    {
        [Constructable]
        public HeirloomSeeds() : base(0xC63) // Using the wheat sheaf graphic as a placeholder
        {
            Name = "a bundle of heirloom seeds";
            Hue = 0x59C;
            Weight = 1.0;
        }

        public HeirloomSeeds(Serial serial) : base(serial) { }

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
