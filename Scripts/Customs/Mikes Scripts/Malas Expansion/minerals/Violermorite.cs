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
