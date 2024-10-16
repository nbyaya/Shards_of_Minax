using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class WasabiCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Wasabi Connoisseur's Request"; } }

        public override object Description
        {
            get
            {
                return "Ah, greetings, brave soul! I am Torvin, the Wasabi Connoisseur. I need your assistance in gathering " +
                       "50 Wasabi. This fiery root is vital for my culinary experiments. As a token of my appreciation, " +
                       "I will reward you with gold, a rare Maxxia Scroll, and a specially crafted Wasabi Connoisseur's Outfit.";
            }
        }

        public override object Refuse { get { return "Very well. If you change your mind, return to me with the Wasabi."; } }

        public override object Uncomplete { get { return "I still need 50 Wasabi. Please bring them to me so I can continue my experiments!"; } }

        public override object Complete { get { return "Splendid! You have gathered the 50 Wasabi I needed. Your efforts are truly appreciated. " +
                       "Please accept these rewards as a mark of my gratitude. May your adventures be as spicy as the Wasabi!"; } }

        public WasabiCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(Wasabi), "Wasabi", 50, 0x24E8)); // Assuming Wasabi item ID is 0x1F4
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(ModStyleTunic), 1, "Wasabi Connoisseur's Outfit")); // Assuming Wasabi Connoisseur's Outfit is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Wasabi Collector quest!");
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

    public class WasabiConnoisseurTorvin : MondainQuester
    {
        [Constructable]
        public WasabiConnoisseurTorvin()
            : base("The Wasabi Connoisseur", "Torvin")
        {
        }

        public WasabiConnoisseurTorvin(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203C; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt { Hue = Utility.Random(1, 3000), Name = "Torvin's Spicy Shirt" });
            AddItem(new ShortPants { Hue = Utility.Random(1, 3000) });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new FeatheredHat { Hue = Utility.Random(1, 3000), Name = "Torvin's Unique Hat" });
            AddItem(new LeatherGloves { Hue = Utility.Random(1, 3000) });
            AddItem(new Backpack { Hue = Utility.Random(1, 3000), Name = "Torvin's Culinary Satchel" });

            // Additional customization
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Torvin's Robe of Taste" });
            AddItem(new GoldRing { Hue = Utility.Random(1, 3000), Name = "Torvin's Lucky Ring" });
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(WasabiCollectorQuest)
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
