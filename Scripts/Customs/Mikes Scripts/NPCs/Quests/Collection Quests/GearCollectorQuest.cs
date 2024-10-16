using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class GearCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Gear Collector"; } }

        public override object Description
        {
            get
            {
                return "Greetings, intrepid adventurer! I am Gearon, the Mechanist. I am in dire need of 50 Gears for my latest invention. " +
                       "These gears are crucial for the mechanism to function properly. In return for your assistance, I will reward you with " +
                       "gold, a rare Maxxia Scroll, and a unique Mechanist's Hood that will be a testament to your mechanical prowess.";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, return to me with the Gears."; } }

        public override object Uncomplete { get { return "I still need 50 Gears. Please bring them to me so I can complete my invention!"; } }

        public override object Complete { get { return "Fantastic! You've brought me the 50 Gears I needed. Your help is invaluable. " +
                       "Please accept these rewards as a token of my gratitude. May your adventures be ever prosperous!"; } }

        public GearCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(Gears), "Gears", 50, 0x1053)); // Assuming Gear item ID is 0x1F0
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(MastersThiefsHood), 1, "Mechanist's Hood")); // Assuming Mechanist's Vest is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Gear Collector quest!");
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

    public class MechanistGearon : MondainQuester
    {
        [Constructable]
        public MechanistGearon()
            : base("The Mechanist", "Gearon")
        {
        }

        public MechanistGearon(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x2046; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new PlateChest { Hue = Utility.Random(1, 3000), Name = "Gearon's Mechanic Chestplate" });
            AddItem(new PlateLegs { Hue = Utility.Random(1, 3000) });
            AddItem(new PlateArms { Hue = Utility.Random(1, 3000), Name = "Gearon's Bracers of Precision" });
            AddItem(new NorseHelm { Hue = Utility.Random(1, 3000), Name = "Gearon's Helm of the Mechanist" });
            AddItem(new Boots { Hue = Utility.Random(1, 3000) });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Gearon's Toolbox";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(GearCollectorQuest)
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
