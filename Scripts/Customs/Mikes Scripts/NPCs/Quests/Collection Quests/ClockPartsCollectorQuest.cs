using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class ClockPartsCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Clockwork Conundrum"; } }

        public override object Description
        {
            get
            {
                return "Ah, adventurer! I am Quirinius, the eccentric clockmaker. My workshop is in disarray, and I need " +
                       "a significant number of ClockParts to repair my finest creations. I require 50 ClockParts to get everything " +
                       "ticking again. If you can bring them to me, I will reward you handsomely with gold, a rare Maxxia Scroll, " +
                       "and a special Clockwork Top Hat that embodies my finest craftsmanship.";
            }
        }

        public override object Refuse { get { return "Oh, very well. If you change your mind, come back and help me get my clocks in order."; } }

        public override object Uncomplete { get { return "I still need 50 ClockParts to complete my repairs. Please bring them to me without delay!"; } }

        public override object Complete { get { return "Splendid! You've gathered all the ClockParts I need. My workshop is now in perfect order. " +
                       "Please accept these rewards as a token of my appreciation. Your help has been invaluable!"; } }

        public ClockPartsCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(ClockParts), "ClockParts", 50, 0x104F)); // Assuming ClockParts item ID is 0x1F5
            AddReward(new BaseReward(typeof(Gold), 6000, "6000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(LeprechaunsLuckyHat), 1, "Clockwork Top Hat")); // Assuming Clockwork Top Hat is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Clockwork Conundrum quest!");
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

    public class ClockPartsCollectorQuirinius : MondainQuester
    {
        [Constructable]
        public ClockPartsCollectorQuirinius()
            : base("The Clockmaker", "Clock Parts Collector Quirinius")
        {
        }

        public ClockPartsCollectorQuirinius(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Body = 0x190; // Male Body

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x204B; // Eccentric hair style
            HairHue = 1150; // Hair hue (silver-gray)
        }

        public override void InitOutfit()
        {
            AddItem(new Robe(1150)); // Robe with a unique hue
            AddItem(new Sandals(1150)); // Matching sandals
            AddItem(new TallStrawHat { Name = "Quirinius' Clockwork Top Hat", Hue = 1150 }); // Custom Clockwork Top Hat
            AddItem(new BlackStaff { Name = "Quirinius' Clockwork Staff", Hue = 1150 }); // Custom Clockwork Staff
            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Bag of Clock Parts";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(ClockPartsCollectorQuest)
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
