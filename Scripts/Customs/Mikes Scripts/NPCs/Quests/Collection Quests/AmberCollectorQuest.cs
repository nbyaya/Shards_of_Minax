using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class AmberCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Amber Seeker's Challenge"; } }

        public override object Description
        {
            get
            {
                return "Greetings, valiant adventurer! I am Zephyr, a dedicated collector of rare and precious gems. " +
                       "At present, I am in need of 50 pieces of amber to complete my collection and unlock the secrets of the ancient stones. " +
                       "Will you assist me in this quest? In return, you will be rewarded with gold, a rare Maxxia Scroll, and a unique " +
                       "Amber Legs that I have crafted just for this occasion.";
            }
        }

        public override object Refuse { get { return "Very well. Should you reconsider, I shall be here, awaiting your aid."; } }

        public override object Uncomplete { get { return "I still require 50 pieces of amber to complete my collection. Please bring them to me as soon as you can!"; } }

        public override object Complete { get { return "Excellent! You have gathered all the amber I needed. My collection is now complete, and I am deeply grateful. " +
                       "Please accept these rewards as a token of my appreciation. Thank you for your help!"; } }

        public AmberCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(Amber), "Amber", 50, 0xF25)); // Assuming Amber item ID is 0x1C1
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(AerobicsInstructorsLegwarmers), 1, "Amber Legs")); // Assuming Amber Pendant is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Amber Seeker's Challenge quest!");
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

    public class AmberCollectorZephyr : MondainQuester
    {
        [Constructable]
        public AmberCollectorZephyr()
            : base("The Gemologist", "Amber Collector Zephyr")
        {
        }

        public AmberCollectorZephyr(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Body = 0x191; // Male Body

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x2040; // Wizard's hair style
            HairHue = 0x2C; // Hair hue (a rich brown)
        }

        public override void InitOutfit()
        {
            AddItem(new Robe(1355)); // Ornate robe
            AddItem(new Sandals(1355)); // Matching sandals
            AddItem(new FancyShirt(1355)); // Fancy shirt
            AddItem(new TallStrawHat { Name = "Zephyr's Hat", Hue = 1355 }); // Custom Straw Hat
            AddItem(new GnarledStaff { Name = "Zephyr's Gnarled Staff", Hue = 1355 }); // Custom Gnarled Staff
            Backpack backpack = new Backpack();
            backpack.Hue = 1355;
            backpack.Name = "Bag of Amber and Secrets";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(AmberCollectorQuest)
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

    public class AmberPendant : Item
    {
        [Constructable]
        public AmberPendant() : base(0x1F09)
        {
            Name = "Amber Pendant";
            Hue = 0x4D0; // Golden hue
        }

        public AmberPendant(Serial serial) : base(serial)
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
