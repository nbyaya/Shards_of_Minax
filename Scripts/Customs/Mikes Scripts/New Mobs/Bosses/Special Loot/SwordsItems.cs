using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class DualKatanas : BaseSword
    {
        public override int LabelNumber { get { return 1073540; } } // Dual Katanas

        [Constructable]
        public DualKatanas()
            : base(0x27A5)
        {
            Weight = 6.0;
            Layer = Layer.TwoHanded;
            Hue = 0x66D;
            WeaponAttributes.HitHarm = 50;
            WeaponAttributes.HitLowerDefend = 50;
            Attributes.WeaponDamage = 60;
            Attributes.WeaponSpeed = 30;
            Attributes.DefendChance = 15;
        }

        public DualKatanas(Serial serial) : base(serial)
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

    public class SamuraiArmor : BaseClothing
    {
        public override int LabelNumber { get { return 1073539; } } // Samurai Armor

        [Constructable]
        public SamuraiArmor()
            : base(0x2776)
        {
            Weight = 10.0;
            Hue = 0x66D;
        }

        public SamuraiArmor(Serial serial) : base(serial)
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

    public class SamuraiSash : BaseWaist
    {
        public override int LabelNumber { get { return 1073541; } } // Samurai Sash

        [Constructable]
        public SamuraiSash()
            : base(0x27D7)
        {
            Weight = 1.0;
            Hue = 0x66D;
            Attributes.RegenStam = 5;
            Attributes.RegenMana = 5;
            Attributes.LowerManaCost = 10;
        }

        public SamuraiSash(Serial serial) : base(serial)
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

    public class WarriorTome : Item
    {
        public override int LabelNumber { get { return 1073542; } } // Warrior's Tome

        [Constructable]
        public WarriorTome()
            : base(0x22C5)
        {
            Weight = 1.0;
            Hue = 0x66D;
            LootType = LootType.Blessed;
        }

        public WarriorTome(Serial serial) : base(serial)
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

    public class SamuraiStatue : Item
    {
        public override int LabelNumber { get { return 1073543; } } // Samurai Statue

        [Constructable]
        public SamuraiStatue()
            : base(0x20F5)
        {
            Weight = 10.0;
            Hue = 0x66D;
        }

        public SamuraiStatue(Serial serial) : base(serial)
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

    public class SwordDisplay : Item
    {
        public override int LabelNumber { get { return 1073544; } } // Sword Display

        [Constructable]
        public SwordDisplay()
            : base(0x3A96)
        {
            Weight = 5.0;
            Hue = 0x66D;
        }

        public SwordDisplay(Serial serial) : base(serial)
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
