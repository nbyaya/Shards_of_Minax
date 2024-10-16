using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class HideCollectorQuest2 : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Hide Master's Request"; } }

        public override object Description
        {
            get
            {
                return "Greetings, bold adventurer! I am Thorne, the Hide Master. Long ago, my forefathers crafted a legendary suit " +
                       "of armor from hides that were imbued with mystical properties. I need your assistance to gather 500 hides " +
                       "to help me recreate this ancient armor. Your efforts will be richly rewarded with gold, a rare Maxxia Scroll, " +
                       "and the prestigious Hide Master's Garb, a truly remarkable piece of attire that will mark you as a true " +
                       "protector of the realm.";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, return to me with the hides."; } }

        public override object Uncomplete { get { return "I still need 500 hides. Please bring them to me so I can complete the armor."; } }

        public override object Complete { get { return "Excellent! You have gathered the 500 hides I needed. Your dedication to the cause is commendable. " +
                       "Please accept these rewards as a token of my appreciation. May the protection of your endeavors be as strong as " +
                       "the armor you helped create!"; } }

        public HideCollectorQuest2() : base()
        {
            AddObjective(new ObtainObjective(typeof(Hides), "Hides", 500, 0x1079)); // Assuming Hide item ID is 0x1C4
            AddReward(new BaseReward(typeof(Gold), 10000, "10000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(PickpocketsNimbleGloves), 1, "Hide Master's Garb")); // Assuming Hide Master's Garb is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Hide Master's Request quest!");
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

    public class HideMasterThorne : MondainQuester
    {
        [Constructable]
        public HideMasterThorne()
            : base("The Hide Master", "Thorne")
        {
        }

        public HideMasterThorne(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x204C; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new LeatherChest { Hue = Utility.Random(1, 3000), Name = "Thorne's Hide Armor" });
            AddItem(new LeatherLegs { Hue = Utility.Random(1, 3000) });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000) });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new BoneHelm { Hue = Utility.Random(1, 3000), Name = "Thorne's Bone Helm" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Thorne's Hide Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(HideCollectorQuest2)
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

    // Assuming Hide Master's Garb is a custom item, you need to define it.
    public class HideMasterGarb : BaseClothing
    {
        [Constructable]
        public HideMasterGarb() : base(0x1C0) // Item ID placeholder
        {
            Hue = Utility.Random(1, 3000);
            Name = "Hide Master's Garb";
        }

        public HideMasterGarb(Serial serial) : base(serial)
        {
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
