using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class BolaBallCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Gorath's Collection Challenge"; } }

        public override object Description
        {
            get
            {
                return "Greetings, adventurer! I am Gorath, a legendary collector of rare and curious items. " +
                       "I am on a quest to add a rare item to my collection: BolaBalls. I need 50 BolaBalls to complete my collection. " +
                       "If you can gather them for me, I will reward you handsomely with gold, a Maxxia Scroll, and a unique Collector's Buckler " +
                       "that is as extraordinary as my collection.";
            }
        }

        public override object Refuse { get { return "Very well, if you change your mind, return to me and prove your worth!"; } }

        public override object Uncomplete { get { return "I still require 50 BolaBalls for my collection. Please bring them to me soon!"; } }

        public override object Complete { get { return "Incredible! You've gathered all 50 BolaBalls for my collection. You have my deepest gratitude. " +
                       "Please accept these rewards as a token of my appreciation. Your efforts have not gone unnoticed!"; } }

        public BolaBallCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(BolaBall), "BolaBall", 50, 0xE73)); // Assuming BolaBall item ID is 0x1BF
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(MysticsGuardBuckler), 1, "Collector's Buckler")); // Assuming Collector's Vest is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Collection Challenge quest!");
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

    public class BolaBallCollectorGorath : MondainQuester
    {
        [Constructable]
        public BolaBallCollectorGorath()
            : base("The Collector", "Gorath the Collector")
        {
        }

        public BolaBallCollectorGorath(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Body = 0x190; // Male Body

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x203C; // Collector's hair style
            HairHue = 1152; // Hair hue (dark brown)
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt(1151)); // Fancy shirt
            AddItem(new LongPants(1151)); // Long pants
            AddItem(new Shoes(1151)); // Matching shoes
            AddItem(new Robe { Name = "Gorath's Collector Robe", Hue = 1151 }); // Custom Robe
            AddItem(new GoldBracelet { Name = "Gorath's Golden Bracelet", Hue = 1151 }); // Custom Bracelet
            AddItem(new QuarterStaff { Name = "Gorath's Collector's Staff", Hue = 1151 }); // Custom Staff
            Backpack backpack = new Backpack();
            backpack.Hue = 1151;
            backpack.Name = "Bag of Collectible Items";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(BolaBallCollectorQuest)
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
