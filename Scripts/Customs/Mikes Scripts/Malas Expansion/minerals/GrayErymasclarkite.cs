using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class GrayErymasclarkite : BaseMineral
    {
        public override string MineralName => "Gray Erymasclarkite";
        public override int MineralHue => 2912;
        public override int MineralGraphic => 0x221A; // Example crystal graphic

        [Constructable]
        public GrayErymasclarkite() : base()
        {
        }

        public GrayErymasclarkite(Serial serial) : base(serial)
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

    public class GrayErymasclarkiteVein : BaseMineralVein
    {
        public override string VeinName => "a Gray Erymasclarkite vein";
        public override int VeinHue => 2912;
        public override int VeinGraphic => 0x0C45; // Example vein graphic
        public override int HarvestedGraphic => 0x221A; // Example harvested graphic
        public override Type MineralType => typeof(GrayErymasclarkite);

        [Constructable]
        public GrayErymasclarkiteVein() : base(0x0C45) // Pass VeinGraphic to base constructor
        {
        }

        public GrayErymasclarkiteVein(Serial serial) : base(serial)
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
