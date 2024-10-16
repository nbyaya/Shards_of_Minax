using System;
using Server;

namespace Server.Items
{
    public class MagicalWand : Item
    {
        [Constructable]
        public MagicalWand() : base(0xDF5)
        {
            Weight = 2.0;
            Name = "Magical Wand";
            Hue = 0x489;
        }

        public MagicalWand(Serial serial) : base(serial)
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

    public class ArcaneCrystal : Item
    {
        [Constructable]
        public ArcaneCrystal() : base(0x1F1C)
        {
            Weight = 1.0;
            Name = "Arcane Crystal";
            Hue = 0x486;
        }

        public ArcaneCrystal(Serial serial) : base(serial)
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

    public class EnchantedLantern : Item
    {
        [Constructable]
        public EnchantedLantern() : base(0xA25)
        {
            Weight = 3.0;
            Name = "Enchanted Lantern";
            Hue = 0x47E;
            Light = LightType.Circle300;
        }

        public EnchantedLantern(Serial serial) : base(serial)
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
