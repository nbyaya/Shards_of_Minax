using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class Baotlusite : BaseMineral
    {
        public override string MineralName => "Baotlusite";
        public override int MineralHue => 2655;
        public override int MineralGraphic => 0x177C; // Example crystal graphic

        [Constructable]
        public Baotlusite() : base()
        {
        }

        public Baotlusite(Serial serial) : base(serial)
        {
        }
    }

    public class BaotlusiteVein : BaseMineralVein
    {
        public override string VeinName => "a Baotlusite vein";
        public override int VeinHue => 2655;
        public override int VeinGraphic => 0x0C45; // Example vein graphic
        public override int HarvestedGraphic => 0x177C; // Example harvested graphic
        public override Type MineralType => typeof(Baotlusite);

        [Constructable]
        public BaotlusiteVein() : base(0x0C45) // Pass VeinGraphic to base constructor
        {
        }

        public BaotlusiteVein(Serial serial) : base(serial)
        {
        }
    }
}
