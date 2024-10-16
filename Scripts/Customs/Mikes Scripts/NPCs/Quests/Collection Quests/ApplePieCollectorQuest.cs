using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class ApplePieCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Great Apple Pie Collection"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Sir Cedric, the Grand Confectioner. Long ago, I was cursed by a rival chef, " +
                       "and now I must gather 50 ApplePies to break this enchantment. The pies must be of the finest quality, made with love " +
                       "and care. In exchange for your invaluable help, I will reward you with gold, a rare Maxxia Scroll, and a special outfit " +
                       "fit for a hero. May your efforts be fruitful and your pies perfect!";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, return to me with the 50 ApplePies."; } }

        public override object Uncomplete { get { return "I still require 50 ApplePies. Bring them to me to help lift this curse!"; } }

        public override object Complete { get { return "Fantastic! You have gathered the 50 ApplePies I needed. You have saved me from my curse. " +
                       "Please accept these rewards as a token of my gratitude. May your future endeavors be as sweet as the pies you brought!"; } }

        public ApplePieCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(ApplePie), "ApplePies", 50, 0x1041)); // Assuming ApplePie item ID is 0x9B1
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(BreakdancersCap), 1, "Sir Cedric's Chef Hat")); // Assuming Chef Hat is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Great Apple Pie Collection quest!");
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

    public class SirCedric : MondainQuester
    {
        [Constructable]
        public SirCedric()
            : base("The Grand Confectioner", "Sir Cedric")
        {
        }

        public SirCedric(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203D; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt { Hue = Utility.Random(1, 3000), Name = "Sir Cedric's Chef Tunic" });
            AddItem(new LongPants { Hue = Utility.Random(1, 3000) });
            AddItem(new Cap { Hue = Utility.Random(1, 3000), Name = "Sir Cedric's Chef Hat" });
            AddItem(new Shoes { Hue = Utility.Random(1, 3000) });
            AddItem(new GoldBracelet { Hue = Utility.Random(1, 3000), Name = "Sir Cedric's Lucky Bracelet" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Sir Cedric's Culinary Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(ApplePieCollectorQuest)
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
