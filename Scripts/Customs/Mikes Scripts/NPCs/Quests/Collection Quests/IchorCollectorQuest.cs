using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class IchorCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Alchemist's Urgent Request"; } }

        public override object Description
        {
            get
            {
                return "Ah, brave adventurer! I am Draven, an alchemist of great renown. My latest concoction requires a rare " +
                       "ingredient that can only be obtained from the depths of the earth. I need 50 bottles of Ichor to complete " +
                       "my elixir of enhancement. Can you assist me in gathering this precious resource? In return, you will be " +
                       "rewarded with a generous sum of gold, a rare Maxxia Scroll, and an Alchemist's Mask that is both mystical " +
                       "and elegant.";
            }
        }

        public override object Refuse { get { return "Very well, adventurer. Should you change your mind, I shall be here, waiting for your assistance."; } }

        public override object Uncomplete { get { return "I still need 50 bottles of Ichor. Please bring them to me at once so I can complete my work."; } }

        public override object Complete { get { return "Marvelous! You've gathered all the Ichor I need. My elixir will be a great success thanks to you. " +
                       "Please accept these rewards as a token of my gratitude. Thank you for your help!"; } }

        public IchorCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(BottleIchor), "Bottle of Ichor", 50, 0x5748)); // Assuming BottleIchor item ID is 0x1D8
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(LuchadorsMask), 1, "Alchemist's Mask")); // Assuming Alchemist's Robe is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Alchemist's Urgent Request quest!");
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

    public class IchorCollectorDraven : MondainQuester
    {
        [Constructable]
        public IchorCollectorDraven()
            : base("The Alchemist", "Ichor Collector Draven")
        {
        }

        public IchorCollectorDraven(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Body = 0x190; // Male Body

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x203C; // Alchemist's hair style
            HairHue = 1153; // Hair hue (deep purple)
        }

        public override void InitOutfit()
        {
            AddItem(new Robe(1153)); // Custom robe
            AddItem(new Sandals(1153)); // Matching sandals
            AddItem(new WizardsHat { Name = "Draven's Mystical Hat", Hue = 1153 }); // Custom wizard hat
            AddItem(new GoldBracelet { Name = "Alchemist's Gold Bracelet", Hue = 1153 }); // Custom bracelet
            AddItem(new GoldNecklace { Name = "Alchemist's Gold Necklace", Hue = 1153 }); // Custom necklace
            Backpack backpack = new Backpack();
            backpack.Hue = 1153;
            backpack.Name = "Bag of Alchemical Ingredients";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(IchorCollectorQuest)
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
