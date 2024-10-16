using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class SweetCocoaButterQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Cocoa Connoisseur's Challenge"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Cadenza, the Cocoa Connoisseur. I require your assistance in gathering 50 Sweet Cocoa Butters. " +
                       "These special butters are essential for a grand feast I am preparing to celebrate the rare celestial alignment. " +
                       "In return for your help, I will reward you with gold, a rare Maxxia Scroll, and a uniquely enchanted Cocoa Connoisseur's Outfit. " +
                       "Join me in this grand celebration and earn your place among the stars!";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, return to me with the Sweet Cocoa Butters."; } }

        public override object Uncomplete { get { return "I still need 50 Sweet Cocoa Butters. Please bring them to me so we can prepare for the feast!"; } }

        public override object Complete { get { return "Fantastic! You have brought me the 50 Sweet Cocoa Butters I needed. Your contribution will make this feast legendary. " +
                       "Please accept these rewards as a token of my gratitude. May your path be as sweet as the butters you have collected!"; } }

        public SweetCocoaButterQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(SweetCocoaButter), "Sweet Cocoa Butters", 50, 0x103D)); // Assuming Sweet Cocoa Butter item ID is 0x1F3
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(TailorsEmbrace), 1, "Cocoa Connoisseur's Outfit")); // Assuming Cocoa Connoisseur's Outfit is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Cocoa Connoisseur's Challenge!");
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

    public class CocoaConnoisseurCadenza : MondainQuester
    {
        [Constructable]
        public CocoaConnoisseurCadenza()
            : base("The Cocoa Connoisseur", "Cadenza")
        {
        }

        public CocoaConnoisseurCadenza(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x191; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203E; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt { Hue = Utility.Random(1, 3000), Name = "Cadenza's Cocoa Tunic" });
            AddItem(new Kilt { Hue = Utility.Random(1, 3000), Name = "Cadenza's Festive Kilt" });
            AddItem(new JesterHat { Hue = Utility.Random(1, 3000), Name = "Cadenza's Cocoa Hat" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new GoldBracelet { Hue = Utility.Random(1, 3000), Name = "Cadenza's Golden Bracelet" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Cadenza's Cocoa Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(SweetCocoaButterQuest)
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
