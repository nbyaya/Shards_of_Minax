using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class FishCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Grand Fish Collector"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Marlin, the Great Fisherman. I need your help to collect 500 Fish. " +
                       "These fish are essential for my grand feast, and I cannot complete it without them. In return for your efforts, " +
                       "I will reward you with gold, a rare Maxxia Scroll, and a unique outfit fit for a true champion of the seas.";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, return to me with the Fish."; } }

        public override object Uncomplete { get { return "I still require 500 Fish. Bring them to me to aid in my grand feast preparation!"; } }

        public override object Complete { get { return "Excellent! You have brought me the 500 Fish I needed. Your assistance is invaluable. " +
                       "Please accept these rewards as a token of my gratitude. May you always have good fortune in your fishing endeavors!"; } }

        public FishCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(Fish), "Fish", 500, 0x09DD)); // Assuming Fish item ID is 0x09CC
            AddReward(new BaseReward(typeof(Gold), 10000, "10000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(ChampionsLeggingsOfParry), 1, "Fish Champion's Outfit")); // Assuming Fish Champion's Outfit is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Grand Fish Collector quest!");
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

    public class GreatFishermanMarlin : MondainQuester
    {
        [Constructable]
        public GreatFishermanMarlin()
            : base("The Great Fisherman", "Marlin")
        {
        }

        public GreatFishermanMarlin(Serial serial)
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
            AddItem(new Shirt { Hue = Utility.Random(1, 3000), Name = "Marlin's Fishing Tunic" });
            AddItem(new ShortPants { Hue = Utility.Random(1, 3000) });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new FeatheredHat { Hue = Utility.Random(1, 3000), Name = "Marlin's Feathered Hat" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000) });
            AddItem(new Backpack { Hue = Utility.Random(1, 3000), Name = "Marlin's Fishing Pack" });
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(FishCollectorQuest)
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
