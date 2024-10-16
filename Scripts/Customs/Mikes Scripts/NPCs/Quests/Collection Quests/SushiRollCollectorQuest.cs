using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class SushiRollCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Sushi Roll Collection"; } }

        public override object Description
        {
            get
            {
                return "Greetings, esteemed traveler! I am Sushi Master Iori, keeper of the ancient recipes and secrets of sushi. " +
                       "My sacred recipe book has been lost, and without it, my culinary skills are diminished. " +
                       "To aid me in restoring my art, I require you to collect 50 Sushi Rolls. These rolls are the essence of my craft " +
                       "and are vital for my restoration. In gratitude for your assistance, I shall reward you with gold, a rare Maxxia Scroll, " +
                       "and a uniquely crafted Sushi Master’s Gloves.";
            }
        }

        public override object Refuse { get { return "I understand. Should you change your mind, return with the Sushi Rolls."; } }

        public override object Uncomplete { get { return "I am still in need of 50 Sushi Rolls. Please bring them to me to restore my culinary prowess!"; } }

        public override object Complete { get { return "Wonderful! You have brought me the 50 Sushi Rolls I required. Your help is truly appreciated. " +
                       "Please accept these rewards as a token of my gratitude. May your travels be as fulfilling as a well-crafted sushi roll!"; } }

        public SushiRollCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(SushiRolls), "Sushi Rolls", 50, 0x283E)); // Assuming Sushi Roll item ID is 0x16D8
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(ArchmagesGloves), 1, "Sushi Master’s Gloves")); // Assuming Sushi Master’s Kimono is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Sushi Roll Collection quest!");
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

    public class SushiMasterIori : MondainQuester
    {
        [Constructable]
        public SushiMasterIori()
            : base("The Sushi Master", "Iori")
        {
        }

        public SushiMasterIori(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x2041; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Shirt { Hue = Utility.Random(1, 3000), Name = "Iori's Sushi Master Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new FeatheredHat { Hue = Utility.Random(1, 3000), Name = "Iori's Mastery Hat" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000), Name = "Iori's Crafting Gloves" });
            AddItem(new Kilt { Hue = Utility.Random(1, 3000), Name = "Iori's Sushi Apron" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Iori's Culinary Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(SushiRollCollectorQuest)
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
