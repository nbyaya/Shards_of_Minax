using System;
using Server.Items;

namespace Server.Items
{
    public class MapOfTheWorld : Item
    {
        [Constructable]
        public MapOfTheWorld() : base(0x14EB) // Use appropriate item ID
        {
            Name = "Map of the World";
            Hue = 0x48E;
        }

        public MapOfTheWorld(Serial serial) : base(serial)
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

    public class CartographerGlobe : Item
    {
        [Constructable]
        public CartographerGlobe() : base(0x14F0) // Use appropriate item ID
        {
            Name = "Cartographer's Globe";
            Hue = 0x482;
        }

        public CartographerGlobe(Serial serial) : base(serial)
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

    public class CartographerTools : Item
    {
        [Constructable]
        public CartographerTools() : base(0x1EB8) // Use appropriate item ID
        {
            Name = "Cartographer's Tools";
            Hue = 0x482;
        }

        public CartographerTools(Serial serial) : base(serial)
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
