using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class DullCopperQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Dull Copper Ingots Request"; } }

        public override object Description
        {
            get
            {
                return "Greetings! I am in need of Dull Copper Ingots for my blacksmithing work. " +
                       "Could you help me by bringing me 50 Dull Copper Ingots? " +
                       "In return, I'll reward you generously!";
            }
        }

        public override object Refuse { get { return "No problem. Come back if you change your mind."; } }

        public override object Uncomplete { get { return "You haven't collected enough Dull Copper Ingots yet. Keep going!"; } }

        public override object Complete { get { return "Thank you for the Dull Copper Ingots! Here is your reward."; } }

        public DullCopperQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(DullCopperIngot), "Dull Copper Ingots", 50, 0x1BF2));
            AddReward(new BaseReward(typeof(Gold), 4000, "4000 Gold"));
            AddReward(new BaseReward(typeof(SmithyHammer), 1, "Smithy Hammer"));
            AddReward(new BaseReward(typeof(MapmakersInsightfulMuffler), 1, "Blacksmith's Muffler"));
			AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You have completed the Dull Copper Ingots Request!");
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

    public class BlacksmithBenjamin : MondainQuester
    {
        [Constructable]
        public BlacksmithBenjamin()
            : base("Blacksmith Benjamin", "The Master Blacksmith")
        {
        }

        public BlacksmithBenjamin(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(90, 90, 30);

            Body = 0x190; // Male Body

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x2048; // Random hair style
            HairHue = 1157; // Hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new SmithHammer { Hue = 0 });
            AddItem(new Server.Items.PlateChest { Hue = 1152 });
            AddItem(new Server.Items.PlateLegs { Hue = 1152 });
            AddItem(new Server.Items.PlateGloves { Hue = 1152 });
            AddItem(new Server.Items.PlateGorget { Hue = 1152 });
            Backpack backpack = new Backpack();
            backpack.Hue = 1152;
            backpack.Name = "Blacksmith's Pack";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(DullCopperQuest)
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
