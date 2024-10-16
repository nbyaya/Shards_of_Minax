using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class IronCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Blacksmith's Request"; } }

        public override object Description
        {
            get
            {
                return "Hello, traveler! I'm in need of iron ore for my blacksmithing. " +
                       "Would you be interested in helping me collect some iron ore? " +
                       "Bring me 50 iron ore, and I will reward you handsomely!";
            }
        }

        public override object Refuse { get { return "Very well. Come back if you change your mind."; } }

        public override object Uncomplete { get { return "You haven't collected enough iron ore yet. Keep at it!"; } }

        public override object Complete { get { return "Thank you! Here is your reward for helping with the iron ore."; } }

        public IronCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(IronOre), "Iron Ore", 50, 0x19B9));
            AddReward(new BaseReward(typeof(Gold), 3500, "3500 Gold"));
            AddReward(new BaseReward(typeof(IronIngot), 10, "10 Iron Ingots"));
            AddReward(new BaseReward(typeof(BakersSoftShoes), 1, "Shoes of Quality Iron"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You have completed the Blacksmith's Request!");
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

    public class IronCollectorIan : MondainQuester
    {

        [Constructable]
        public IronCollectorIan()
            : base("Iron Collector Ian", "The Blacksmith")
        {
        }

        public IronCollectorIan(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(85, 85, 25);

            Body = 0x190; // Male Body

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x2049; // Random hair style
            HairHue = 1149; // Hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new RingmailChest { Hue = 2406 });
            AddItem(new RingmailLegs { Hue = 2406 });
            AddItem(new Boots { Hue = 2301 });
            AddItem(new HammerPick { Name = "Ian's Iron Collector Hammer", Hue = 0 });
            Backpack backpack = new Backpack();
            backpack.Hue = 2406;
            backpack.Name = "Bag of Quality Iron";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(IronCollectorQuest)
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
