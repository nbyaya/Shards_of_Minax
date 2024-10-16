using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class DiseasedBarkCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Diseased Bark Dilemma"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Thalric, the Cursed Gardener. The once vibrant woods have fallen into decay, " +
                       "and I need your help to gather 50 Diseased Bark samples from the afflicted trees. These samples are vital for my research " +
                       "to reverse the curse that plagues the forest. In exchange for your assistance, I will reward you with gold, a rare Maxxia Scroll, " +
                       "and the enchanted Cursed Gardener's Tunic, which holds the essence of the ancient woods.";
            }
        }

        public override object Refuse { get { return "Very well. Should you change your mind, return to me with the Diseased Bark samples."; } }

        public override object Uncomplete { get { return "I still need 50 Diseased Bark samples. Please bring them to me so we can save the forest!"; } }

        public override object Complete { get { return "Wonderful! You have brought me the 50 Diseased Bark samples. Your contribution is invaluable. " +
                       "As a token of my gratitude, please accept these rewards. May the forest’s spirits bless your path!"; } }

        public DiseasedBarkCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(DiseasedBark), "Diseased Bark", 50, 0x318B)); // Assuming Diseased Bark item ID is 0x1E3
            AddReward(new BaseReward(typeof(Gold), 6000, "6000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(MelodiousMuffler), 1, "Cursed Gardener's Tunic")); // Assuming Cursed Gardener's Tunic is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Diseased Bark Dilemma quest!");
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

    public class CursedGardenerThalric : MondainQuester
    {
        [Constructable]
        public CursedGardenerThalric()
            : base("The Cursed Gardener", "Thalric")
        {
        }

        public CursedGardenerThalric(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(120, 100, 40);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x2041; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Doublet { Hue = Utility.Random(1, 3000), Name = "Thalric's Cursed Doublet" });
            AddItem(new ShortPants { Hue = Utility.Random(1, 3000) });
            AddItem(new Bandana { Hue = Utility.Random(1, 3000), Name = "Thalric's Enchanted Bandana" });
            AddItem(new Boots { Hue = Utility.Random(1, 3000) });
            AddItem(new Cloak { Hue = Utility.Random(1, 3000), Name = "Thalric's Forest Cloak" });
            AddItem(new QuarterStaff { Hue = Utility.Random(1, 3000), Name = "Thalric's Mystical Staff" }); // Assuming this is a custom item
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Thalric's Magical Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(DiseasedBarkCollectorQuest)
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
