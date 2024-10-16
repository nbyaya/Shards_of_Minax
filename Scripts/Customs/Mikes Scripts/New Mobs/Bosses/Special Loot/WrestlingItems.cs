using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class ChampionshipBelt : Item
    {
        [Constructable]
        public ChampionshipBelt() : base(0x2B68)
        {
            Weight = 1.0;
            Name = "Championship Belt";
            Hue = 0x501;
        }

        public ChampionshipBelt(Serial serial) : base(serial)
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

    public class WrestlersGloves : Item
    {
        [Constructable]
        public WrestlersGloves() : base(0x13C6)
        {
            Weight = 2.0;
            Name = "Wrestler's Gloves";
            Hue = 900;
        }

        public WrestlersGloves(Serial serial) : base(serial)
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

    public class WrestlingTome : Item
    {
        [Constructable]
        public WrestlingTome() : base(0x1C12)
        {
            Weight = 1.0;
            Name = "Wrestling Tome";
            Hue = 0x46E;

        }

        public WrestlingTome(Serial serial) : base(serial)
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

    public class WrestlersStatue : Item
    {
        [Constructable]
        public WrestlersStatue() : base(0x20E7)
        {
            Weight = 10.0;
            Name = "Wrestler's Statue";
            Hue = 0x48F;
        }

        public WrestlersStatue(Serial serial) : base(serial)
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
