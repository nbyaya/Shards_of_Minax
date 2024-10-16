using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class CocoaLiquorCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Cocoa Liquor Connoisseur"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave traveler! I am Roderick, the renowned Cocoa Liquor Connoisseur. " +
                       "I am on a quest to find the finest Cocoa Liquor for a grand celebration. I require 50 bottles of " +
                       "this exquisite drink to complete my collection. Your assistance will be richly rewarded with gold, a rare " +
                       "Maxxia Scroll, and a specially crafted outfit that reflects the splendor of my collection.";
            }
        }

        public override object Refuse { get { return "Very well. Should you change your mind, return to me with the Cocoa Liquor."; } }

        public override object Uncomplete { get { return "I still need 50 bottles of Cocoa Liquor. Please bring them to me so I can complete my collection!"; } }

        public override object Complete { get { return "Splendid! You've brought me the 50 bottles of Cocoa Liquor I needed. Your effort is truly appreciated. " +
                       "Accept these rewards as a token of my gratitude. May your adventures be as delightful as this fine liquor!"; } }

        public CocoaLiquorCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(CocoaLiquor), "Cocoa Liquor", 50, 0x103F)); // Assuming Cocoa Liquor item ID is 0x1F5
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(WarHeronsCap), 1, "Roderick's Celebration Attire")); // Assuming Roderick's Celebration Attire is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Cocoa Liquor Connoisseur quest!");
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

    public class Roderick : MondainQuester
    {
        [Constructable]
        public Roderick()
            : base("The Cocoa Liquor Connoisseur", "Roderick")
        {
        }

        public Roderick(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x2044; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new ShortPants { Hue = Utility.Random(1, 3000), Name = "Roderick's Elegant Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new FeatheredHat { Hue = Utility.Random(1, 3000), Name = "Roderick's Fine Hat" });
            AddItem(new GoldRing { Hue = Utility.Random(1, 3000), Name = "Roderick's Golden Ring" });
            AddItem(new FancyShirt { Hue = Utility.Random(1, 3000), Name = "Roderick's Celebration Shirt" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Roderick's Celebration Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(CocoaLiquorCollectorQuest)
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
