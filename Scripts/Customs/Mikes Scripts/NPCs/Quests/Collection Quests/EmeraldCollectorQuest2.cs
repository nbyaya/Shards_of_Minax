using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class EmeraldCollectorQuest2 : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Emerald Enigma"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Seraphis, the Keeper of Ancient Jewels. Legends speak of an ancient artifact " +
                       "that can only be restored with 50 Perfect Emeralds. These gems are said to hold the essence of the earth itself, " +
                       "imbued with mystical energies that are crucial for the artifact's reawakening. If you can retrieve these emeralds for me, " +
                       "I will reward you with gold, a rare Maxxia Scroll, and the Emerald Keeper's Mantle, a garment of unparalleled beauty and power.";
            }
        }

        public override object Refuse { get { return "I understand. If you change your mind, return with the Perfect Emeralds."; } }

        public override object Uncomplete { get { return "I still require 50 Perfect Emeralds. They are crucial for restoring the ancient artifact!"; } }

        public override object Complete { get { return "Splendid work! You have gathered the 50 Perfect Emeralds I needed. Your dedication is truly commendable. " +
                       "Please accept these rewards as a token of my gratitude. May your adventures be filled with fortune and wonder!"; } }

        public EmeraldCollectorQuest2() : base()
        {
            AddObjective(new ObtainObjective(typeof(PerfectEmerald), "Perfect Emeralds", 50, 0xF10)); // Assuming Perfect Emerald item ID is 0x1F5
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(WitchesBrewedHat), 1, "Emerald Keeper's Mantle")); // Assuming Emerald Keeper's Mantle is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Emerald Enigma quest!");
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

    public class EmeraldKeeperSeraphis : MondainQuester
    {
        [Constructable]
        public EmeraldKeeperSeraphis()
            : base("The Emerald Keeper", "Seraphis")
        {
        }

        public EmeraldKeeperSeraphis(Serial serial)
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
            AddItem(new LongPants { Hue = Utility.Random(1, 3000), Name = "Seraphis's Enchanted Pants" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new JesterHat { Hue = Utility.Random(1, 3000), Name = "Seraphis's Mystical Hat" });
            AddItem(new GoldBracelet { Hue = Utility.Random(1, 3000), Name = "Seraphis's Jewel Bracelet" });
            AddItem(new FancyShirt { Hue = Utility.Random(1, 3000), Name = "Seraphis's Shimmering Shirt" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Seraphis's Mystic Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(EmeraldCollectorQuest2)
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
