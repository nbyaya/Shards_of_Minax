using System;
using Server;

namespace Server.Items
{
    public class StealthCloak : BaseCloak
    {
        [Constructable]
        public StealthCloak() : base(0x1515)
        {
            Name = "Cloak of Stealth";
            Hue = 0x455;
            Attributes.BonusDex = 5;
            Attributes.NightSight = 1;
            Attributes.RegenStam = 2;
        }

        public StealthCloak(Serial serial) : base(serial)
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

    public class ShadowStatue : Item
    {
        [Constructable]
        public ShadowStatue() : base(0x20F6)
        {
            Name = "Shadow Statue";
            Hue = 0x455;
            Weight = 5.0;
        }

        public ShadowStatue(Serial serial) : base(serial)
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

    public class StealthDagger : BaseKnife
    {
        [Constructable]
        public StealthDagger() : base(0xF52)
        {
            Name = "Dagger of Stealth";
            Hue = 0x455;
            Attributes.WeaponSpeed = 30;
            Attributes.WeaponDamage = 40;
            Attributes.AttackChance = 10;
            Attributes.DefendChance = 5;
            WeaponAttributes.HitLowerDefend = 20;
            WeaponAttributes.HitMagicArrow = 15;
        }

        public StealthDagger(Serial serial) : base(serial)
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
