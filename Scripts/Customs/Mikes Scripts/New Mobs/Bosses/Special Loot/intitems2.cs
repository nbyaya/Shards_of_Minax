using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class EinsteinChalkboard : Item
    {
        [Constructable]
        public EinsteinChalkboard() : base(0x1E5E)
        {
            Name = "Einstein's Chalkboard";
            Weight = 5.0;
            Hue = 0;
        }

        public EinsteinChalkboard(Serial serial) : base(serial)
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

    public class TheoryOfRelativityScroll : Item
    {
        [Constructable]
        public TheoryOfRelativityScroll() : base(0x46AF)
        {
            Name = "Theory of Relativity Scroll";
            Weight = 1.0;
            Hue = 0x481; // Light yellow hue
        }

        public TheoryOfRelativityScroll(Serial serial) : base(serial)
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

    public class EinsteinPipe : Item
    {
        [Constructable]
        public EinsteinPipe() : base(0xE88)
        {
            Name = "Einstein's Pipe";
            Weight = 1.0;
            Hue = 0x8AB; // Brown hue
        }

        public EinsteinPipe(Serial serial) : base(serial)
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