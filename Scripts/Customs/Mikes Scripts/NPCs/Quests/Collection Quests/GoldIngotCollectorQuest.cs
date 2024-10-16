using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class GoldIngotCollectorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Gold Ingot Collector's Request"; } }

        public override object Description
        {
            get
            {
                return "Greetings, adventurer! I am in need of gold ingots for a special project. " +
                       "If you could bring me 50 gold ingots, I will reward you generously!";
            }
        }

        public override object Refuse { get { return "Very well. Return if you change your mind."; } }

        public override object Uncomplete { get { return "You haven't collected enough gold ingots yet. Keep trying!"; } }

        public override object Complete { get { return "Thank you for the gold ingots! Here is your reward."; } }

        public GoldIngotCollectorQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(GoldIngot), "Gold Ingots", 50, 0x1BF2));
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(MaxxiaScroll), 1, "Maxxia Scroll"));
			AddReward(new BaseReward(typeof(AlmsmansAegis), 1, "Almsmans Aegis")); // Assuming Alchemist's Robe is a custom item
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You have completed the Gold Ingot Collector's Request!");
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

    public class GoldIngotCollector : MondainQuester
    {
        [Constructable]
        public GoldIngotCollector()
            : base("The Treasure Seeker", "Gold Ingot Collector")
        {
        }

        public GoldIngotCollector(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 30);

            Body = 0x190; // Male Body

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x203C; // Random hair style
            HairHue = 1101; // Hair hue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe(0x8A)); // Elegant robe
            AddItem(new Sandals(0x3B)); // Fancy sandals
            AddItem(new FeatheredHat(0x1F)); // Feathered hat
            AddItem(new GoldBracelet { Name = "Collector's Gold Bracelet", Hue = 0x8A }); // Decorative item
            Backpack backpack = new Backpack();
            backpack.Hue = 0x8A; // Golden color
            backpack.Name = "Collector's Treasure Bag";
            AddItem(backpack);
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(GoldIngotCollectorQuest)
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
