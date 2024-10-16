using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class CocoaPulpCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Cocoa Connoisseur's Request"; } }

        public override object Description
        {
            get
            {
                return "Ah, welcome, traveler! I am Ithara, the Cocoa Connoisseur. My studies into the magical properties of CocoaPulp are " +
                       "nearing a breakthrough, but I need your help. I require 50 pieces of CocoaPulp for my research. In return, you will " +
                       "be rewarded with a generous amount of gold, a rare Maxxia Scroll, and an exquisite Cocoa Connoisseur's Attire that " +
                       "will make you the talk of any adventuring party.";
            }
        }

        public override object Refuse { get { return "I understand. If you change your mind, bring the CocoaPulp to me, and I shall reward you well."; } }

        public override object Uncomplete { get { return "I still need 50 pieces of CocoaPulp. Please gather them so I can continue my research."; } }

        public override object Complete { get { return "Wonderful! You have collected the 50 CocoaPulp I needed. Your help is invaluable. As a token of my gratitude, " +
                       "please accept these rewards. May your adventures be as delightful as the cocoa I cherish!"; } }

        public CocoaPulpCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(CocoaPulp), "CocoaPulp", 50, 0xF7C)); // Assuming CocoaPulp item ID is 0x1F2
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(BellowsPoweredCoif), 1, "Cocoa Connoisseur's Attire")); // Assuming Cocoa Attire is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Cocoa Connoisseur's Request quest!");
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

    public class CocoaConnoisseurIthara : MondainQuester
    {
        [Constructable]
        public CocoaConnoisseurIthara()
            : base("The Cocoa Connoisseur", "Ithara")
        {
        }

        public CocoaConnoisseurIthara(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Female Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x2040; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Ithara's Cocoa Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new FeatheredHat { Hue = Utility.Random(1, 3000), Name = "Ithara's Cocoa Hat" });
            AddItem(new GoldBracelet { Hue = Utility.Random(1, 3000), Name = "Ithara's Cocoa Bracelet" });
            AddItem(new Cloak { Hue = Utility.Random(1, 3000), Name = "Ithara's Cocoa Pendant" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Ithara's Cocoa Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(CocoaPulpCollectorQuest)
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
