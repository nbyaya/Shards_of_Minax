using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class MilkChocolateCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Sweetest Quest"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave wanderer! I am Lord Dulce, the Grand Confectioner of the enchanted realm. " +
                       "My kingdom's magical sweet reserves are dwindling, and only you can help restore them. " +
                       "I require 50 Milk Chocolates to create a grand feast for our annual celebration. " +
                       "In return, you shall be richly rewarded with gold, a rare Maxxia Scroll, and a dazzling outfit befitting a master of sweets.";
            }
        }

        public override object Refuse { get { return "Very well. Should you reconsider, return to me with the Milk Chocolates."; } }

        public override object Uncomplete { get { return "I still need 50 Milk Chocolates. Gather them quickly to assist in our grand feast preparations!"; } }

        public override object Complete { get { return "Splendid! You have gathered the 50 Milk Chocolates I required. Your help will ensure our celebration is a success. " +
                       "As a token of my appreciation, please accept these rewards. May your adventures be as sweet as the chocolates you brought!"; } }

        public MilkChocolateCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(MilkChocolate), "Milk Chocolates", 50, 0xF18)); // Assuming Milk Chocolate item ID is 0xF7D
            AddReward(new BaseReward(typeof(Gold), 6000, "6000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(SwordSaintsArmguards), 1, "Grand Confectioner's Outfit")); // Assuming Confectioner's Outfit is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Sweetest Quest!");
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

    public class GrandConfectionerLordDulce : MondainQuester
    {
        [Constructable]
        public GrandConfectionerLordDulce()
            : base("The Grand Confectioner", "Lord Dulce")
        {
        }

        public GrandConfectionerLordDulce(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(120, 120, 40);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203F; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt { Hue = Utility.Random(1, 3000), Name = "Lord Dulce's Grand Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new JesterHat { Hue = Utility.Random(1, 3000), Name = "Lord Dulce's Sweet Hat" });
            AddItem(new GoldBracelet { Hue = Utility.Random(1, 3000), Name = "Lord Dulce's Golden Bracelet" });
            AddItem(new ShortPants { Hue = Utility.Random(1, 3000), Name = "Lord Dulce's Fancy Trousers" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Lord Dulce's Sweet Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(MilkChocolateCollectorQuest)
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
