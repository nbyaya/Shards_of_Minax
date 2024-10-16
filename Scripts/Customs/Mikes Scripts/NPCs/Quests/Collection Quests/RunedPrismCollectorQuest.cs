using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class RunedPrismCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Runed Prism Connoisseur"; } }

        public override object Description
        {
            get
            {
                return "Greetings, brave traveler! I am Elira, the Keeper of Mystical Prisms. " +
                       "The Runed Prisms are artifacts of great power, and I need your assistance in collecting 50 of them. " +
                       "These prisms are vital for my upcoming ritual to restore balance to the realms. " +
                       "In gratitude for your help, I will reward you with a generous sum of gold, a rare Maxxia Scroll, " +
                       "and a special Bandana imbued with the essence of the Runed Prisms.";
            }
        }

        public override object Refuse { get { return "I see. Should you change your mind, return with the Runed Prisms."; } }

        public override object Uncomplete { get { return "I am still in need of 50 Runed Prisms. Bring them to me to aid in the restoration ritual!"; } }

        public override object Complete { get { return "Fantastic! You have collected the 50 Runed Prisms I required. Your contribution will help immensely. " +
                       "Please accept these rewards as a token of my appreciation. May you always find success in your endeavors!"; } }

        public RunedPrismCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(RunedPrism), "Runed Prisms", 50, 0x2F57)); // Assuming RunedPrism item ID is 0x1F5
            AddReward(new BaseReward(typeof(Gold), 7000, "7000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
            AddReward(new BaseReward(typeof(HippiePeaceBandana), 1, "Runed Prism Bandana")); // Assuming Runed Prism Robe is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations! You have completed the Runed Prism Collector quest!");
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

    public class RunedPrismKeeperElira : MondainQuester
    {
        [Constructable]
        public RunedPrismKeeperElira()
            : base("The Keeper of Mystical Prisms", "Elira")
        {
        }

        public RunedPrismKeeperElira(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Female Body

            Hue = Utility.Random(1, 3000); // Unique hue for the NPC
            HairItemID = 0x2047; // Unique hair style
            HairHue = Utility.Random(1, 3000); // Unique hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Elira's Mystical Robe" });
            AddItem(new Sandals { Hue = Utility.Random(1, 3000) });
            AddItem(new FeatheredHat { Hue = Utility.Random(1, 3000), Name = "Elira's Enchanted Hat" });
            AddItem(new Robe { Hue = Utility.Random(1, 3000), Name = "Elira's Mystical Robe" });
            AddItem(new GoldRing { Hue = Utility.Random(1, 3000), Name = "Elira's Arcane Ring" });
            Backpack backpack = new Backpack();
            backpack.Hue = Utility.Random(1, 3000);
            backpack.Name = "Elira's Arcane Satchel";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(RunedPrismCollectorQuest)
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
