using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class CrystallineBlackrockCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Crystalline Conundrum"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Arion, the Crystal Sage. I need your aid to gather 50 Crystalline Blackrocks, " +
                       "which are crucial for my research on the ancient magic of the realm. In return for your efforts, you will be rewarded " +
                       "with gold, a rare Maxxia Scroll, and a unique outfit that reflects the beauty of the crystals.";
            }
        }

        public override object Refuse { get { return "I understand. If you change your mind, return to me with the blackrocks."; } }

        public override object Uncomplete { get { return "I am still in need of 50 Crystalline Blackrocks. Please bring them to me so that I may proceed with my research!"; } }

        public override object Complete { get { return "Excellent! You have brought me the 50 Crystalline Blackrocks I required. Your help is greatly appreciated. " +
                       "Please accept these rewards as a token of my gratitude. May the crystals guide your path!"; } }

        public CrystallineBlackrockCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(CrystallineBlackrock), "Crystalline Blackrock", 50, 0x5732)); // Assuming Crystalline Blackrock item ID is 0x1F2
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(SerenitysHelm), 1, "Crystal Sage's Garb")); // Assuming Crystal Sage's Garb is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Crystalline Conundrum quest!");
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

    public class CrystalSageArion : MondainQuester
    {
        [Constructable]
        public CrystalSageArion()
            : base("The Crystal Sage", "Arion")
        {
        }

        public CrystalSageArion(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203F; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Arion's Crystal Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new WizardsHat { Hue = Utility.Random(1, 3000), Name = "Arion's Crystal Hat" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Arion's Crystal Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(CrystallineBlackrockCollectorQuest)
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
