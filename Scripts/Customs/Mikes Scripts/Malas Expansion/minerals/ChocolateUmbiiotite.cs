using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class ChocolateUmbiiotite : BaseMineral
    {
        public override string MineralName => "Chocolate Umbiiotite";
        public override int MineralHue => 1018;
        public override int MineralGraphic => 0x2FE7; // Example crystal graphic

        [Constructable]
        public ChocolateUmbiiotite() : base()
        {
        }

        public ChocolateUmbiiotite(Serial serial) : base(serial)
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

    public class ChocolateUmbiiotiteVein : BaseMineralVein
    {
        public override string VeinName => "a Chocolate Umbiiotite vein";
        public override int VeinHue => 1018;
        public override int VeinGraphic => 0x0C45; // Example vein graphic
        public override int HarvestedGraphic => 0x2FE7; // Example harvested graphic
        public override Type MineralType => typeof(ChocolateUmbiiotite);

        [Constructable]
        public ChocolateUmbiiotiteVein() : base(0x0C45) // Pass VeinGraphic to base constructor
        {
        }

        public ChocolateUmbiiotiteVein(Serial serial) : base(serial)
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
