using System;
using Server;

namespace Server.Items
{
    public class HephaestusHammer : Item
    {
        [Constructable]
        public HephaestusHammer() : base(0x13E3)
        {
            Name = "Hephaestus's Hammer";
            Hue = 0x489;
            Weight = 5.0;
        }

        public HephaestusHammer(Serial serial) : base(serial)
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

namespace Server.Items
{
    public class VolcanicOre : Item
    {
        [Constructable]
        public VolcanicOre() : base(0x19B9)
        {
            Name = "Volcanic Ore";
            Hue = 0x66D;
            Weight = 2.0;
        }

        public VolcanicOre(Serial serial) : base(serial)
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

namespace Server.Items
{
    public class AncientAnvil : Item
    {
        [Constructable]
        public AncientAnvil() : base(0xFAF)
        {
            Name = "Ancient Anvil";
            Hue = 0x835;
            Weight = 10.0;
        }

        public AncientAnvil(Serial serial) : base(serial)
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
