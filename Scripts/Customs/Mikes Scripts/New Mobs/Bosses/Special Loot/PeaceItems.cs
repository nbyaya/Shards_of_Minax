using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class PeacemakersManual : Item
    {
        [Constructable]
        public PeacemakersManual()
            : base(0xFF4)
        {
            Weight = 1.0;
            Hue = 0x47E;
            Name = "Peacemaker's Manual";
        }

        public PeacemakersManual(Serial serial)
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

    public class CalmPotion : Item
    {
        [Constructable]
        public CalmPotion()
            : base(0xF0B)
        {
            Weight = 1.0;
            Hue = 0x2D6;
            Name = "Calm Potion";
        }

        public CalmPotion(Serial serial)
            : base(serial)
        {
        }

        private void ReleaseEffect(object state)
        {
            Mobile from = (Mobile)state;
            from.EndAction(typeof(CalmPotion));
            from.SendMessage("The calming effect of the potion wears off.");
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

    public class PeaceLily : Item
    {
        [Constructable]
        public PeaceLily()
            : base(0xC3D)
        {
            Weight = 1.0;
            Hue = 0x484;
            Name = "Peace Lily";
        }

        public PeaceLily(Serial serial)
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
