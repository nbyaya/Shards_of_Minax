using System;
using Server;

namespace Server.Items
{
    public class MacGyverToolkit : Item
    {
        [Constructable]
        public MacGyverToolkit()
            : base(0x1EBA)
        {
            Weight = 5.0;
            Name = "MacGyver's Toolkit";
            Hue = 0x481;
        }

        public MacGyverToolkit(Serial serial)
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

    public class LockpickingKit : Item
    {
        [Constructable]
        public LockpickingKit()
            : base(0x14FB)
        {
            Weight = 2.0;
            Name = "Lockpicking Kit";
            Hue = 0x482;
        }

        public LockpickingKit(Serial serial)
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

    public class TrapDisarmKit : Item
    {
        [Constructable]
        public TrapDisarmKit()
            : base(0x1EB8)
        {
            Weight = 3.0;
            Name = "Trap Disarm Kit";
            Hue = 0x483;
        }

        public TrapDisarmKit(Serial serial)
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
