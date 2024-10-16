using System;
using Server.Items;

namespace Server.Items
{
    public class ArcherTalisman : Item
    {
        [Constructable]
        public ArcherTalisman() : base(0x2F58)
        {
            Name = "Archer's Talisman";
            Hue = 0x482;
        }

        public ArcherTalisman(Serial serial) : base(serial)
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

    public class Quiver : Item
    {
        [Constructable]
        public Quiver() : base(0x2FB7)
        {
            Name = "Quiver of the Hawk";
            Hue = 0x59C;
        }

        public Quiver(Serial serial) : base(serial)
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

    public class TrophyArrow : Item
    {
        [Constructable]
        public TrophyArrow() : base(0xF3F)
        {
            Name = "Trophy Arrow";
            Hue = 0x44E;
        }

        public TrophyArrow(Serial serial) : base(serial)
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

    public class BowStand : Item
    {
        [Constructable]
        public BowStand() : base(0x3EAA)
        {
            Name = "Bow Stand";
            Hue = 0x83F;
        }

        public BowStand(Serial serial) : base(serial)
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
