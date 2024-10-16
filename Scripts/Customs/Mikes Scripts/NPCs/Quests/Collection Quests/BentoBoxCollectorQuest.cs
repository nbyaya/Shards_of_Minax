using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class BentoBoxCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Bento Box Collector"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Kaito, the Culinary Alchemist. My quest is to gather 50 Bento Boxes, " +
                       "each filled with unique ingredients from across the land. These Bento Boxes are vital for my grand feast, " +
                       "which will be held to honor the spirits of our ancestors. Your help will be rewarded with gold, a rare Maxxia Scroll, " +
                       "and a special Bento Chef's Attire that will mark you as a master of the culinary arts.";
            }
        }

        public override object Refuse { get { return "Very well. Should you change your mind, return to me with the Bento Boxes."; } }

        public override object Uncomplete { get { return "I still require 50 Bento Boxes. Please bring them to me to aid in the preparation of the grand feast!"; } }

        public override object Complete { get { return "Fantastic! You have brought me the 50 Bento Boxes I needed. Your assistance is invaluable. " +
                       "Please accept these rewards as a token of my gratitude. May your culinary endeavors be ever successful!"; } }

        public BentoBoxCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(BentoBox), "Bento Boxes", 50, 0x2836)); // Assuming BentoBox item ID is 0xF6C
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(ArchersStuddedLeggingsOfAgility), 1, "Bento Chef's Leggings")); // Assuming Bento Chef's Attire is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Bento Box Collector quest!");
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

    public class BentoChefKaito : MondainQuester
    {
        [Constructable]
        public BentoChefKaito()
            : base("The Culinary Alchemist", "Kaito")
        {
        }

        public BentoChefKaito(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x2045; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Shirt { Hue = Utility.Random(1, 3000), Name = "Kaito's Bento Chef Robe" });
            AddItem(new Kilt { Hue = Utility.Random(1, 3000) });
            AddItem(new JesterHat { Hue = Utility.Random(1, 3000), Name = "Kaito's Culinary Hat" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000) });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Kaito's Recipe Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(BentoBoxCollectorQuest)
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
