using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class Meneochromite : BaseMineral
    {
        public override string MineralName => "Meneochromite";
        public override int MineralHue => 916;
        public override int MineralGraphic => 0x35F6; // Example crystal graphic

        [Constructable]
        public Meneochromite() : base()
        {
        }

        public Meneochromite(Serial serial) : base(serial)
        {
        }
    }

    public class MeneochromiteVein : BaseMineralVein
    {
        public override string VeinName => "a Meneochromite vein";
        public override int VeinHue => 916;
        public override int VeinGraphic => 0x0C45; // Example vein graphic
        public override int HarvestedGraphic => 0x35F6; // Example harvested graphic
        public override Type MineralType => typeof(Meneochromite);

        [Constructable]
        public MeneochromiteVein() : base(0x0C45) // Pass VeinGraphic to base constructor
        {
        }

        public MeneochromiteVein(Serial serial) : base(serial)
        {
        }
    }
}
