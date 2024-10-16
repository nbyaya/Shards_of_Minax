using System;
using Server;

namespace Server.Items
{
    public class NightingalesLamp : Item
    {
        [Constructable]
        public NightingalesLamp() : base(0xA22)
        {
            Movable = true;
            Hue = 0x47E;
            Name = "Nightingale's Lamp";
        }

        public NightingalesLamp(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class BandageOfHealing : Item
    {
        [Constructable]
        public BandageOfHealing() : base(0xE21)
        {
            Movable = true;
            Hue = 0x48E;
            Name = "Bandage of Healing";
        }

        public BandageOfHealing(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
