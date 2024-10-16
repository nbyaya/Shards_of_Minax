using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class ArrowFetcherQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Arrow Fetcher's Request"; } }

        public override object Description
        {
            get
            {
                return "Greetings, adventurer! I am Elenor, the famed Arrow Fetcher. My archery collection is missing a critical " +
                       "component—arrows! I need 500 arrows to replenish my stock and continue my work. Can you assist me in this task? " +
                       "In return, I will reward you with gold, a rare Maxxia Scroll, and a unique Arrowhead Gauntlet that signifies " +
                       "your contribution.";
            }
        }

        public override object Refuse { get { return "Very well, if you change your mind, I'll be here. My arrows await!"; } }

        public override object Uncomplete { get { return "I still need 50 arrows to complete my collection. Please bring them to me as soon as possible!"; } }

        public override object Complete { get { return "Excellent! You've gathered all the arrows I needed. My collection is now complete thanks to you. " +
                       "Please accept these rewards as a token of my appreciation. Thank you for your help!"; } }

        public ArrowFetcherQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(Arrow), "Arrow", 500, 0xF3F)); // Assuming Arrow item ID is 0x1B2
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(GauntletsOfSecrecy), 1, "Arrowhead Gauntlet")); // Assuming Arrowhead Pendant is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Arrow Fetcher's Request quest!");
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

    public class ArrowFetcherElenor : MondainQuester
    {
        [Constructable]
        public ArrowFetcherElenor()
            : base("The Arrow Fetcher", "Elenor")
        {
        }

        public ArrowFetcherElenor(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Body = 0x191; // Female Body

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x203C; // Stylish hair style
            HairHue = 1154; // Hair hue (light brown)
        }

        public override void InitOutfit()
        {
            AddItem(new Robe(1161)); // Elegant robe
            AddItem(new Sandals(1161)); // Matching sandals
            AddItem(new LongPants(1161)); // Long pants
            AddItem(new Cloak(1161)); // Custom cloak
            AddItem(new Bow { Name = "Elenor's Longbow", Hue = 1161 }); // Custom Longbow
            AddItem(new Arrow(0x1B2)); // Arrows

            Backpack backpack = new Backpack();
            backpack.Hue = 1161;
            backpack.Name = "Bag of Arrows";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(ArrowFetcherQuest)
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

    public class ArrowheadPendant : Item
    {
        [Constructable]
        public ArrowheadPendant() : base(0x2D0B)
        {
            Name = "Arrowhead Pendant";
            Hue = 1150;
        }

        public ArrowheadPendant(Serial serial) : base(serial)
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
