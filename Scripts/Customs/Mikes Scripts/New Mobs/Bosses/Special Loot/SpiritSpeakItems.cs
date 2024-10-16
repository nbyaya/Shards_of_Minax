using System;
using Server;

namespace Server.Items
{
    public class CrystalSkull : Item
    {
        [Constructable]
        public CrystalSkull() : base(0x1F18)
        {
            Name = "Crystal Skull";
            Hue = 0x482;
            Weight = 5.0;
        }

        public CrystalSkull(Serial serial) : base(serial)
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

    public class NecromancersRobes : BaseClothing
    {
        [Constructable]
        public NecromancersRobes() : base(0x1F03)
        {
            Name = "Necromancer's Robes";
            Hue = 0x497;
            Weight = 3.0;
        }

        public NecromancersRobes(Serial serial) : base(serial)
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

    public class SpiritTome : Item
    {
        [Constructable]
        public SpiritTome() : base(0x1C11)
        {
            Name = "Spirit Tome";
            Hue = 0x46E;
            Weight = 1.0;
        }

        public SpiritTome(Serial serial) : base(serial)
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

    public class SpectralLantern : Lantern
    {
        [Constructable]
        public SpectralLantern() : base()
        {
            Name = "Spectral Lantern";
            Hue = 0x47E;
            Weight = 2.0;
        }

        public SpectralLantern(Serial serial) : base(serial)
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

    public class EtherealAltar : Item
    {
        [Constructable]
        public EtherealAltar() : base(0x3A99)
        {
            Name = "Ethereal Altar";
            Hue = 0x482;
            Weight = 50.0;
        }

        public EtherealAltar(Serial serial) : base(serial)
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
