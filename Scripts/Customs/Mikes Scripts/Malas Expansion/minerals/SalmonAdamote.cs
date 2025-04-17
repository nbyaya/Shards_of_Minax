using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class SalmonAdamote : BaseMineral
    {
        public override string MineralName => "Salmon Adamote";
        public override int MineralHue => 1349;
        public override int MineralGraphic => 0x221A; // Example crystal graphic

        [Constructable]
        public SalmonAdamote() : base()
        {
        }

        public SalmonAdamote(Serial serial) : base(serial)
        {
        }
    }

    public class SalmonAdamoteVein : BaseMineralVein
    {
        public override string VeinName => "a Salmon Adamote vein";
        public override int VeinHue => 1349;
        public override int VeinGraphic => 0x0C45; // Example vein graphic
        public override int HarvestedGraphic => 0x221A; // Example harvested graphic
        public override Type MineralType => typeof(SalmonAdamote);

        [Constructable]
        public SalmonAdamoteVein() : base(0x0C45) // Pass VeinGraphic to base constructor
        {
        }

        public SalmonAdamoteVein(Serial serial) : base(serial)
        {
        }
    }
}
