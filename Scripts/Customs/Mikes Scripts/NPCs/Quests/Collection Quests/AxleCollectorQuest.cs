using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class AxleCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Mechanic's Request"; } }

        public override object Description
        {
            get
            {
                return "Greetings, traveler! I am Gearlock, the master mechanic of this realm. I am in desperate need of 50 Axles " +
                       "to complete my grand contraption. These Axles are crucial for the machine's functionality. " +
                       "If you can bring me these Axles, I will reward you generously with gold, a rare Maxxia Scroll, and a " +
                       "unique Mechanic's Helm. Will you aid me in this mechanical endeavor?";
            }
        }

        public override object Refuse { get { return "I understand. If you change your mind, please come back. My contraption awaits its Axles!"; } }

        public override object Uncomplete { get { return "I still need 50 Axles to finish my machine. Please gather them and return to me!"; } }

        public override object Complete { get { return "Excellent work! You've collected all 50 Axles I need. My contraption will be operational thanks to your help. " +
                       "Here are your rewards as a token of my appreciation. Thank you for your assistance!"; } }

        public AxleCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(Axle), "Axles", 50, 0x105B)); // Assuming Axle item ID is 0x1E5
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(KenseisResolveGreaves), 1, "Mechanic's Greaves")); // Assuming Mechanics Helm is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Mechanic's Request quest!");
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

    public class AxleCollectorGearlock : MondainQuester
    {
        [Constructable]
        public AxleCollectorGearlock()
            : base("The Mechanic", "Gearlock")
        {
        }

        public AxleCollectorGearlock(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(120, 120, 30);

            Body = 0x190; // Male Body

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x203B; // Mechanic's hair style
            HairHue = 1150; // Hair hue (grayish)
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt(1150)); // Fancy shirt
            AddItem(new ShortPants(1150)); // Short pants
            AddItem(new Boots(1150)); // Matching boots
            AddItem(new PlateChest { Name = "Gearlock's Mechanic Vest", Hue = 1150 }); // Custom Mechanic Vest
            AddItem(new PlateGloves { Name = "Gearlock's Mechanic Gloves", Hue = 1150 }); // Custom Mechanic Gloves
            AddItem(new PlateHelm { Name = "Gearlock's Mechanic Helm", Hue = 1150 }); // Custom Mechanic Helm
            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Bag of Tools and Parts";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(AxleCollectorQuest)
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
