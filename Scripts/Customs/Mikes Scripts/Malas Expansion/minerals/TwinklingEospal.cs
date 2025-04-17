using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class TwinklingEospal : BaseMineral
    {
        public override string MineralName => "Twinkling Eospal";
        public override int MineralHue => 65;
        public override int MineralGraphic => 0x2FDD; // Example crystal graphic

        [Constructable]
        public TwinklingEospal() : base()
        {
        }

        public TwinklingEospal(Serial serial) : base(serial)
        {
        }
    }

    public class TwinklingEospalVein : BaseMineralVein
    {
        public override string VeinName => "a Twinkling Eospal vein";
        public override int VeinHue => 65;
        public override int VeinGraphic => 0x0C45; // Example vein graphic
        public override int HarvestedGraphic => 0x2FDD; // Example harvested graphic
        public override Type MineralType => typeof(TwinklingEospal);

        [Constructable]
        public TwinklingEospalVein() : base(0x0C45) // Pass VeinGraphic to base constructor
        {
        }

        public TwinklingEospalVein(Serial serial) : base(serial)
        {
        }
    }
}
