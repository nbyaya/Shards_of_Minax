using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class Botiolite : BaseMineral
    {
        public override string MineralName => "Botiolite";
        public override int MineralHue => 2275;
        public override int MineralGraphic => 0x177C; // Example crystal graphic

        [Constructable]
        public Botiolite() : base()
        {
        }

        public Botiolite(Serial serial) : base(serial)
        {
        }
    }

    public class BotioliteVein : BaseMineralVein
    {
        public override string VeinName => "a Botiolite vein";
        public override int VeinHue => 2275;
        public override int VeinGraphic => 0x0C45; // Example vein graphic
        public override int HarvestedGraphic => 0x177C; // Example harvested graphic
        public override Type MineralType => typeof(Botiolite);

        [Constructable]
        public BotioliteVein() : base(0x0C45) // Pass VeinGraphic to base constructor
        {
        }

        public BotioliteVein(Serial serial) : base(serial)
        {
        }
    }
}
