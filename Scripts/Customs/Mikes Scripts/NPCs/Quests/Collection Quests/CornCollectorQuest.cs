using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class CornCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Corn Collector's Challenge"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Grundle, the Corn Collector. My village has been struck by a terrible drought, " +
                       "and our precious corn supply is dwindling. I need your help to gather 50 Wooden Bowls of Corn. These bowls will be used " +
                       "to sustain our people through these harsh times. In return, you shall be rewarded with gold, a rare Maxxia Scroll, and " +
                       "the legendary Corn Collector's Garb. Your assistance is not only appreciated, but vital to our survival!";
            }
        }

        public override object Refuse { get { return "Very well. Should you reconsider, return to me with the Wooden Bowls of Corn."; } }

        public override object Uncomplete { get { return "I still need 50 Wooden Bowls of Corn. Please gather them to aid our village!"; } }

        public override object Complete { get { return "Fantastic work! You have brought the 50 Wooden Bowls of Corn I needed. Your help is invaluable. " +
                       "Please accept these rewards as a token of my gratitude. May your journey be filled with prosperity!"; } }

        public CornCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(WoodenBowlOfCorn), "Wooden Bowls of Corn", 50, 0x15FA)); // Assuming Wooden Bowl of Corn item ID is 0x1E2
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(TechnicosTassets), 1, "Corn Collector's Garb")); // Assuming Corn Collector's Garb is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Corn Collector's Challenge!");
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

    public class CornCollectorGrundle : MondainQuester
    {
        [Constructable]
        public CornCollectorGrundle()
            : base("The Corn Collector", "Grundle")
        {
        }

        public CornCollectorGrundle(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203B; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Sandals { Hue = Utility.Random(1, 3000), Name = "Grundle's Sandals" });
            AddItem(new Kilt { Hue = Utility.Random(1, 3000), Name = "Grundle's Corn Kilt" });
            AddItem(new Shirt { Hue = Utility.Random(1, 3000), Name = "Grundle's Corn Shirt" });
            AddItem(new FeatheredHat { Hue = Utility.Random(1, 3000), Name = "Grundle's Corn Hat" });
            AddItem(new GoldRing { Hue = Utility.Random(1, 3000), Name = "Grundle's Lucky Ring" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Grundle's Corn Sack";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(CornCollectorQuest)
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
