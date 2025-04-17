using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class Maringstonite : BaseMineral
    {
        public override string MineralName => "Maringstonite";
        public override int MineralHue => 2258;
        public override int MineralGraphic => 0x35F7; // Example crystal graphic

        [Constructable]
        public Maringstonite() : base()
        {
        }

        public Maringstonite(Serial serial) : base(serial)
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

    public class MaringstoniteVein : BaseMineralVein
    {
        public override string VeinName => "a Maringstonite vein";
        public override int VeinHue => 2258;
        public override int VeinGraphic => 0x0C45; // Example vein graphic
        public override int HarvestedGraphic => 0x35F7; // Example harvested graphic
        public override Type MineralType => typeof(Maringstonite);

        [Constructable]
        public MaringstoniteVein() : base(0x0C45) // Pass VeinGraphic to base constructor
        {
        }

        public MaringstoniteVein(Serial serial) : base(serial)
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
