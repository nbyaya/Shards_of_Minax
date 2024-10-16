using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class BatWingCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Mysterious Collector's Request"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave soul! I am Zilara, a collector of the arcane and the bizarre. I have a peculiar " +
                       "interest in the wings of bats. They possess unique properties that I find quite fascinating. I need " +
                       "50 BatWings to complete my latest experiment. Will you assist me in gathering them? In return, I will " +
                       "reward you with a handsome sum of gold, a rare Maxxia Scroll, and a unique and enchanting Batwing Cloak.";
            }
        }

        public override object Refuse { get { return "Very well, adventurer. If you change your mind, come back to me. The BatWings are waiting!"; } }

        public override object Uncomplete { get { return "I still need 50 BatWings for my experiment. Please bring them to me!"; } }

        public override object Complete { get { return "Excellent! You've gathered all the BatWings I need. My experiment will be a success thanks to you. " +
                       "Please accept these rewards as a token of my gratitude. Thank you for your help!"; } }

        public BatWingCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(BatWing), "BatWing", 50, 0xF78)); // Assuming BatWing item ID is 0x1D0
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(TailorsFancyApron), 1, "Batwing Apron")); // Assuming Batwing Cloak is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Mysterious Collector's Request quest!");
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

    public class BatWingCollectorZilara : MondainQuester
    {
        [Constructable]
        public BatWingCollectorZilara()
            : base("The Mysterious Collector", "Zilara")
        {
        }

        public BatWingCollectorZilara(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Body = 0x191; // Female Body

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x203D; // Unique hair style
            HairHue = 1152; // Hair hue (dark blue)
        }

        public override void InitOutfit()
        {
            AddItem(new Robe(1153)); // Enchanted robe
            AddItem(new Sandals(1153)); // Matching sandals
            AddItem(new Cloak { Name = "Zilara's Batwing Cloak", Hue = 1153 }); // Custom Batwing Cloak
            AddItem(new GnarledStaff { Name = "Zilara's Arcane Staff", Hue = 1153 }); // Custom Arcane Staff
            Backpack backpack = new Backpack();
            backpack.Hue = 1153;
            backpack.Name = "Bag of Batwing Essence";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(BatWingCollectorQuest)
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
