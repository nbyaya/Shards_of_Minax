using System;
using Server.Items;

namespace Server.Items
{
    public class WoolenCloak : BaseCloak
    {
        [Constructable]
        public WoolenCloak()
            : base(0x1515) // Cloak Item ID
        {
            Name = "Woolen Cloak";
            Hue = 0x47E; // Custom hue

            Attributes.RegenHits = 3;
            Attributes.RegenStam = 3;
            Attributes.LowerManaCost = 5;
            Attributes.LowerRegCost = 10;
        }

        public WoolenCloak(Serial serial)
            : base(serial)
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

    public class HerdingTome : Item
    {
        [Constructable]
        public HerdingTome()
            : base(0x1C12) // Book Item ID
        {
            Name = "Tome of Herding";
            Hue = 0x47E; // Custom hue
        }

        public HerdingTome(Serial serial)
            : base(serial)
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

    public class Fleece : Item
    {
        [Constructable]
        public Fleece()
            : base(0x101F) // Wool Item ID
        {
            Name = "Fleece";
            Hue = 0x47E; // Custom hue
        }

        public Fleece(Serial serial)
            : base(serial)
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

    public class SheepStatuette : Item
    {
        [Constructable]
        public SheepStatuette()
            : base(0x20DA) // Statuette Item ID
        {
            Name = "Sheep Statuette";
            Hue = 0x47E; // Custom hue
            LootType = LootType.Blessed;
        }

        public SheepStatuette(Serial serial)
            : base(serial)
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
