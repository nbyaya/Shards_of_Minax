using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    public class TravelingMerchant : BaseVendor
    {
        private readonly List<SBInfo> m_SBInfos = new List<SBInfo>();

        [Constructable]
        public TravelingMerchant()
            : base("the traveling merchant")
        {
            // Optional: set skills or fame/karma if desired
        }

        // ─── Enable the randomised‐stock system ──────────────────────────────
        public override bool UsesRandomisedStock => true;

        // ─── Per‐vendor parameters ───────────────────────────────────────────
        public override double RandomStockInclusionChance => 0.75;   // 75% chance to include each item
        public override int    RandomStockMinQty        => 1;      // minimum stack size
        public override int    RandomStockMaxQty        => 25;     // maximum stack size
        public override double RandomPriceFactorMin     => 0.85;   // –15% on price
        public override double RandomPriceFactorMax     => 1.25;   // +25% on price

        // ─── SB list wiring ─────────────────────────────────────────────────
        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBTravelingMerchant());
        }

        public TravelingMerchant(Serial serial)
            : base(serial)
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

    // ─── Simple SBInfo for the TravelingMerchant ──────────────────────────
    public class SBTravelingMerchant : SBInfo
    {
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo      m_SellInfo = new InternalSellInfo();

        public override List<GenericBuyInfo> BuyInfo  => m_BuyInfo;
        public override IShopSellInfo        SellInfo => m_SellInfo;

        // ── What the merchant offers for sale ─────────────────────────────
        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                // typeof(ItemType), price, maxQty, itemID, hue
                Add(new GenericBuyInfo(typeof(Apple),  2, 50, 0x9D0, 0));
                Add(new GenericBuyInfo(typeof(Rope), 10, 20, 0x104, 0));
                Add(new GenericBuyInfo(typeof(Torch), 5, 30, 0xF6B, 0));
                Add(new GenericBuyInfo(typeof(Shovel),15, 10, 0xF39, 0));
            }
        }

        // ── What the merchant will buy from players ───────────────────────
        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
                Add(typeof(Apple),   1);
                Add(typeof(Rope),    5);
                Add(typeof(Torch),   3);
                Add(typeof(Shovel),  8);
            }
        }
    }
}
