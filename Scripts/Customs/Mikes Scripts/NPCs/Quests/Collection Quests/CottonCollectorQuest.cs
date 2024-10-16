using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class CottonCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Weaver's Request"; } }

        public override object Description
        {
            get
            {
                return "Greetings, adventurer! I am Elowen, a skilled weaver and tailor of fine fabrics. Currently, I am in dire need " +
                       "of cotton to create some exquisite garments. I require 50 pieces of cotton to continue my work. Can you help me " +
                       "gather this cotton? In return, I will reward you with gold, a rare Maxxia Scroll, and a unique Tailor's Hat that " +
                       "will surely make you stand out!";
            }
        }

        public override object Refuse { get { return "Very well, if you change your mind, feel free to return and assist me in my quest."; } }

        public override object Uncomplete { get { return "I still need 50 pieces of cotton to continue my weaving. Please bring them to me soon!"; } }

        public override object Complete { get { return "Wonderful! You've gathered all the cotton I needed. Your help is greatly appreciated. Here are your rewards: gold, a Maxxia Scroll, and a Tailor's Hat. Thank you!"; } }

        public CottonCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(Cotton), "Cotton", 50, 0xDF9)); // Assuming Cotton item ID is 0x1BF
            AddReward(new BaseReward(typeof(Gold), 3000, "3000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(FishermansSunHat), 1, "Tailor's Hat")); // Assuming Tailor's Hat is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed The Weaver's Request quest!");
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

    public class CottonCollectorElowen : MondainQuester
    {
        [Constructable]
        public CottonCollectorElowen()
            : base("The Weaver", "Cotton Collector Elowen")
        {
        }

        public CottonCollectorElowen(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Body = 0x191; // Female Body

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x203B; // Weaver's hair style
            HairHue = 1162; // Hair hue (soft brown)
        }

        public override void InitOutfit()
        {
            AddItem(new Robe(1153)); // Elegant robe
            AddItem(new Sandals(1153)); // Matching sandals
            AddItem(new Cloak { Name = "Elowen's Weaving Cloak", Hue = 1153 }); // Custom Weaving Cloak
            AddItem(new TallStrawHat { Name = "Elowen's Tailor's Hat", Hue = 1153 }); // Custom Tailor's Hat
            AddItem(new SewingKit { Name = "Elowen's Sewing Kit", Hue = 1153 }); // Custom Sewing Kit
            Backpack backpack = new Backpack();
            backpack.Hue = 1153;
            backpack.Name = "Elowen's Weaving Supplies";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(CottonCollectorQuest)
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
