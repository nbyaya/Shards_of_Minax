using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class CrystalBall : BaseJewel
    {
        [Constructable]
        public CrystalBall() : base(0xE2E, Layer.Neck)
        {
            Name = "Crystal Ball";
            Hue = 0x47E;
            Weight = 2.0;
        }

        public CrystalBall(Serial serial) : base(serial)
        {
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add(1070722, "Mysticism Bonus\t+10");
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);
            if (parent is Mobile)
            {
                Mobile m = (Mobile)parent;
            }
        }

        public override void OnRemoved(object parent)
        {
            base.OnRemoved(parent);
            if (parent is Mobile)
            {
                Mobile m = (Mobile)parent;
            }
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

    public class SeersRobes : BaseArmor
    {
        [Constructable]
        public SeersRobes() : base(0x1F03)
        {
            Name = "Seer's Robes";
            Hue = 0x47E;
            Weight = 3.0;
            Layer = Layer.OuterTorso;
        }

        public SeersRobes(Serial serial) : base(serial)
        {
        }

        public override int BasePhysicalResistance { get { return 5; } }
        public override int BaseFireResistance { get { return 5; } }
        public override int BaseColdResistance { get { return 5; } }
        public override int BasePoisonResistance { get { return 5; } }
        public override int BaseEnergyResistance { get { return 5; } }

        public override int InitMinHits { get { return 50; } }
        public override int InitMaxHits { get { return 60; } }

        public override int AosStrReq { get { return 20; } }
        public override int OldStrReq { get { return 20; } }

        public override int ArmorBase { get { return 13; } }

        public override ArmorMaterialType MaterialType { get { return ArmorMaterialType.Cloth; } }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add(1070722, "Magical Effectiveness\t+10%");
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

    public class ProphecyScroll : SpellScroll
    {
        [Constructable]
        public ProphecyScroll() : this(1)
        {
        }

        [Constructable]
        public ProphecyScroll(int amount) : base(1, 0x1F56, amount)
        {
            Name = "Prophecy Scroll";
            Hue = 0x490;
        }

        public ProphecyScroll(Serial serial) : base(serial)
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

    public class MysticShieldScroll : SpellScroll
    {
        [Constructable]
        public MysticShieldScroll() : this(1)
        {
        }

        [Constructable]
        public MysticShieldScroll(int amount) : base(2, 0x1F56, amount)
        {
            Name = "Mystic Shield Scroll";
            Hue = 0x491;
        }

        public MysticShieldScroll(Serial serial) : base(serial)
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

    public class MysticTome : BaseBook
    {
        [Constructable]
        public MysticTome() : base(0xFEF)
        {
            Name = "Mystic Tome";
            Hue = 0x530;
            Weight = 1.0;
        }

        public MysticTome(Serial serial) : base(serial)
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

    public class PropheticStaff : BaseStaff
    {
        [Constructable]
        public PropheticStaff() : base(0xE89)
        {
            Name = "Prophetic Staff";
            Hue = 0x481;
            Weight = 4.0;
        }

        public PropheticStaff(Serial serial) : base(serial)
        {
        }

        public override WeaponAbility PrimaryAbility { get { return WeaponAbility.ConcussionBlow; } }
        public override WeaponAbility SecondaryAbility { get { return WeaponAbility.ForceOfNature; } }

        public override int AosStrengthReq { get { return 20; } }
        public override int AosMinDamage { get { return 15; } }
        public override int AosMaxDamage { get { return 18; } }
        public override int AosSpeed { get { return 33; } }

        public override int OldStrengthReq { get { return 20; } }
        public override int OldMinDamage { get { return 15; } }
        public override int OldMaxDamage { get { return 18; } }
        public override int OldSpeed { get { return 33; } }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add(1070722, "Mysticism Bonus\t+5");
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);
            if (parent is Mobile)
            {
                Mobile m = (Mobile)parent;
            }
        }

        public override void OnRemoved(object parent)
        {
            base.OnRemoved(parent);
            if (parent is Mobile)
            {
                Mobile m = (Mobile)parent;
            }
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