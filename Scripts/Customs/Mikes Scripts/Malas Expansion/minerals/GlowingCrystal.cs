using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class GlowingCrystal : BaseMineral
    {
        public override string MineralName => "Glowing Crystal";
        public override int MineralHue => 2154;
        public override int MineralGraphic => 0x1779; // Example crystal graphic

        [Constructable]
        public GlowingCrystal() : base()
        {
        }

        public GlowingCrystal(Serial serial) : base(serial)
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

    public class GlowingCrystalVein : BaseMineralVein
    {
        public override string VeinName => "a glowing crystal vein";
        public override int VeinHue => 2154;
        public override int VeinGraphic => 0x0C45; // Example vein graphic
        public override int HarvestedGraphic => 0x1779; // Example harvested graphic
        public override Type MineralType => typeof(GlowingCrystal);

        [Constructable]
        public GlowingCrystalVein() : base(0x0C45) // Pass VeinGraphic to base constructor
        {
        }

        public GlowingCrystalVein(Serial serial) : base(serial)
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
