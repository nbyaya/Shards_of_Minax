using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class SpringGreenEudchikhite : BaseMineral
    {
        public override string MineralName => "Spring Green Eudchikhite";
        public override int MineralHue => 774;
        public override int MineralGraphic => 0x2224; // Example crystal graphic

        [Constructable]
        public SpringGreenEudchikhite() : base()
        {
        }

        public SpringGreenEudchikhite(Serial serial) : base(serial)
        {
        }
    }

    public class SpringGreenEudchikhiteVein : BaseMineralVein
    {
        public override string VeinName => "a Spring Green Eudchikhite vein";
        public override int VeinHue => 774;
        public override int VeinGraphic => 0x0C45; // Example vein graphic
        public override int HarvestedGraphic => 0x2224; // Example harvested graphic
        public override Type MineralType => typeof(SpringGreenEudchikhite);

        [Constructable]
        public SpringGreenEudchikhiteVein() : base(0x0C45) // Pass VeinGraphic to base constructor
        {
        }

        public SpringGreenEudchikhiteVein(Serial serial) : base(serial)
        {
        }
    }
}
