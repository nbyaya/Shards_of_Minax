using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class CrystallineLiberierite : BaseMineral
    {
        public override string MineralName => "Crystalline Liberierite";
        public override int MineralHue => 2154;
        public override int MineralGraphic => 0x2210; // Example crystal graphic

        [Constructable]
        public CrystallineLiberierite() : base()
        {
        }

        public CrystallineLiberierite(Serial serial) : base(serial)
        {
        }
    }

    public class CrystallineLiberieriteVein : BaseMineralVein
    {
        public override string VeinName => "a Crystalline Liberierite vein";
        public override int VeinHue => 2154;
        public override int VeinGraphic => 0x0C45; // Example vein graphic
        public override int HarvestedGraphic => 0x2210; // Example harvested graphic
        public override Type MineralType => typeof(CrystallineLiberierite);

        [Constructable]
        public CrystallineLiberieriteVein() : base(0x0C45) // Pass VeinGraphic to base constructor
        {
        }

        public CrystallineLiberieriteVein(Serial serial) : base(serial)
        {
        }
    }
}
