using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class ZodiacusGrizzlebeardQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Zodiacus Grizzlebeard's Celestial Challenge"; } }

        public override object Description
        {
            get
            {
                return 
                    "Greetings, adventurer! I am Zodiacus Grizzlebeard, master of celestial bears. " +
                    "To prove your worth and gain access to my unique shop, you must hunt down these mighty " +
                    "bears representing the signs of the zodiac. Complete the following tasks:\n\n" +
                    "1. Slay an Aries Ram Bear.\n" +
                    "2. Defeat a Cancer Shell Bear.\n" +
                    "3. Conquer a Capricorn Mountain Bear.\n" +
                    "4. Overcome a Gemini Twin Bear.\n" +
                    "5. Destroy a Leo Sun Bear.\n" +
                    "6. Vanquish a Libra Balance Bear.\n" +
                    "7. Subdue a Sagittarius Archer Bear.\n" +
                    "8. Eradicate a Scorpio Venom Bear.\n" +
                    "9. Defeat a Taurus Earth Bear.\n" +
                    "10. Exterminate a Virgo Purity Bear.\n\n" +
                    "Complete these tasks to gain access to my shop where these celestial bears await!";
            }
        }

        public override object Refuse { get { return "Very well. Return when you are ready to seek the zodiac bears."; } }

        public override object Uncomplete { get { return "You haven't completed all the tasks yet. Keep hunting the bears!"; } }

        public override object Complete { get { return "Astounding! You have proven yourself worthy of the zodiac bears. My shop is now open to you!"; } }

        public ZodiacusGrizzlebeardQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(AriesRamBear), "Aries Ram Bears", 1));
            AddObjective(new SlayObjective(typeof(CancerShellBear), "Cancer Shell Bears", 1));
            AddObjective(new SlayObjective(typeof(CapricornMountainBear), "Capricorn Mountain Bears", 1));
            AddObjective(new SlayObjective(typeof(GeminiTwinBear), "Gemini Twin Bears", 1));
            AddObjective(new SlayObjective(typeof(LeoSunBear), "Leo Sun Bears", 1));
            AddObjective(new SlayObjective(typeof(LibraBalanceBear), "Libra Balance Bears", 1));
            AddObjective(new SlayObjective(typeof(SagittariusArcherBear), "Sagittarius Archer Bears", 1));
            AddObjective(new SlayObjective(typeof(ScorpioVenomBear), "Scorpio Venom Bears", 1));
            AddObjective(new SlayObjective(typeof(TaurusEarthBear), "Taurus Earth Bears", 1));
            AddObjective(new SlayObjective(typeof(VirgoPurityBear), "Virgo Purity Bears", 1));

            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
            AddReward(new BaseReward(typeof(ZodiacToken), 1, "Zodiac Token"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "Congratulations on completing Zodiacus Grizzlebeard's challenge!");
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

    public class ZodiacusGrizzlebeard : MondainQuester
    {
        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBZodiacusGrizzlebeard());
        }

        [Constructable]
        public ZodiacusGrizzlebeard()
            : base("Zodiacus Grizzlebeard", "Master of Celestial Bears")
        {
        }

        public ZodiacusGrizzlebeard(Serial serial)
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
                Item token = player.Backpack.FindItemByType(typeof(ZodiacToken));
				Item masterToken = player.Backpack.FindItemByType(typeof(MasterToken));

                if (token != null || masterToken != null)
                {
                    SayTo(from, "Welcome to my celestial bear shop!");
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
                    typeof(ZodiacusGrizzlebeardQuest)
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

    public class SBZodiacusGrizzlebeard : SBInfo
    {
        private System.Collections.Generic.List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBZodiacusGrizzlebeard()
        {
        }

        public override System.Collections.Generic.List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
        public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

        public class InternalBuyInfo : System.Collections.Generic.List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                // Add the zodiac-themed creatures here
                Add(new AnimalBuyInfo(1, typeof(AriesRamBear), 1000, 10, 212, 0));
                Add(new AnimalBuyInfo(1, typeof(CancerShellBear), 1200, 10, 212, 0));
                Add(new AnimalBuyInfo(1, typeof(CapricornMountainBear), 1100, 10, 212, 0));
                Add(new AnimalBuyInfo(1, typeof(GeminiTwinBear), 1300, 10, 212, 0));
                Add(new AnimalBuyInfo(1, typeof(LeoSunBear), 1400, 10, 212, 0));
                Add(new AnimalBuyInfo(1, typeof(LibraBalanceBear), 1250, 10, 212, 0));
                Add(new AnimalBuyInfo(1, typeof(SagittariusArcherBear), 1350, 10, 212, 0));
                Add(new AnimalBuyInfo(1, typeof(ScorpioVenomBear), 1500, 10, 212, 0));
                Add(new AnimalBuyInfo(1, typeof(TaurusEarthBear), 1150, 10, 212, 0));
                Add(new AnimalBuyInfo(1, typeof(VirgoPurityBear), 1400, 10, 212, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
            }
        }
    }

    public class ZodiacToken : Item
    {
        [Constructable]
        public ZodiacToken() : base(0x1B72)
        {
            Name = "Zodiac Token";
            Hue = 1150; // Optional, for visual distinction
        }

        public ZodiacToken(Serial serial) : base(serial)
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
