using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class SapphireCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Sapphire Seeker"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave adventurer! I am Seraphina, the Sapphire Enchantress. The shimmering sapphires I seek hold great mystical power " +
                       "and are essential for a grand ritual to restore the balance of magic in our realm. Please bring me 50 Sapphires, and as a reward, " +
                       "I will grant you a generous sum of gold, a rare Maxxia Scroll, and a wondrous Sapphire Enchantress's Attire, imbued with enchantments.";
            }
        }

        public override object Refuse { get { return "I understand. Should you change your mind, return to me with the Sapphires."; } }

        public override object Uncomplete { get { return "I am still in need of 50 Sapphires. Return to me when you have collected them to aid in my ritual."; } }

        public override object Complete { get { return "Marvelous! You have gathered the 50 Sapphires I required. Your dedication is truly appreciated. " +
                       "Please accept these rewards as a token of my gratitude. May your path be ever radiant and prosperous!"; } }

        public SapphireCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(Sapphire), "Sapphires", 50, 0xF19)); // Assuming Sapphire item ID is 0x0F2
            AddReward(new BaseReward(typeof(Gold), 7000, "7000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(TamersMuffler), 1, "Sapphire Enchantress's Attire")); // Custom item for the reward
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Sapphire Seeker quest!");
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

    public class SapphireEnchantressSeraphina : MondainQuester
    {
        [Constructable]
        public SapphireEnchantressSeraphina()
            : base("The Sapphire Enchantress", "Seraphina")
        {
        }

        public SapphireEnchantressSeraphina(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x191; // Female Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x2041; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new FemaleLeatherChest { Hue = Utility.Random(1, 3000), Name = "Seraphina's Sapphire Chest" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new FeatheredHat { Hue = Utility.Random(1, 3000), Name = "Seraphina's Enchanted Hat" });
            AddItem(new GoldRing { Hue = Utility.Random(1, 3000), Name = "Seraphina's Mystical Ring" });
            AddItem(new LeatherArms { Hue = Utility.Random(1, 3000), Name = "Seraphina's Enchanted Bracers" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Seraphina's Mystical Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(SapphireCollectorQuest)
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

    public class SapphireEnchantressAttire : Item
    {
        [Constructable]
        public SapphireEnchantressAttire() : base(0x1F03) // Use an existing item ID or create a new one for custom attire
        {
            Hue = Utility.Random(1, 3000);
            Name = "Sapphire Enchantress's Attire";
            // Additional properties or functionality can be added here
        }

        public SapphireEnchantressAttire(Serial serial) : base(serial)
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
