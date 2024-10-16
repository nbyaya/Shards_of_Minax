using System;
using Server;
using Server.Items;

namespace Server.Items
{
    // Inventor's Toolkit
    public class InventorsToolkit : Item
    {
        [Constructable]
        public InventorsToolkit() : base(0x1EBA)
        {
            Name = "Inventor's Toolkit";
            Weight = 1.0;
            Hue = 0x48D;
        }

        public InventorsToolkit(Serial serial) : base(serial)
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

    // Lightbulb Pendant
    public class LightbulbPendant : Item
    {
        [Constructable]
        public LightbulbPendant() : base(0x4CFA)
        {
            Name = "Lightbulb Pendant";
            Weight = 1.0;
            Hue = 0x48D;
        }

        public LightbulbPendant(Serial serial) : base(serial)
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

    // Small Clock
    public class SmallClock : Item
    {
        [Constructable]
        public SmallClock() : base(0x104B)
        {
            Name = "Small Clock";
            Weight = 1.0;
            Hue = 0x48D;
        }

        public SmallClock(Serial serial) : base(serial)
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
