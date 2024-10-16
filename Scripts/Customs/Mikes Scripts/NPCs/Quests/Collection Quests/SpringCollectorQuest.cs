using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class SpringCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Enigmatic Spring Collector"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Faelwyn, the Keeper of Springs. A mysterious force has disturbed the natural balance, " +
                       "and I require 50 Springs to restore it. These Springs hold a vital essence for my ancient rituals. In return for your invaluable " +
                       "aid, I will reward you with gold, a rare Maxxia Scroll, and an exquisite Spring Keeper's Garb that will mark you as a true hero of the realms.";
            }
        }

        public override object Refuse { get { return "Very well. Should you change your mind, return to me with the Springs."; } }

        public override object Uncomplete { get { return "I still require 50 Springs. Bring them to me so I can restore the balance."; } }

        public override object Complete { get { return "Marvelous! You have gathered the 50 Springs I needed. Your help is truly appreciated. " +
                       "Please accept these rewards as a symbol of my gratitude. May your adventures be as vibrant as the springs you have collected!"; } }

        public SpringCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(Springs), "Springs", 50, 0x105D)); // Assuming Spring item ID is 0x1F4
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(WarriorOfUlstersTunic), 1, "Spring Keeper's Garb")); // Assuming Spring Keeper's Garb is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Spring Collector quest!");
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

    public class SpringKeeperFaelwyn : MondainQuester
    {
        [Constructable]
        public SpringKeeperFaelwyn()
            : base("The Keeper of Springs", "Faelwyn")
        {
        }

        public SpringKeeperFaelwyn(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Female Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x204C; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Sandals { Hue = Utility.Random(1, 3000), Name = "Faelwyn's Enchanted Sandals" });
            AddItem(new FemalePlateChest { Hue = Utility.Random(1, 3000), Name = "Faelwyn's Spring Robe" });
            AddItem(new FeatheredHat { Hue = Utility.Random(1, 3000), Name = "Faelwyn's Spring Hat" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000), Name = "Faelwyn's Mystical Gloves" });
            AddItem(new Kilt { Hue = Utility.Random(1, 3000), Name = "Faelwyn's Spring Kilt" });
            AddItem(new GoldRing { Hue = Utility.Random(1, 3000), Name = "Faelwyn's Mystical Ring" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Faelwyn's Magical Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(SpringCollectorQuest)
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
