using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class AwaseMisoCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Awase Miso Collectors"; } }

        public override object Description
        {
            get
            {
                return "Ah, traveler, you have come at a fortuitous time! I am Tsukiko, the Keeper of Flavors. " +
                       "Our village is suffering from a dire shortage of AwaseMisoSoup, a dish of immense cultural and spiritual significance. " +
                       "I need your assistance to gather 50 bowls of this sacred soup. " +
                       "In exchange for your help, I will grant you gold, a rare Maxxia Scroll, and a unique Buckler befitting the Keeper of Flavors.";
            }
        }

        public override object Refuse { get { return "I understand. If you change your mind, you know where to find me."; } }

        public override object Uncomplete { get { return "I am still in need of 50 bowls of AwaseMisoSoup. Please help us with this vital task!"; } }

        public override object Complete { get { return "Marvelous! You have brought me the 50 bowls of AwaseMisoSoup. Your generosity will not go unnoticed. " +
                       "Please accept these rewards as a token of our gratitude. May your journeys be filled with flavors as rich as your heart!"; } }

        public AwaseMisoCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(AwaseMisoSoup), "AwaseMisoSoup", 50, 0x2850)); // Assuming AwaseMisoSoup item ID is 0x1F0
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(DefendersEnchantedBuckler), 1, "Keeper of Flavors' Buckler")); // Assuming Keeper of Flavors' Garb is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Awase Miso Collector quest!");
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

    public class KeeperOfFlavorsTsukiko : MondainQuester
    {
        [Constructable]
        public KeeperOfFlavorsTsukiko()
            : base("The Keeper of Flavors", "Tsukiko")
        {
        }

        public KeeperOfFlavorsTsukiko(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Female Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x203C; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new FemaleLeatherChest { Hue = Utility.Random(1, 3000), Name = "Tsukiko's Traditional Kimono" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new StrawHat { Hue = Utility.Random(1, 3000), Name = "Tsukiko's Culinary Hat" });
            AddItem(new PlateGloves { Hue = Utility.Random(1, 3000), Name = "Tsukiko's Delicate Gloves" });
            AddItem(new Kilt { Hue = Utility.Random(1, 3000) });
            AddItem(new Cloak { Hue = Utility.Random(1, 3000), Name = "Tsukiko's Enchanted Cloak" }); // Assuming this is a custom item
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Tsukiko's Culinary Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(AwaseMisoCollectorQuest)
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
