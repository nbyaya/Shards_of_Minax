using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class DullCopperCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Dull Copper Challenge"; } }

        public override object Description
        {
            get
            {
                return "Hail, brave traveler! I am Gorram the Stout, a seeker of ancient knowledge and artifacts. Long ago, " +
                       "an ancient empire left behind treasures made from a rare metal known as Dull Copper. To restore the lost glory " +
                       "of this ancient civilization, I require 50 Dull Copper Ores. In return for your invaluable aid, you shall be rewarded " +
                       "with gold, a rare Maxxia Scroll, and the illustrious Gorram's Collector's Gear, imbued with the essence of the olden days.";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, return to me with the Dull Copper Ores."; } }

        public override object Uncomplete { get { return "I still require 50 Dull Copper Ores to continue my research. Please bring them to me!"; } }

        public override object Complete { get { return "Magnificent! You have gathered the 50 Dull Copper Ores I needed. Your dedication to restoring the ancient legacy is admirable. " +
                       "As a token of my gratitude, accept these rewards. May your journey be ever victorious!"; } }

        public DullCopperCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(DullCopperOre), "Dull Copper Ores", 50, 0x19B7)); // Assuming Dull Copper Ore item ID is 0x19B2
            AddReward(new BaseReward(typeof(Gold), 6000, "6000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(BardsTunicOfStonehenge), 1, "Gorram's Collector's Gear")); // Assuming Gorram's Collector's Gear is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed The Dull Copper Challenge quest!");
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

    public class GorramTheStout : MondainQuester
    {
        [Constructable]
        public GorramTheStout()
            : base("The Ancient Seeker", "Gorram")
        {
        }

        public GorramTheStout(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(120, 120, 50);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x2042; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new PlateChest { Hue = Utility.Random(1, 3000), Name = "Gorram's Ancient Plate" });
            AddItem(new PlateLegs { Hue = Utility.Random(1, 3000) });
            AddItem(new PlateHelm { Hue = Utility.Random(1, 3000), Name = "Gorram's Stalwart Helm" });
            AddItem(new PlateGloves { Hue = Utility.Random(1, 3000) });
            AddItem(new PlateGorget { Hue = Utility.Random(1, 3000), Name = "Gorram's Ancient Gorget" });
            AddItem(new PlateArms { Hue = Utility.Random(1, 3000) });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Gorram's Treasure Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(DullCopperCollectorQuest)
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
