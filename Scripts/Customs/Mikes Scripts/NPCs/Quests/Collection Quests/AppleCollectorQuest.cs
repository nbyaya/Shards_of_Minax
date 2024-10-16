using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class AppleCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Apple Collector's Request"; } }

        public override object Description
        {
            get
            {
                return "Hello, traveler! The orchard has yielded an amazing apple harvest this season. " +
                       "Would you be interested in helping me collect some apples? " +
                       "Bring me 50 apples, and I will reward you handsomely!";
            }
        }

        public override object Refuse { get { return "Very well. Come back if you change your mind."; } }

        public override object Uncomplete { get { return "You haven't collected enough apples yet. Keep at it!"; } }

        public override object Complete { get { return "Thank you! Here is your reward for helping with the apple harvest."; } }

        public AppleCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(Apple), "Apples", 50, 0x9D0));
            AddReward(new BaseReward(typeof(Gold), 3000, "3000 Gold"));
            AddReward(new BaseReward(typeof(ApplePie), 1, "Apple Pie"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
			AddReward(new BaseReward(typeof(SwingsDancersShoes), 1, "Anna's Apple Shoes")); // Assuming Chef Hat is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You have completed the Apple Collector's Request!");
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

    public class AppleCollectorAnna : MondainQuester
    {

        [Constructable]
        public AppleCollectorAnna()
            : base("The Orchardist", "Apple Collector Anna")
        {
        }

        public AppleCollectorAnna(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(80, 80, 25);

            Female = true;
            Body = 0x191; // Female Body

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x203C; // Random hair style
            HairHue = 44; // Hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new PlainDress(33));
            AddItem(new Bonnet(44));
            AddItem(new Boots(23));
            AddItem(new ShepherdsCrook { Name = "Anna's Apple Collector Stick", Hue = 0 });
            Backpack backpack = new Backpack();
            backpack.Hue = 44;
            backpack.Name = "Bag O' Fresh Apples";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(AppleCollectorQuest)
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
