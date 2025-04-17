using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class Agreramite : BaseMineral
    {
        public override string MineralName => "Agreramite";
        public override int MineralHue => 2091;
        public override int MineralGraphic => 0x2206; // Example crystal graphic

        [Constructable]
        public Agreramite() : base()
        {
        }

        public Agreramite(Serial serial) : base(serial)
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

    public class AgreramiteVein : BaseMineralVein
    {
        public override string VeinName => "a Agreramite vein";
        public override int VeinHue => 2091;
        public override int VeinGraphic => 0x0C45; // Example vein graphic
        public override int HarvestedGraphic => 0x2206; // Example harvested graphic
        public override Type MineralType => typeof(Agreramite);

        [Constructable]
        public AgreramiteVein() : base(0x0C45) // Pass VeinGraphic to base constructor
        {
        }

        public AgreramiteVein(Serial serial) : base(serial)
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
