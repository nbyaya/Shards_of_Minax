using System;
using Server;

namespace Server.Items
{
    public class PrintersInk : Item
    {
        [Constructable]
        public PrintersInk() : base(0xF8C) // ItemID for ink is just a placeholder, you can change it as needed
        {
            Weight = 1.0;
            Hue = 1153; // Color for the item, can be changed
            Name = "Printer's Ink";
        }

        public PrintersInk(Serial serial) : base(serial)
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

    public class GutenbergsPress : Item
    {
        [Constructable]
        public GutenbergsPress() : base(0x1EB9) // ItemID for a press, can be adjusted
        {
            Weight = 10.0;
            Hue = 0x47E; // Color for the item, can be changed
            Name = "Gutenberg's Press";
        }

        public GutenbergsPress(Serial serial) : base(serial)
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

    public class EnchantedParchment : Item
    {
        [Constructable]
        public EnchantedParchment() : base(0x1F3F) // ItemID for parchment, can be adjusted
        {
            Weight = 1.0;
            Hue = 1152; // Color for the item, can be changed
            Name = "Enchanted Parchment";
        }

        public EnchantedParchment(Serial serial) : base(serial)
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
