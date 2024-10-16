using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class DesignersScissors : Item
    {
        [Constructable]
        public DesignersScissors() : base(0xF9F) // Scissors graphic
        {
            Name = "Designer's Scissors";
            Hue = 1153; // Custom hue
            Weight = 1.0;
        }

        public DesignersScissors(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class CoutureFabric : Item
    {
        [Constructable]
        public CoutureFabric() : base(0x1766) // Fabric graphic
        {
            Name = "Couture Fabric";
            Hue = 1154; // Custom hue
            Weight = 5.0;
        }

        public CoutureFabric(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class EmbroideredShirt : BaseShirt
    {
        [Constructable]
        public EmbroideredShirt() : base(0x1EFD) // Shirt graphic
        {
            Name = "Embroidered Shirt";
            Hue = 1155; // Custom hue
            Weight = 2.0;
        }

        public EmbroideredShirt(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class FashionMannequin : Item
    {
        [Constructable]
        public FashionMannequin() : base(0x2106) // Mannequin graphic
        {
            Name = "Fashion Mannequin";
            Hue = 1156; // Custom hue
            Weight = 10.0;
        }

        public FashionMannequin(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class FineCloth : Item
    {
        [Constructable]
        public FineCloth() : base(0x1767) // Cloth graphic
        {
            Name = "Fine Cloth";
            Hue = 1157; // Custom hue
            Weight = 5.0;
        }

        public FineCloth(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
