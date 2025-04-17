using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class Tunrzalite : BaseMineral
    {
        public override string MineralName => "Tunrzalite";
        public override int MineralHue => 978;
        public override int MineralGraphic => 0x2210; // Example crystal graphic

        [Constructable]
        public Tunrzalite() : base()
        {
        }

        public Tunrzalite(Serial serial) : base(serial)
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

    public class TunrzaliteVein : BaseMineralVein
    {
        public override string VeinName => "a Tunrzalite vein";
        public override int VeinHue => 978;
        public override int VeinGraphic => 0x0C45; // Example vein graphic
        public override int HarvestedGraphic => 0x2210; // Example harvested graphic
        public override Type MineralType => typeof(Tunrzalite);

        [Constructable]
        public TunrzaliteVein() : base(0x0C45) // Pass VeinGraphic to base constructor
        {
        }

        public TunrzaliteVein(Serial serial) : base(serial)
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
