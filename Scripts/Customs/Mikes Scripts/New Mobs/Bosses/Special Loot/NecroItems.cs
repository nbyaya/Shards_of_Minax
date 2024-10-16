using System;
using Server;

namespace Server.Items
{
    public class NecromancerTome : Item
    {
        [Constructable]
        public NecromancerTome()
            : base(0x1F4D)
        {
            Name = "Necromancer's Tome";
            Hue = 0x497;
            Weight = 1.0;
        }

        public NecromancerTome(Serial serial) : base(serial)
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

    public class BoneArmor : BaseClothing
    {
        [Constructable]
        public BoneArmor()
            : base(0x144F)
        {
            Name = "Bone Armor";
            Hue = 0x835;
            Weight = 5.0;
        }

        public BoneArmor(Serial serial) : base(serial)
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

    public class DarkCandle : Item
    {
        [Constructable]
        public DarkCandle()
            : base(0xA26)
        {
            Name = "Dark Candle";
            Hue = 0x497;
            Weight = 1.0;
        }

        public DarkCandle(Serial serial) : base(serial)
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

    public class NecromanticAltar : Item
    {
        [Constructable]
        public NecromanticAltar()
            : base(0x2A58)
        {
            Name = "Necromantic Altar";
            Hue = 0x497;
            Weight = 50.0;
        }

        public NecromanticAltar(Serial serial) : base(serial)
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
