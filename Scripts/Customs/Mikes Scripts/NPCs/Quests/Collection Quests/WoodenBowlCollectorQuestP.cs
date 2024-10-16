using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class WoodenBowlCollectorQuestP : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Legendary Pea Bowl Collector"; } }

        public override object Description
        {
            get
            {
                return "Hail, brave wanderer! I am Orin, the Culinary Curator of the Arcane Pantry. My enchanted collection is missing a key ingredient: " +
                       "50 Wooden Bowls of Peas. These bowls are imbued with ancient flavors that are vital to the preservation of my magical recipes. " +
                       "Should you bring me these bowls, I will reward you with a trove of gold, a rare Maxxia Scroll, and a magnificent Chef's Garb, " +
                       "woven with the finest threads and enchanted with culinary magic.";
            }
        }

        public override object Refuse { get { return "I see. If you change your mind, return to me with the Wooden Bowls of Peas."; } }

        public override object Uncomplete { get { return "I still need 50 Wooden Bowls of Peas. The magic of my pantry is incomplete without them!"; } }

        public override object Complete { get { return "Wonderful! You have brought me the 50 Wooden Bowls of Peas. Your assistance has restored the magic to my collection. " +
                       "Accept these rewards as a token of my appreciation. May your travels be as delightful as a well-cooked feast!"; } }

        public WoodenBowlCollectorQuestP() : base()
        {
            AddObjective(new ObtainObjective(typeof(WoodenBowlOfPeas), "Wooden Bowls of Peas", 50, 0x15FC)); // Assuming Wooden Bowl of Peas item ID is 0x2E4
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(BlademastersChestplate), 1, "Chef's Garb")); // Assuming Chef's Garb is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Legendary Pea Bowl Collector quest!");
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

    public class CulinaryCuratorOrin : MondainQuester
    {
        [Constructable]
        public CulinaryCuratorOrin()
            : base("The Culinary Curator", "Orin")
        {
        }

        public CulinaryCuratorOrin(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x191; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x2042; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Shirt { Hue = Utility.Random(1, 3000), Name = "Orin's Enchanted Chef Tunic" });
            AddItem(new Kilt { Hue = Utility.Random(1, 3000), Name = "Orin's Chef Kilt" });
            AddItem(new JesterHat { Hue = Utility.Random(1, 3000), Name = "Orin's Culinary Hat" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000), Name = "Orin's Cooking Sandals" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000), Name = "Orin's Cooking Gloves" });
            AddItem(new FullApron { Hue = Utility.Random(1, 3000), Name = "Orin's Magical Apron" });

            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Orin's Recipe Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(WoodenBowlCollectorQuestP)
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
