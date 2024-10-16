using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class ClockFrameCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Timepiece Collector"; } }

        public override object Description
        {
            get
            {
                return "Greetings, adventurer! I am Horatio, a dedicated horologist with a passion for timepieces. " +
                       "I am in need of 50 ClockFrames for my collection. These frames are essential for my work, " +
                       "and I cannot complete my masterpiece without them. Will you help me gather these ClockFrames? " +
                       "In return, I will reward you with gold, a rare Maxxia Scroll, and a unique Timekeeper's Outfit that is both functional and extraordinary.";
            }
        }

        public override object Refuse { get { return "Very well. Should you change your mind, return to me. My collection awaits!"; } }

        public override object Uncomplete { get { return "I still need 50 ClockFrames. Please bring them to me when you have them!"; } }

        public override object Complete { get { return "Wonderful! You've brought me all the ClockFrames I needed. My masterpiece is now complete. " +
                       "Please accept these rewards as a token of my appreciation. Thank you for your assistance!"; } }

        public ClockFrameCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(ClockFrame), "ClockFrame", 50, 0x104D)); // Assuming ClockFrame item ID is 0x1BF
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(MoltenCloak), 1, "Timekeeper's Cloak")); // Assuming Timekeeper's Hat is a custom item
            AddReward(new BaseReward(typeof(ScholarsRobe), 1, "Timekeeper's Robe")); // Assuming Timekeeper's Robe is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Timepiece Collector quest!");
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

    public class ClockFrameCollectorHoratio : MondainQuester
    {
        [Constructable]
        public ClockFrameCollectorHoratio()
            : base("The Horologist", "Horatio the Horologist")
        {
        }

        public ClockFrameCollectorHoratio(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Body = 0x190; // Male Body

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x203C; // Long hair style
            HairHue = 1150; // Hair hue (gray)
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt(1150)); // Fancy shirt
            AddItem(new LongPants(1150)); // Long pants
            AddItem(new Shoes(1150)); // Matching shoes
            AddItem(new Robe(1150) { Name = "Horatio's Timekeeper's Robe" }); // Custom Robe
            AddItem(new SkullCap(1150) { Name = "Horatio's Timekeeper's Hat" }); // Custom Hat
            AddItem(new GoldRing { Name = "Horatio's Timepiece Ring", Hue = 1150 }); // Custom Ring
            AddItem(new Clock(1150) { Name = "Horatio's Pocket Watch" }); // Custom Pocket Watch
            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Bag of Clock Frames";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(ClockFrameCollectorQuest)
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
