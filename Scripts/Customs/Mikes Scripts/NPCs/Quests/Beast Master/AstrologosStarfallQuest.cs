using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class AstrologosStarfallQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Astrologos Starfall's Zodiac Challenge"; } }

        public override object Description
        {
            get
            {
                return 
                    "Greetings, adventurer! I am Astrologos Starfall, keeper of celestial secrets. " +
                    "To prove your worth and gain access to my exclusive shop, you must embark on a journey " +
                    "to hunt the harpies of the zodiac. Complete these tasks:\n\n" +
                    "1. Slay an Aries Harpy.\n" +
                    "2. Defeat a Cancer Harpy.\n" +
                    "3. Conquer a Capricorn Harpy.\n" +
                    "4. Overcome a Gemini Harpy.\n" +
                    "5. Destroy a Leo Harpy.\n" +
                    "6. Vanquish a Libra Harpy.\n" +
                    "7. Subdue a Sagittarius Harpy.\n" +
                    "8. Eradicate a Scorpio Harpy.\n" +
                    "9. Defeat a Taurus Harpy.\n" +
                    "10. Exterminate a Virgo Harpy.\n\n" +
                    "Complete these tasks, and I will grant you access to my celestial beast shop!";
            }
        }

        public override object Refuse { get { return "Very well. Return when you are ready to face the celestial harpies."; } }

        public override object Uncomplete { get { return "You haven't completed all the tasks yet. The stars are not aligned!"; } }

        public override object Complete { get { return "Remarkable! You have conquered the celestial harpies. My shop is now open to you!"; } }

        public AstrologosStarfallQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(AriesHarpy), "Aries Harpies", 1));
            AddObjective(new SlayObjective(typeof(CancerHarpy), "Cancer Harpies", 1));
            AddObjective(new SlayObjective(typeof(CapricornHarpy), "Capricorn Harpies", 1));
            AddObjective(new SlayObjective(typeof(GeminiHarpy), "Gemini Harpies", 1));
            AddObjective(new SlayObjective(typeof(LeoHarpy), "Leo Harpies", 1));
            AddObjective(new SlayObjective(typeof(LibraHarpy), "Libra Harpies", 1));
            AddObjective(new SlayObjective(typeof(SagittariusHarpy), "Sagittarius Harpies", 1));
            AddObjective(new SlayObjective(typeof(ScorpioHarpy), "Scorpio Harpies", 1));
            AddObjective(new SlayObjective(typeof(TaurusHarpy), "Taurus Harpies", 1));
            AddObjective(new SlayObjective(typeof(VirgoHarpy), "Virgo Harpies", 1));

            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
            AddReward(new BaseReward(typeof(AstroToken), 1, "Zodiac Token"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You have completed Astrologos Starfall's challenge!");
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

    public class AstrologosStarfall : MondainQuester
    {
        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBAstrologosStarfall());
        }

        [Constructable]
        public AstrologosStarfall()
            : base("Astrologos Starfall", "Keeper of Celestial Secrets")
        {
        }

        public AstrologosStarfall(Serial serial)
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
                Item token = player.Backpack.FindItemByType(typeof(AstroToken));
				Item masterToken = player.Backpack.FindItemByType(typeof(MasterToken));

                if (token != null || masterToken != null)
                {
                    SayTo(from, "Welcome to my celestial beast shop!");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You must have a Zodiac Token to access my shop.");
                }
            }
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(AstrologosStarfallQuest)
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

    public class SBAstrologosStarfall : SBInfo
    {
        private System.Collections.Generic.List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBAstrologosStarfall()
        {
        }

        public override System.Collections.Generic.List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
        public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

        public class InternalBuyInfo : System.Collections.Generic.List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                // Add the zodiac-themed creatures here
                Add(new AnimalBuyInfo(1, typeof(AriesHarpy), 1000, 10, 30, 0));
                Add(new AnimalBuyInfo(1, typeof(CancerHarpy), 1200, 10, 30, 0));
                Add(new AnimalBuyInfo(1, typeof(CapricornHarpy), 1100, 10, 30, 0));
                Add(new AnimalBuyInfo(1, typeof(GeminiHarpy), 950, 10, 30, 0));
                Add(new AnimalBuyInfo(1, typeof(LeoHarpy), 1300, 10, 30, 0));
                Add(new AnimalBuyInfo(1, typeof(LibraHarpy), 1050, 10, 30, 0));
                Add(new AnimalBuyInfo(1, typeof(SagittariusHarpy), 1150, 10, 30, 0));
                Add(new AnimalBuyInfo(1, typeof(ScorpioHarpy), 1250, 10, 30, 0));
                Add(new AnimalBuyInfo(1, typeof(TaurusHarpy), 1000, 10, 30, 0));
                Add(new AnimalBuyInfo(1, typeof(VirgoHarpy), 950, 10, 30, 0));
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
