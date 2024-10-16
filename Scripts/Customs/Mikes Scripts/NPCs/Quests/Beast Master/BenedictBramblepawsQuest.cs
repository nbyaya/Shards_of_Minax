using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class BenedictBramblepawsQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Benedict Bramblepaws' Oddity Hunt"; } }

        public override object Description
        {
            get
            {
                return 
                    "Greetings, adventurer! I am Benedict Bramblepaws, a connoisseur of the strange and peculiar. " +
                    "To prove your prowess and gain access to my exclusive shop, you must hunt down some of the rarest beasts " +
                    "from the far reaches of the realm. Complete these tasks:\n\n" +
                    "1. Slay an Abyssal Bouncer.\n" +
                    "2. Defeat a Chaos Hare.\n" +
                    "3. Conquer a Cosmic Bouncer.\n" +
                    "4. Overcome an Eldritch Harbinger.\n" +
                    "5. Destroy an Eldritch Hare.\n" +
                    "6. Vanquish an Enigmatic Skipper.\n" +
                    "7. Subdue a Forgotten Warden.\n" +
                    "8. Eradicate an Infinite Pouncer.\n" +
                    "9. Defeat a Nightmare Leaper.\n" +
                    "10. Exterminate a Whispering Pooka.\n\n" +
                    "Complete these tasks, and I will grant you access to my collection of bizarre and wondrous beasts!";
            }
        }

        public override object Refuse { get { return "Very well. Return when you are ready to embrace the oddities of the world."; } }

        public override object Uncomplete { get { return "You haven't completed all the tasks yet. Keep hunting the strange and unusual!"; } }

        public override object Complete { get { return "Remarkable! You have proven yourself worthy of my peculiar collection. My shop is now open to you!"; } }

        public BenedictBramblepawsQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(AbyssalBouncer), "Abyssal Bouncers", 1));
            AddObjective(new SlayObjective(typeof(ChaosHare), "Chaos Hares", 1));
            AddObjective(new SlayObjective(typeof(CosmicBouncer), "Cosmic Bouncers", 1));
            AddObjective(new SlayObjective(typeof(EldritchHarbinger), "Eldritch Harbingers", 1));
            AddObjective(new SlayObjective(typeof(EldritchHare), "Eldritch Hares", 1));
            AddObjective(new SlayObjective(typeof(EnigmaticSkipper), "Enigmatic Skippers", 1));
            AddObjective(new SlayObjective(typeof(ForgottenWarden), "Forgotten Wardens", 1));
            AddObjective(new SlayObjective(typeof(InfinitePouncer), "Infinite Pouncers", 1));
            AddObjective(new SlayObjective(typeof(NightmareLeaper), "Nightmare Leapers", 1));
            AddObjective(new SlayObjective(typeof(WhisperingPooka), "Whispering Pookas", 1));

            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
            AddReward(new BaseReward(typeof(BenToken), 1, "Bramble Token"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You have completed Benedict Bramblepaws' challenge!");
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

    public class BenedictBramblepaws : MondainQuester
    {
        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBenedictBramblepaws());
        }

        [Constructable]
        public BenedictBramblepaws()
            : base("Benedict Bramblepaws", "Master of Peculiar Beasts")
        {
        }

        public BenedictBramblepaws(Serial serial)
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
                Item token = player.Backpack.FindItemByType(typeof(BenToken));
				Item masterToken = player.Backpack.FindItemByType(typeof(MasterToken));

                if (token != null || masterToken != null)
                {
                    SayTo(from, "Welcome to my peculiar beast shop!");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You must have a Bramble Token to access my shop.");
                }
            }
        }

        public override Type[] Quests
        {
            get
            {
                return new Type[]
                {
                    typeof(BenedictBramblepawsQuest)
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

    public class SBBenedictBramblepaws : SBInfo
    {
        private System.Collections.Generic.List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBBenedictBramblepaws()
        {
        }

        public override System.Collections.Generic.List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
        public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

        public class InternalBuyInfo : System.Collections.Generic.List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                // Add the peculiar-themed creatures here
                Add(new AnimalBuyInfo(1, typeof(AbyssalBouncer), 1000, 10, 205, 0));
                Add(new AnimalBuyInfo(1, typeof(ChaosHare), 1200, 10, 205, 0));
                Add(new AnimalBuyInfo(1, typeof(CosmicBouncer), 900, 10, 205, 0));
                Add(new AnimalBuyInfo(1, typeof(EldritchHarbinger), 950, 10, 205, 0));
                Add(new AnimalBuyInfo(1, typeof(EldritchHare), 1100, 10, 205, 0));
                Add(new AnimalBuyInfo(1, typeof(EnigmaticSkipper), 1300, 10, 205, 0));
                Add(new AnimalBuyInfo(1, typeof(ForgottenWarden), 1400, 10, 205, 0));
                Add(new AnimalBuyInfo(1, typeof(InfinitePouncer), 1250, 10, 205, 0));
                Add(new AnimalBuyInfo(1, typeof(NightmareLeaper), 1350, 10, 205, 0));
                Add(new AnimalBuyInfo(1, typeof(WhisperingPooka), 1600, 10, 205, 0));
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
