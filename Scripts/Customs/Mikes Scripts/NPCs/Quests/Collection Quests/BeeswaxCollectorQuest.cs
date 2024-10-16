using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class BeeswaxCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Beekeeper's Bounty"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Elowen, a humble beekeeper with a grand plan to craft exquisite candles " +
                       "for the upcoming festival. However, I am in dire need of beeswax to complete my work. Could you gather " +
                       "50 pieces of beeswax for me? In return, I will reward you handsomely with gold, a rare Maxxia Scroll, and a " +
                       "unique Beekeeper's Vest that will surely make you stand out!";
            }
        }

        public override object Refuse { get { return "I understand if you can't help me now. But should you change your mind, I'll be here with open arms!"; } }

        public override object Uncomplete { get { return "I still require 50 pieces of beeswax. Please bring them to me as soon as possible!"; } }

        public override object Complete { get { return "Wonderful! You've gathered all the beeswax I need. Your assistance is greatly appreciated. Please accept these rewards as a token of my gratitude."; } }

        public BeeswaxCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(Beeswax), "Beeswax", 50, 0x1422)); // Assuming Beeswax item ID is 0x1D3
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(HackersVRGoggles), 1, "Beekeeper's Goggles")); // Assuming Beekeeper's Vest is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Beekeeper's Bounty quest!");
            Owner.PlaySound(CompleteSound);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class BeeswaxCollectorElowen : MondainQuester
    {
        [Constructable]
        public BeeswaxCollectorElowen()
            : base("The Beekeeper", "Beeswax Collector Elowen")
        {
        }

        public BeeswaxCollectorElowen(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Body = 0x191; // Female Body

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x203D; // Custom hair style
            HairHue = 1153; // Hair hue (dark brown)
        }

        public override void InitOutfit()
        {
            AddItem(new Robe(0x3B2)); // Custom color robe
            AddItem(new Sandals(0x3B2)); // Matching sandals
            AddItem(new Bandana { Name = "Elowen's Bandana", Hue = 0x3B2 }); // Custom Bandana
            AddItem(new OrderShield { Name = "Elowen's Beekeeper Shield", Hue = 0x3B2 }); // Custom Shield
            AddItem(new QuarterStaff { Name = "Elowen's Staff of Nectar", Hue = 0x3B2 }); // Custom Staff
            Backpack backpack = new Backpack();
            backpack.Hue = 0x3B2;
            backpack.Name = "Bag of Beekeeper's Tools";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(BeeswaxCollectorQuest)
                };
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
