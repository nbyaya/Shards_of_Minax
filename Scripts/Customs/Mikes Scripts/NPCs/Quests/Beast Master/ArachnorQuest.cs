using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class ArachnorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Arachnor the Eight-Legged's Spider Hunt"; } }

        public override object Description
        {
            get
            {
                return 
                    "Greetings, adventurer! I am Arachnor the Eight-Legged, master of arachnids. " +
                    "To prove your worth and gain access to my exclusive shop, you must show " +
                    "your bravery by defeating these formidable spiders:\n\n" +
                    "1. Slay a Black Widow Queen.\n" +
                    "2. Defeat a Giant Trapdoor Spider.\n" +
                    "3. Conquer a Giant Wolf Spider.\n" +
                    "4. Overcome a Golden Orb Weaver.\n" +
                    "5. Destroy a Goliath Birdeater.\n" +
                    "6. Vanquish a Huntsman Spider.\n" +
                    "7. Subdue a Purse Spider.\n" +
                    "8. Eradicate a Scorpion Spider.\n" +
                    "9. Defeat a Spiderling Broodmother.\n" +
                    "10. Exterminate a Tarantula Warrior.\n\n" +
                    "Complete these tasks, and I will grant you access to my arachnid beast shop!";
            }
        }

        public override object Refuse { get { return "Very well. Return when you are ready to face the spiders."; } }

        public override object Uncomplete { get { return "You haven't completed all the tasks yet. Keep hunting the spiders!"; } }

        public override object Complete { get { return "Impressive! You have proven yourself against the spiders. My shop is now open to you!"; } }

        public ArachnorQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(BlackWidowQueen), "Black Widow Queens", 1));
            AddObjective(new SlayObjective(typeof(GiantTrapdoorSpider), "Giant Trapdoor Spiders", 1));
            AddObjective(new SlayObjective(typeof(GiantWolfSpider), "Giant Wolf Spiders", 1));
            AddObjective(new SlayObjective(typeof(GoldenOrbWeaver), "Golden Orb Weavers", 1));
            AddObjective(new SlayObjective(typeof(GoliathBirdeater), "Goliath Birdeaters", 1));
            AddObjective(new SlayObjective(typeof(HuntsmanSpider), "Huntsman Spiders", 1));
            AddObjective(new SlayObjective(typeof(PurseSpider), "Purse Spiders", 1));
            AddObjective(new SlayObjective(typeof(ScorpionSpider), "Scorpion Spiders", 1));
            AddObjective(new SlayObjective(typeof(SpiderlingMinionBroodmother), "Spiderling Broodmothers", 1));
            AddObjective(new SlayObjective(typeof(TarantulaWarrior), "Tarantula Warriors", 1));

            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
            AddReward(new BaseReward(typeof(ArachnidToken), 1, "Arachnid Token"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You have completed Arachnor's spider hunt!");
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

    public class Arachnor : MondainQuester
    {
        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBArachnor());
        }

        [Constructable]
        public Arachnor()
            : base("Arachnor the Eight-Legged", "Master of Arachnids")
        {
        }

        public Arachnor(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Female = false;
            Race = Race.Human;

            Hue = Utility.RandomSkinHue();
            HairItemID = Utility.RandomList(0x203B, 0x2049, 0x2048);
            HairHue = Utility.RandomHairHue();
            FacialHairItemID = Utility.RandomList(0x203E, 0x203F, 0x2040, 0x2041, 0x204B, 0x204C, 0x204D);
            FacialHairHue = HairHue;
        }

        public override void InitOutfit()
        {
            AddItem(new Backpack());
            AddItem(new Boots(Utility.RandomNeutralHue()));
            AddItem(new LongPants(Utility.RandomNeutralHue()));
            AddItem(new FancyShirt(Utility.RandomNeutralHue()));
            AddItem(new Cloak(Utility.RandomNeutralHue()));
        }

        public override void VendorBuy(Mobile from)
        {
            PlayerMobile player = from as PlayerMobile;
            if (player != null)
            {
                Item token = player.Backpack.FindItemByType(typeof(ArachnidToken));

                if (token != null)
                {
                    SayTo(from, "Welcome to my arachnid beast shop!");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You must have an Arachnid Token to access my shop.");
                }
            }
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(ArachnorQuest)
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

    public class SBArachnor : SBInfo
    {
        private System.Collections.Generic.List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBArachnor()
        {
        }

        public override System.Collections.Generic.List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
        public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

        public class InternalBuyInfo : System.Collections.Generic.List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                // Add the spider-themed creatures here
                Add(new AnimalBuyInfo(1, typeof(BlackWidowQueen), 1000, 10, 28, 0));
                Add(new AnimalBuyInfo(1, typeof(GiantTrapdoorSpider), 1200, 10, 28, 0));
                Add(new AnimalBuyInfo(1, typeof(GiantWolfSpider), 900, 10, 28, 0));
                Add(new AnimalBuyInfo(1, typeof(GoldenOrbWeaver), 950, 10, 28, 0));
                Add(new AnimalBuyInfo(1, typeof(GoliathBirdeater), 1100, 10, 28, 0));
                Add(new AnimalBuyInfo(1, typeof(HuntsmanSpider), 1300, 10, 28, 0));
                Add(new AnimalBuyInfo(1, typeof(PurseSpider), 1400, 10, 28, 0));
                Add(new AnimalBuyInfo(1, typeof(ScorpionSpider), 1250, 10, 28, 0));
                Add(new AnimalBuyInfo(1, typeof(SpiderlingMinionBroodmother), 1350, 10, 28, 0));
                Add(new AnimalBuyInfo(1, typeof(TarantulaWarrior), 1600, 10, 28, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
            }
        }
    }
}
