using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class PeriwinkleMarclesite : BaseMineral
    {
        public override string MineralName => "Periwinkle Marclesite";
        public override int MineralHue => 1987;
        public override int MineralGraphic => 0x177A; // Example crystal graphic

        [Constructable]
        public PeriwinkleMarclesite() : base()
        {
        }

        public PeriwinkleMarclesite(Serial serial) : base(serial)
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

    public class PeriwinkleMarclesiteVein : BaseMineralVein
    {
        public override string VeinName => "a Periwinkle Marclesite vein";
        public override int VeinHue => 1987;
        public override int VeinGraphic => 0x0C45; // Example vein graphic
        public override int HarvestedGraphic => 0x177A; // Example harvested graphic
        public override Type MineralType => typeof(PeriwinkleMarclesite);

        [Constructable]
        public PeriwinkleMarclesiteVein() : base(0x0C45) // Pass VeinGraphic to base constructor
        {
        }

        public PeriwinkleMarclesiteVein(Serial serial) : base(serial)
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
