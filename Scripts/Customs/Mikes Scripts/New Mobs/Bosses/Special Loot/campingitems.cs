using System;
using Server;

namespace Server.Items
{
    public class BearGryllsJournal : Item
    {
        [Constructable]
        public BearGryllsJournal()
            : base(0x2252) // Book item ID
        {
            Name = "Bear Grylls' Journal";
            Hue = 0x47E;
            Weight = 1.0;
        }

        public BearGryllsJournal(Serial serial)
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

    public class SurvivalKnife : Item
    {
        [Constructable]
        public SurvivalKnife()
            : base(0x13F6) // Knife item ID
        {
            Name = "Survival Knife";
            Hue = 0x47E;
            Weight = 2.0;
        }

        public SurvivalKnife(Serial serial)
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

    public class SurvivalBackpack : Item
    {
        [Constructable]
        public SurvivalBackpack()
            : base(0x9B2) // Backpack item ID
        {
            Name = "Survival Backpack";
            Hue = 0x47E;
            Weight = 5.0;
        }

        public SurvivalBackpack(Serial serial)
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
