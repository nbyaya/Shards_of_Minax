using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class SoupQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Legendary Tomato Soup"; } }

        public override object Description
        {
            get
            {
                return "Greetings, noble traveler! I am Arlo, the Soup Enthusiast. My culinary quest involves a legendary recipe for " +
                       "Tomato Soup that has been lost to time. I require 50 Wooden Bowls of Tomato Soup to recreate this ancient delicacy. " +
                       "Your assistance will be rewarded with gold, a rare Maxxia Scroll, and a unique Soup Enthusiast's Ensemble.";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, return to me with the Wooden Bowls of Tomato Soup."; } }

        public override object Uncomplete { get { return "I am still in need of 50 Wooden Bowls of Tomato Soup. Bring them to me so I can complete my culinary masterpiece!"; } }

        public override object Complete { get { return "Wonderful! You have brought me the 50 Wooden Bowls of Tomato Soup I needed. Your contribution is greatly appreciated. " +
                       "Please accept these rewards as a token of my gratitude. May your culinary adventures be as rich as the soups you’ve helped create!"; } }

        public SoupQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(WoodenBowlOfTomatoSoup), "Wooden Bowls of Tomato Soup", 50, 0x1606)); // Assuming Wooden Bowl of Tomato Soup item ID is 0x1F4
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(ChefsAegisApron), 1, "Soup Enthusiast's Ensemble")); // Assuming Soup Enthusiast's Ensemble is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Legendary Tomato Soup quest!");
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

    public class SoupEnthusiastArlo : MondainQuester
    {
        [Constructable]
        public SoupEnthusiastArlo()
            : base("The Soup Enthusiast", "Arlo")
        {
        }

        public SoupEnthusiastArlo(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x2047; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Arlo's Chef Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new Bandana { Hue = Utility.Random(1, 3000), Name = "Arlo's Cooking Bandana" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000), Name = "Arlo's Cooking Gloves" });
            AddItem(new FullApron { Hue = Utility.Random(1, 3000), Name = "Arlo's Chef Apron" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Arlo's Culinary Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(SoupQuest)
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
