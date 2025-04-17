using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class Violermorite : BaseMineral
    {
        public override string MineralName => "Violermorite";
        public override int MineralHue => 302;
        public override int MineralGraphic => 0x2FE6; // Example crystal graphic

        [Constructable]
        public Violermorite() : base()
        {
        }

        public Violermorite(Serial serial) : base(serial)
        {
        }
    }

    public class ViolermoriteVein : BaseMineralVein
    {
        public override string VeinName => "a Violermorite vein";
        public override int VeinHue => 302;
        public override int VeinGraphic => 0x0C45; // Example vein graphic
        public override int HarvestedGraphic => 0x2FE6; // Example harvested graphic
        public override Type MineralType => typeof(Violermorite);

        [Constructable]
        public ViolermoriteVein() : base(0x0C45) // Pass VeinGraphic to base constructor
        {
        }

        public ViolermoriteVein(Serial serial) : base(serial)
        {
        }
    }
}
