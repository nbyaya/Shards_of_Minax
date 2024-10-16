using System;
using Server;
using Server.Items;

namespace Server.Items
{
    // Chef Toque
    public class ChefToque : BaseHat
    {
        public override int BasePhysicalResistance { get { return 2; } }
        public override int BaseFireResistance { get { return 2; } }
        public override int BaseColdResistance { get { return 2; } }
        public override int BasePoisonResistance { get { return 2; } }
        public override int BaseEnergyResistance { get { return 2; } }

        [Constructable]
        public ChefToque()
            : base(0x1713)
        {
            Name = "Chef Toque";
            Weight = 1.0;
            Hue = 0x47E;
        }

        public ChefToque(Serial serial) : base(serial) { }

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

    // Master Chef's Knife
    public class MasterChefsKnife : BaseWeapon
    {
        public override WeaponAbility PrimaryAbility { get { return WeaponAbility.ParalyzingBlow; } }
        public override WeaponAbility SecondaryAbility { get { return WeaponAbility.InfectiousStrike; } }

        public override int AosStrengthReq { get { return 10; } }
        public override int AosMinDamage { get { return 15; } }
        public override int AosMaxDamage { get { return 20; } }
        public override int AosSpeed { get { return 45; } }

        public override int OldStrengthReq { get { return 10; } }
        public override int OldMinDamage { get { return 15; } }
        public override int OldMaxDamage { get { return 20; } }
        public override int OldSpeed { get { return 45; } }

        public override int InitMinHits { get { return 255; } }
        public override int InitMaxHits { get { return 255; } }

        [Constructable]
        public MasterChefsKnife()
            : base(0xEC4)
        {
            Name = "Master Chef's Knife";
            Weight = 2.0;
            Hue = 0x482;

            Attributes.SpellChanneling = 1;
            Attributes.WeaponDamage = 50;
            Attributes.WeaponSpeed = 30;
            SkillBonuses.SetValues(0, SkillName.TasteID, 15.0);
        }

        public MasterChefsKnife(Serial serial) : base(serial) { }

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

    // Flavor Extract
    public class FlavorExtract : Item
    {
        [Constructable]
        public FlavorExtract()
            : base(0x0F0B)
        {
            Name = "Flavor Extract";
            Weight = 1.0;
            Hue = 0x489;
        }

        public FlavorExtract(Serial serial) : base(serial) { }

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

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                return;
            }

            from.SendMessage("You feel a surge of flavor enhancing your abilities!");

            // Apply some buff or effect here
            // Example: Buff food-related skills or temporary stat boost
            from.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);
            from.PlaySound(0x1EA);
        }
    }
}
