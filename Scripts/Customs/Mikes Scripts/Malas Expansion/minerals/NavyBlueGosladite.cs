using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class NavyBlueGosladite : BaseMineral
    {
        public override string MineralName => "Navy Blue Gosladite";
        public override int MineralHue => 1687;
        public override int MineralGraphic => 0x177B; // Example crystal graphic

        [Constructable]
        public NavyBlueGosladite() : base()
        {
        }

        public NavyBlueGosladite(Serial serial) : base(serial)
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

    public class NavyBlueGosladiteVein : BaseMineralVein
    {
        public override string VeinName => "a Navy Blue Gosladite vein";
        public override int VeinHue => 1687;
        public override int VeinGraphic => 0x0C45; // Example vein graphic
        public override int HarvestedGraphic => 0x177B; // Example harvested graphic
        public override Type MineralType => typeof(NavyBlueGosladite);

        [Constructable]
        public NavyBlueGosladiteVein() : base(0x0C45) // Pass VeinGraphic to base constructor
        {
        }

        public NavyBlueGosladiteVein(Serial serial) : base(serial)
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
