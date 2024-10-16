// TamerGloves.cs
using System;
using Server;

namespace Server.Items
{
    public class TamerGloves : BaseClothing
    {
        [Constructable]
        public TamerGloves() : base(0x13C6)
        {
            Name = "Tamer Gloves";
            Hue = 1150;
            Attributes.BonusDex = 5;
            Attributes.BonusInt = 5;
            SkillBonuses.SetValues(0, SkillName.AnimalTaming, 10.0);
        }

        public TamerGloves(Serial serial) : base(serial)
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


namespace Server.Items
{
    public class VeterinaryTools : Item
    {
        [Constructable]
        public VeterinaryTools() : base(0x1EB8)
        {
            Name = "Veterinary Tools";
            Hue = 0x48D;
            Weight = 1.0;
        }

        public VeterinaryTools(Serial serial) : base(serial)
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


namespace Server.Items
{
    public class StuffedCrocodile : Item
    {
        [Constructable]
        public StuffedCrocodile() : base(0x25D9)
        {
            Name = "Stuffed Crocodile";
            Hue = 0;
            Weight = 10.0;
        }

        public StuffedCrocodile(Serial serial) : base(serial)
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


namespace Server.Items
{
    public class AnimalTamingBook : Item
    {
        [Constructable]
        public AnimalTamingBook() : base(0xFF2)
        {
            Name = "Animal Taming Book";
            Hue = 0x47E;
        }

        public AnimalTamingBook(Serial serial) : base(serial)
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
