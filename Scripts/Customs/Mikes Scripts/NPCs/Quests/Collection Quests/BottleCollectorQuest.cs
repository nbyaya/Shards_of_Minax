using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class BottleCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Curator's Collection"; } }

        public override object Description
        {
            get
            {
                return "Greetings, adventurer! I am Olwyn the Curator, keeper of rare and ancient artifacts. My collection is missing a crucial item: bottles. " +
                       "I need 500 bottles to complete a new display for my collection. Will you assist me in gathering them? In return, I will reward you with gold, " +
                       "a rare Maxxia Scroll, and a unique Curator's Cloak that is both striking and stylish.";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, come back and see me. My collection awaits completion!"; } }

        public override object Uncomplete { get { return "I still require 500 bottles to complete my collection. Please bring them to me as soon as possible!"; } }

        public override object Complete { get { return "Excellent work! You've gathered all 500 bottles. My collection is now complete, and your contribution is greatly appreciated. " +
                       "Please accept these rewards as a token of my gratitude. Thank you!"; } }

        public BottleCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(Bottle), "Bottle", 500, 0xF0E)); // Assuming Bottle item ID is 0x1F9
            AddReward(new BaseReward(typeof(Gold), 7000, "7000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(BladesDancersChest), 1, "Curator's Chest")); // Assuming Curator's Cloak is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Curator's Collection quest!");
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

    public class BottleCollectorOlwyn : MondainQuester
    {
        [Constructable]
        public BottleCollectorOlwyn()
            : base("The Curator", "Bottle Collector Olwyn")
        {
        }

        public BottleCollectorOlwyn(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Body = 0x191; // Female Body

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x2044; // Elegant hair style
            HairHue = 1151; // Hair hue (light blue)
        }

        public override void InitOutfit()
        {
            AddItem(new Sandals(1152)); // Matching sandals
            AddItem(new Cloak { Name = "Olwyn's Curator's Cloak", Hue = 1152 }); // Custom Cloak
            AddItem(new FancyShirt(1152)); // Fancy shirt
            AddItem(new HoodedShroudOfShadows { Name = "Curator's Hood", Hue = 1152 }); // Custom Hood
            Backpack backpack = new Backpack();
            backpack.Hue = 1152;
            backpack.Name = "Curator's Collection Bag";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(BottleCollectorQuest)
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
