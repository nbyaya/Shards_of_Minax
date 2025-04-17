using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class Onyxldrenite : BaseMineral
    {
        public override string MineralName => "Onyxldrenite";
        public override int MineralHue => 295;
        public override int MineralGraphic => 0x177A; // Example crystal graphic

        [Constructable]
        public Onyxldrenite() : base()
        {
        }

        public Onyxldrenite(Serial serial) : base(serial)
        {
        }
    }

    public class OnyxldreniteVein : BaseMineralVein
    {
        public override string VeinName => "a Onyxldrenite vein";
        public override int VeinHue => 295;
        public override int VeinGraphic => 0x0C45; // Example vein graphic
        public override int HarvestedGraphic => 0x177A; // Example harvested graphic
        public override Type MineralType => typeof(Onyxldrenite);

        [Constructable]
        public OnyxldreniteVein() : base(0x0C45) // Pass VeinGraphic to base constructor
        {
        }

        public OnyxldreniteVein(Serial serial) : base(serial)
        {
        }
    }
}
