using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class CopperCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Copper Collector's Request"; } }

        public override object Description
        {
            get
            {
                return "Greetings, adventurer! I am in need of a large quantity of copper ingots for my latest project. " +
                       "Could you help me by collecting 50 copper ingots? Bring them to me and you will be generously rewarded!";
            }
        }

        public override object Refuse { get { return "I see. Come back if you change your mind."; } }

        public override object Uncomplete { get { return "You haven't gathered enough copper ingots yet. Keep going!"; } }

        public override object Complete { get { return "Fantastic! Thank you for your effort. Here is your reward as promised."; } }

        public CopperCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(CopperIngot), "Copper Ingots", 50, 0x1BF2));
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
			AddReward(new BaseReward(typeof(SmithsPrecisionGauntlets), 1, "Zeeper's Vestments"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You have completed the Copper Collector's Request!");
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

    public class CopperCollectorJoe : MondainQuester
    {
        [Constructable]
        public CopperCollectorJoe()
            : base("The Metallurgist", "Copper Collector Joe")
        {
        }

        public CopperCollectorJoe(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x2048; // Random hair style
            HairHue = 1153; // Hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new PlateChest { Hue = 2413 });
            AddItem(new PlateLegs { Hue = 2413 });
            AddItem(new PlateArms { Hue = 2413 });
            AddItem(new PlateGorget { Hue = 2413 });
            AddItem(new PlateGloves { Hue = 2413 });
            AddItem(new PlateHelm { Hue = 2413 });
            AddItem(new Boots { Hue = 1175 });
            AddItem(new SmithHammer { Name = "Joe's Copper Hammer", Hue = 2413 });
            Backpack backpack = new Backpack();
            backpack.Hue = 1175;
            backpack.Name = "Joe's Metalworking Kit";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(CopperCollectorQuest)
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
