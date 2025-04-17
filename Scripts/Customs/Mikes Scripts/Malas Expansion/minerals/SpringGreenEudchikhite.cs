using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class SpringGreenEudchikhite : BaseMineral
    {
        public override string MineralName => "Spring Green Eudchikhite";
        public override int MineralHue => 774;
        public override int MineralGraphic => 0x2224; // Example crystal graphic

        [Constructable]
        public SpringGreenEudchikhite() : base()
        {
        }

        public SpringGreenEudchikhite(Serial serial) : base(serial)
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

    public class SpringGreenEudchikhiteVein : BaseMineralVein
    {
        public override string VeinName => "a Spring Green Eudchikhite vein";
        public override int VeinHue => 774;
        public override int VeinGraphic => 0x0C45; // Example vein graphic
        public override int HarvestedGraphic => 0x2224; // Example harvested graphic
        public override Type MineralType => typeof(SpringGreenEudchikhite);

        [Constructable]
        public SpringGreenEudchikhiteVein() : base(0x0C45) // Pass VeinGraphic to base constructor
        {
        }

        public SpringGreenEudchikhiteVein(Serial serial) : base(serial)
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
