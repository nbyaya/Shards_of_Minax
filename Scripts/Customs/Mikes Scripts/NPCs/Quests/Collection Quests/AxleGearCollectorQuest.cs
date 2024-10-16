using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class AxleGearCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Mechanic's Request"; } }

        public override object Description
        {
            get
            {
                return "Greetings, traveler! I am Garrick, a skilled mechanic with a knack for tinkering and inventing. " +
                       "At the moment, I am working on a grand project that requires a vast number of AxleGears. I need 50 of these " +
                       "gears to complete my masterpiece. If you can help me gather them, I will reward you with gold, a rare Maxxia Scroll, " +
                       "and a unique Mechanic's Outfit that will make you stand out as a true artisan.";
            }
        }

        public override object Refuse { get { return "I understand if you're not interested. Should you reconsider, feel free to return and assist me with my project."; } }

        public override object Uncomplete { get { return "I still require 50 AxleGears to finish my project. Please bring them to me as soon as you can!"; } }

        public override object Complete { get { return "Wonderful! You've brought me all the AxleGears I need. My project is now complete thanks to you. " +
                       "As a token of my appreciation, please accept these rewards. Thank you for your assistance!"; } }

        public AxleGearCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(AxleGears), "AxleGear", 50, 0x1051)); // Assuming AxleGear item ID is 0x1BF
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(StrawHat), 1, "Mechanic's Hat")); // Assuming Mechanics Hat is a custom item
            AddReward(new BaseReward(typeof(LeatherGloves), 1, "Mechanic's Gloves")); // Assuming Mechanics Gloves is a custom item
            AddReward(new BaseReward(typeof(LorekeepersSash), 1, "Mechanic's Sash")); // Assuming Mechanics Tunic is a custom item
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

    public class AxleGearCollectorGarrick : MondainQuester
    {
        [Constructable]
        public AxleGearCollectorGarrick()
            : base("The Mechanic", "Axle Gear Collector Garrick")
        {
        }

        public AxleGearCollectorGarrick(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Body = 0x190; // Male Body

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x203D; // Mechanic's hair style
            HairHue = 0x8A; // Hair hue (light brown)
        }

        public override void InitOutfit()
        {
            AddItem(new FullApron { Name = "Garrick's Apron", Hue = 1150 }); // Custom Apron
            AddItem(new LongPants(1150)); // Long pants
            AddItem(new Shoes(1150)); // Matching shoes
            AddItem(new BodySash(1150)); // Custom Sash
            AddItem(new StrawHat { Name = "Garrick's Mechanic Hat", Hue = 1150 }); // Custom Mechanic Hat
            AddItem(new LeatherGloves { Name = "Garrick's Mechanic Gloves", Hue = 1150 }); // Custom Mechanic Gloves
            AddItem(new Tunic { Name = "Garrick's Mechanic Tunic", Hue = 1150 }); // Custom Mechanic Tunic
            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Garrick's Tool Kit";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(AxleGearCollectorQuest)
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
