using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class SangriaBreicophoenicite : BaseMineral
    {
        public override string MineralName => "Sangria Breicophoenicite";
        public override int MineralHue => 1598;
        public override int MineralGraphic => 0x2FDC; // Example crystal graphic

        [Constructable]
        public SangriaBreicophoenicite() : base()
        {
        }

        public SangriaBreicophoenicite(Serial serial) : base(serial)
        {
        }
    }

    public class SangriaBreicophoeniciteVein : BaseMineralVein
    {
        public override string VeinName => "a Sangria Breicophoenicite vein";
        public override int VeinHue => 1598;
        public override int VeinGraphic => 0x0C45; // Example vein graphic
        public override int HarvestedGraphic => 0x2FDC; // Example harvested graphic
        public override Type MineralType => typeof(SangriaBreicophoenicite);

        [Constructable]
        public SangriaBreicophoeniciteVein() : base(0x0C45) // Pass VeinGraphic to base constructor
        {
        }

        public SangriaBreicophoeniciteVein(Serial serial) : base(serial)
        {
        }
    }
}
