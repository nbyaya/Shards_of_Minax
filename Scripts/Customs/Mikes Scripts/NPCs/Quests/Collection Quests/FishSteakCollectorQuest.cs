using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class FishSteakCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Gourmet's Request"; } }

        public override object Description
        {
            get
            {
                return "Ah, greetings, brave adventurer! I am Gaston, the renowned gourmet of these lands. My culinary creations " +
                       "require the finest ingredients, and I am in desperate need of 50 Fish Steaks. They are integral to my " +
                       "legendary seafood dishes that have enchanted taste buds across the realm. Help me in this gastronomic quest, " +
                       "and I shall reward you with gold, a rare Maxxia Scroll, and a splendid outfit fit for any noble chef.";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind and wish to help with the Fish Steaks, return to me."; } }

        public override object Uncomplete { get { return "I still need 50 Fish Steaks. Please bring them to me so I can continue my culinary masterpieces!"; } }

        public override object Complete { get { return "Fantastic work! You have delivered the 50 Fish Steaks I needed. Your help is greatly appreciated. " +
                       "Here are your rewards: gold, a Maxxia Scroll, and a unique set of gourmet attire. May your travels be as delightful " +
                       "as the dishes you have helped create!"; } }

        public FishSteakCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(FishSteak), "Fish Steaks", 50, 0x97B)); // Assuming FishSteak item ID is 0x97F
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(CuratorsKilt), 1, "Gourmet's Attire")); // Assuming GourmetOutfit is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Gourmet's Request quest!");
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

    public class GourmetGaston : MondainQuester
    {
        [Constructable]
        public GourmetGaston()
            : base("The Renowned Gourmet", "Gaston")
        {
        }

        public GourmetGaston(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203B; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Cap { Hue = Utility.Random(1, 3000), Name = "Gaston’s Chef Hat" });
            AddItem(new FullApron { Hue = Utility.Random(1, 3000), Name = "Gaston’s Apron" });
            AddItem(new Shoes { Hue = Utility.Random(1, 3000) });
            AddItem(new Shirt { Hue = Utility.Random(1, 3000), Name = "Gaston’s Culinary Shirt" });
            AddItem(new Kilt { Hue = Utility.Random(1, 3000) });
            AddItem(new PlateGloves { Hue = Utility.Random(1, 3000), Name = "Gaston’s Cooking Gloves" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Gaston’s Ingredient Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(FishSteakCollectorQuest)
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
