using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class MeatPieCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Great Meat Pie Collection"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Grumble the Pie Maker, renowned throughout the lands for my delectable " +
                       "meat pies. But alas, my stock has dwindled, and I require 50 Meat Pies to feed the hungry masses of my village. " +
                       "Your assistance in this gastronomic quest will not go unrewarded. In return, I shall bestow upon you a hefty sum of gold, " +
                       "a rare Maxxia Scroll, and a truly unique Pie Maker's Cap, enchanted with the essence of my culinary mastery.";
            }
        }

        public override object Refuse { get { return "Very well. Should you change your mind, return with the Meat Pies I seek."; } }

        public override object Uncomplete { get { return "I am still in need of 50 Meat Pies. Please bring them to me so that I can continue my work!"; } }

        public override object Complete { get { return "Splendid! You have delivered the 50 Meat Pies I needed. You have my deepest thanks and gratitude. " +
                       "Accept these rewards as a token of my appreciation, and may your travels be as satisfying as a freshly baked pie!"; } }

        public MeatPieCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(MeatPie), "Meat Pies", 50, 0x1041)); // Assuming Meat Pie item ID is 0x9D1
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(DeathwhispersCap), 1, "Pie Maker's Cap")); // Assuming Pie Maker's Apron is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Great Meat Pie Collection quest!");
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

    public class PieMakerGrumble : MondainQuester
    {
        [Constructable]
        public PieMakerGrumble()
            : base("The Pie Maker", "Grumble")
        {
        }

        public PieMakerGrumble(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203F; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new FullApron { Hue = Utility.Random(1, 3000), Name = "Grumble's Pie Maker Apron" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new Bandana { Hue = Utility.Random(1, 3000), Name = "Grumble's Cooking Bandana" });
            AddItem(new Kilt { Hue = Utility.Random(1, 3000) });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000), Name = "Grumble's Baking Gloves" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Grumble's Culinary Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(MeatPieCollectorQuest)
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
