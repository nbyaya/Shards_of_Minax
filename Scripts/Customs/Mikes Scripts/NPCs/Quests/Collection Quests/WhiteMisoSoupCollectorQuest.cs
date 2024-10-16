using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class WhiteMisoSoupCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Legendary White Miso Soup"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Kuroda, the Culinary Sage. In the realm of gastronomy, there is a dish of legend—" +
                       "the White Miso Soup. It is said to hold mystical properties that can rejuvenate the spirit and heal the soul. I need " +
                       "your help to collect 50 bowls of this sacred soup. In return for your effort, I shall reward you with gold, a rare Maxxia Scroll, " +
                       "and an exquisite Culinary Sage's Ensemble that will mark you as a true connoisseur of fine cuisine.";
            }
        }

        public override object Refuse { get { return "Very well. Should you change your mind, return to me with the White Miso Soup."; } }

        public override object Uncomplete { get { return "I still require 50 bowls of White Miso Soup. Return to me when you have collected them."; } }

        public override object Complete { get { return "Fantastic! You have brought me the 50 bowls of White Miso Soup I requested. Your dedication to the culinary arts is commendable. " +
                       "Please accept these rewards as a token of my gratitude. May your adventures be filled with delightful feasts!"; } }

        public WhiteMisoSoupCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(WhiteMisoSoup), "White Miso Soup", 50, 0x284E)); // Assuming White Miso Soup item ID is 0x1F0
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(GarbOfTheGrandCouturier), 1, "Culinary Sage's Ensemble")); // Assuming Culinary Sage's Ensemble is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Legendary White Miso Soup quest!");
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

    public class CulinarySageKuroda : MondainQuester
    {
        [Constructable]
        public CulinarySageKuroda()
            : base("The Culinary Sage", "Kuroda")
        {
        }

        public CulinarySageKuroda(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x204E; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt { Hue = Utility.Random(1, 3000), Name = "Kuroda's Culinary Tunic" });
            AddItem(new LongPants { Hue = Utility.Random(1, 3000) });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new FeatheredHat { Hue = Utility.Random(1, 3000), Name = "Kuroda's Sage Hat" });
            AddItem(new GoldBracelet { Hue = Utility.Random(1, 3000), Name = "Kuroda's Golden Bracelet" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Kuroda's Culinary Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(WhiteMisoSoupCollectorQuest)
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
