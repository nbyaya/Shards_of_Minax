using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class RotwormStewQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Rotworm Stew Connoisseur"; } }

        public override object Description
        {
            get
            {
                return "Greetings, daring adventurer! I am Gorrum, the Rotworm Stew Connoisseur. My culinary expertise " +
                       "requires a rare and peculiar ingredient – the BowlOfRotwormStew. I need you to bring me 50 of these " +
                       "steaming bowls to complete my collection. In exchange for your dedication, I will reward you with " +
                       "a handsome sum of gold, a rare Maxxia Scroll, and an exquisitely designed Chef's Attire that is " +
                       "said to be enchanted with the magic of the great Rotworm Feast.";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, return to me with the BowlOfRotwormStew."; } }

        public override object Uncomplete { get { return "I still require 50 BowlOfRotwormStew. Please bring them to me so I can continue my culinary pursuits!"; } }

        public override object Complete { get { return "Fantastic! You have brought me the 50 BowlOfRotwormStew I needed. Your contribution to my culinary " +
                       "collection is invaluable. Please accept these rewards as a token of my appreciation. May your travels be " +
                       "filled with delicious adventures!"; } }

        public RotwormStewQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(BowlOfRotwormStew), "BowlOfRotwormStew", 50, 0x2DBA)); // Assuming BowlOfRotwormStew item ID is 0xE1C
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(ReindeerFurCap), 1, "Chef's Attire")); // Assuming Chef's Attire is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Rotworm Stew Connoisseur quest!");
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

    public class Gorrum : MondainQuester
    {
        [Constructable]
        public Gorrum()
            : base("The Rotworm Stew Connoisseur", "Gorrum")
        {
        }

        public Gorrum(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x2044; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt { Hue = Utility.Random(1, 3000), Name = "Gorrum's Gourmet Shirt" });
            AddItem(new LongPants { Hue = Utility.Random(1, 3000) });
            AddItem(new Boots { Hue = Utility.Random(1, 3000) });
            AddItem(new Bandana { Hue = Utility.Random(1, 3000), Name = "Gorrum's Culinary Bandana" });
            AddItem(new FullApron { Hue = Utility.Random(1, 3000), Name = "Gorrum's Chef Apron" });
            AddItem(new GoldRing { Hue = Utility.Random(1, 3000), Name = "Gorrum's Chef Ring" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Gorrum's Recipe Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(RotwormStewQuest)
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
