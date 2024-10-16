using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class HideCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Tanner's Request"; } }

        public override object Description
        {
            get
            {
                return "Hello, traveler! I'm in need of hides for my leatherwork. " +
                       "Would you be interested in helping me collect some hides? " +
                       "Bring me 50 hides, and I will reward you handsomely!";
            }
        }

        public override object Refuse { get { return "Very well. Come back if you change your mind."; } }

        public override object Uncomplete { get { return "You haven't collected enough hides yet. Keep at it!"; } }

        public override object Complete { get { return "Thank you! Here is your reward for helping with the hides."; } }

        public HideCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(Hides), "Hides", 50, 0x1079));
            AddReward(new BaseReward(typeof(Gold), 3500, "3500 Gold"));
            AddReward(new BaseReward(typeof(LeatherChest), 1, "Leather Chest"));
            AddReward(new BaseReward(typeof(BowyersInsightfulBandana), 1, "Bandana of Quality Hides"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You have completed the Tanner's Request!");
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

    public class HideCollectorHenry : MondainQuester
    {

        [Constructable]
        public HideCollectorHenry()
            : base("Hide Collector Henry", "The Tanner")
        {
        }

        public HideCollectorHenry(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(85, 85, 25);

            Body = 0x190; // Male Body

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x2048; // Random hair style
            HairHue = 1148; // Hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new LeatherChest { Hue = 1045 });
            AddItem(new LongPants { Hue = 1023 });
            AddItem(new ThighBoots { Hue = 1124 });
            AddItem(new SkinningKnife { Name = "Henry's Hide Collector Knife", Hue = 0 });
            Backpack backpack = new Backpack();
            backpack.Hue = 1045;
            backpack.Name = "Bag of Quality Hides";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(HideCollectorQuest)
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
