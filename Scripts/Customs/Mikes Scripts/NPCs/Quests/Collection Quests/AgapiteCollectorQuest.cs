using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class AgapiteCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Agapite Collector's Request"; } }

        public override object Description
        {
            get
            {
                return "Greetings, adventurer! I am in need of Agapite ingots for my research. " +
                       "Could you help me collect 50 Agapite ingots? " +
                       "I will reward you generously for your efforts.";
            }
        }

        public override object Refuse { get { return "Very well. Return if you change your mind."; } }

        public override object Uncomplete { get { return "You haven't collected enough Agapite ingots yet. Keep going!"; } }

        public override object Complete { get { return "Thank you! Here is your reward for helping with the Agapite collection."; } }

        public AgapiteCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(AgapiteIngot), "Agapite Ingots", 50, 0x1BF2));
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
			AddReward(new BaseReward(typeof(GauntletsOfTheMasterArtisan), 1, "Agapite Gauntlets")); // Assuming Agapite Armor is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You have completed the Agapite Collector's Request!");
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

    public class AgapiteCollectorNPC : MondainQuester
    {

        [Constructable]
        public AgapiteCollectorNPC()
            : base("The Ingots Master", "Agapite Collector")
        {
        }

        public AgapiteCollectorNPC(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Female = false;
            Body = 0x190; // Male Body

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x2048; // Random hair style
            HairHue = 1153; // Hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new PlateChest { Hue = 1150 });
            AddItem(new PlateLegs { Hue = 1150 });
            AddItem(new PlateArms { Hue = 1150 });
            AddItem(new PlateGloves { Hue = 1150 });
            AddItem(new PlateGorget { Hue = 1150 });
            AddItem(new PlateHelm { Hue = 1150 });
            AddItem(new Cloak(1157));
            AddItem(new Sandals(1153));
            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Bag O' Agapite";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(AgapiteCollectorQuest)
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
