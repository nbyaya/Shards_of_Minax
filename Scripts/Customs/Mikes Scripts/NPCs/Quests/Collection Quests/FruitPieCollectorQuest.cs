using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class FruitPieCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Great Fruit Pie Challenge"; } }

        public override object Description
        {
            get
            {
                return "Ah, a new face! I am Gormag the Gourmand, renowned for my insatiable appetite and discerning palate. " +
                       "I’ve recently developed an obsession with Fruit Pies, but alas, I require 50 of these delectable pastries " +
                       "to satisfy my craving. The pies are not just for my pleasure; they hold the key to a grand feast that will " +
                       "celebrate the renewal of the Harvest Moon. In exchange for your assistance, I shall reward you with gold, " +
                       "a rare Maxxia Scroll, and an exquisitely crafted Gourmand's Attire that will surely turn heads at any feast.";
            }
        }

        public override object Refuse { get { return "Very well, adventurer. If you change your mind, return with the Fruit Pies, and I shall reward you handsomely."; } }

        public override object Uncomplete { get { return "I still need 50 Fruit Pies. Please bring them to me so I can continue preparing for the grand feast!"; } }

        public override object Complete { get { return "Marvelous! You have brought me the 50 Fruit Pies I required. Your contribution to the Harvest Moon feast is invaluable. " +
                       "As a token of my gratitude, accept these rewards. May your travels be filled with as much delight as my feast will be!"; } }

        public FruitPieCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(FruitPie), "Fruit Pies", 50, 0x1041)); // Assuming Fruit Pie item ID is 0x1F2
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(RudosReinforcedGreaves), 1, "Gourmand's Attire")); // Assuming Gourmand's Attire is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Great Fruit Pie Challenge quest!");
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

    public class GormagTheGourmand : MondainQuester
    {
        [Constructable]
        public GormagTheGourmand()
            : base("The Gourmand", "Gormag")
        {
        }

        public GormagTheGourmand(Serial serial)
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
            AddItem(new Doublet { Hue = Utility.Random(1, 3000), Name = "Gormag's Fine Doublet" });
            AddItem(new Kilt { Hue = Utility.Random(1, 3000) });
            AddItem(new JesterHat { Hue = Utility.Random(1, 3000), Name = "Gormag's Festive Hat" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new FeatheredHat { Hue = Utility.Random(1, 3000), Name = "Gormag's Feathered Hat" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Gormag's Gourmet Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(FruitPieCollectorQuest)
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
