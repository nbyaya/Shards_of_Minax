using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class GrayFerrellyite : BaseMineral
    {
        public override string MineralName => "Gray Ferrellyite";
        public override int MineralHue => 2032;
        public override int MineralGraphic => 0x177B; // Example crystal graphic

        [Constructable]
        public GrayFerrellyite() : base()
        {
        }

        public GrayFerrellyite(Serial serial) : base(serial)
        {
        }
		
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Versioning for future changes, start with version 0.
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt(); // Read the version number, used for future migrations.
        }		
    }

    public class GrayFerrellyiteVein : BaseMineralVein
    {
        public override string VeinName => "a Gray Ferrellyite vein";
        public override int VeinHue => 2032;
        public override int VeinGraphic => 0x0C45; // Example vein graphic
        public override int HarvestedGraphic => 0x177B; // Example harvested graphic
        public override Type MineralType => typeof(GrayFerrellyite);

        [Constructable]
        public GrayFerrellyiteVein() : base(0x0C45) // Pass VeinGraphic to base constructor
        {
        }

        public GrayFerrellyiteVein(Serial serial) : base(serial)
        {
        }
		
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Versioning for future changes, start with version 0.
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt(); // Read the version number, used for future migrations.
        }		
    }
}
