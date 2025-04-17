using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class BrumgarQuest : BaseQuest
    {
        public override bool DoneOnce => true;

        public override object Title => "The Grizzly Reckoning";

        public override object Description => 
            "The woods whisper of your strength. I am Brumgar, once a hunter, now a tamer.\n\n" +
            "But the balance of nature is lost. Grizzly bears have multiplied, angered, twisted.\n\n" +
            "Bring down *five hundred* of them, and I will share with you what few dare to offer:\n" +
            "rare wares of the wild—tamed beasts, ancient relics, and strange tokens from nature itself.";

        public override object Refuse => "Nature does not wait. But perhaps you will return when ready.";

        public override object Uncomplete => "The grizzlies still roam. You have not yet completed the hunt.";

        public override object Complete => 
            "It is done. I can feel it—the forest breathes easier.\n\n" +
            "You have earned my trust, and with it, access to my most prized possessions.\n\n" +
            "Use them wisely, friend of the wild.";

        public BrumgarQuest()
        {
            AddObjective(new SlayObjective(typeof(GrizzlyBear), "Grizzly Bears", 500));
            AddReward(new BaseReward(typeof(Gold), 2000, "2000 Gold"));
            AddReward(new BaseReward(typeof(BlueSand), 1, "Access to Brumgar's Shop"));
        }

        public override void OnCompleted()
        {
            base.OnCompleted();

            var profile = Owner.AcquireTalents();
            if (!profile.Talents.ContainsKey(TalentID.GrizzlyBearSlayerQuest))
                profile.Talents[TalentID.GrizzlyBearSlayerQuest] = new Talent(TalentID.GrizzlyBearSlayerQuest);

            profile.Talents[TalentID.GrizzlyBearSlayerQuest].Points = 1;

            Owner.SendMessage(0x23, "You have slain 500 Grizzly Bears for Brumgar!");
            Owner.PlaySound(CompleteSound);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class Brumgar : MondainQuester
    {
        public override bool IsActiveVendor => true;

        private List<SBInfo> m_SBInfos = new List<SBInfo>();

        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBrumgar());
        }

        [Constructable]
        public Brumgar() : base("Brumgar", "The Wilds Tamer") { }

        public Brumgar(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            Female = false;
            Race = Race.Human;

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x2048;
            HairHue = Utility.RandomHairHue();
        }

        public override void InitOutfit()
        {
            AddItem(new Backpack());
            AddItem(new BearMask());
            AddItem(new FurCape());
            AddItem(new StuddedGloves());
            AddItem(new StuddedGorget());
            AddItem(new Sandals());
        }

        public override void VendorBuy(Mobile from)
        {
            PlayerMobile player = from as PlayerMobile;
            if (player != null)
            {
                var profile = player.AcquireTalents();
                Talent talent;

                if (profile.Talents.TryGetValue(TalentID.GrizzlyBearSlayerQuest, out talent) && talent.Points > 0)
                {
                    SayTo(from, "Welcome, friend of the wild. My stock is yours to explore.");
                    base.VendorBuy(from);
                }
                else
                {
                    SayTo(from, "You must first prove yourself against the Grizzlies.");
                }
            }
        }

        public override Type[] Quests => new Type[] { typeof(BrumgarQuest) };

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class SBBrumgar : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public override List<GenericBuyInfo> BuyInfo => m_BuyInfo;
        public override IShopSellInfo SellInfo => m_SellInfo;

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new AnimalBuyInfo(1, typeof(GrizzlyBear), 5000, 10, 17, 0));
                Add(new GenericBuyInfo(typeof(LargeWeatheredBook), 5000, 10, 0x0FEF, 0));
                Add(new GenericBuyInfo(typeof(WeddingCandelabra), 5000, 10, 0xB1E, 0));
                Add(new GenericBuyInfo(typeof(DeckOfMagicCards), 5000, 10, 0x12AB, 0));
                Add(new GenericBuyInfo(typeof(BrokenBottle), 5000, 10, 0x0924, 0));
                Add(new GenericBuyInfo(typeof(FancyHornOfPlenty), 5000, 10, 0x171C, 0));
                Add(new GenericBuyInfo(typeof(FineGoldWire), 5000, 10, 0x1EBB, 0));
                Add(new GenericBuyInfo(typeof(WaterRelic), 5000, 10, 0xF8C, 0));
                Add(new GenericBuyInfo(typeof(EnchantedAnnealer), 5000, 10, 0x186A, 0));
                Add(new GenericBuyInfo(typeof(LampPostC), 5000, 10, 0x0A29, 0));
                Add(new GenericBuyInfo(typeof(BlueSand), 5000, 10, 0x11C6, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo() { }
        }
    }
}
