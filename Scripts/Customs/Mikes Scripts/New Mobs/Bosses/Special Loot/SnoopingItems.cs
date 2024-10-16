using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class ThiefsGloves : BaseClothing
    {
        [Constructable]
        public ThiefsGloves() : base(0x13C6)
        {
            Name = "Thief's Gloves";
            Hue = 0x455;
            Attributes.BonusDex = 5;
            SkillBonuses.SetValues(0, SkillName.Snooping, 10.0);
        }

        public ThiefsGloves(Serial serial) : base(serial)
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

    public class SnoopingTome : Item
    {
        [Constructable]
        public SnoopingTome() : base(0x1C11)
        {
            Name = "Tome of Snooping";
            Hue = 0x481;
        }

        public SnoopingTome(Serial serial) : base(serial)
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

    public class CarmenStatue : Item
    {
        [Constructable]
        public CarmenStatue() : base(0x20E)
        {
            Name = "Statue of Carmen Sandiego";
            Hue = 0x47E;
            Weight = 5.0;
        }

        public CarmenStatue(Serial serial) : base(serial)
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

    public class InvisibleInk : Item
    {
        [Constructable]
        public InvisibleInk() : base(0xF3F)
        {
            Name = "Invisible Ink";
            Hue = 0x3E9;
        }

        public InvisibleInk(Serial serial) : base(serial)
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
