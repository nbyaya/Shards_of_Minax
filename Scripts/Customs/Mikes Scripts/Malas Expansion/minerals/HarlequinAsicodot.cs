using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class HarlequinAsicodot : BaseMineral
    {
        public override string MineralName => "Harlequin Asicodot";
        public override int MineralHue => 1974;
        public override int MineralGraphic => 0x1779; // Example crystal graphic

        [Constructable]
        public HarlequinAsicodot() : base()
        {
        }

        public HarlequinAsicodot(Serial serial) : base(serial)
        {
        }
    }

    public class HarlequinAsicodotVein : BaseMineralVein
    {
        public override string VeinName => "a Harlequin Asicodot vein";
        public override int VeinHue => 1974;
        public override int VeinGraphic => 0x0C45; // Example vein graphic
        public override int HarvestedGraphic => 0x1779; // Example harvested graphic
        public override Type MineralType => typeof(HarlequinAsicodot);

        [Constructable]
        public HarlequinAsicodotVein() : base(0x0C45) // Pass VeinGraphic to base constructor
        {
        }

        public HarlequinAsicodotVein(Serial serial) : base(serial)
        {
        }
    }
}
