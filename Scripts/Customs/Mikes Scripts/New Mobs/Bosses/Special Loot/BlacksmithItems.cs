using System;
using Server;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
    public class ForgeHammer : Item
    {
        private int m_UsesRemaining;

        [CommandProperty(AccessLevel.GameMaster)]
        public int UsesRemaining
        {
            get { return m_UsesRemaining; }
            set { m_UsesRemaining = value; InvalidateProperties(); }
        }

        [Constructable]
        public ForgeHammer() : base(0x13E3)
        {
            Weight = 8.0;
            Name = "Forge Hammer";
            Hue = 1161; // A fiery red hue
            m_UsesRemaining = 250;
        }

        public ForgeHammer(Serial serial) : base(serial)
        {
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            list.Add(1060584, m_UsesRemaining.ToString()); // uses remaining: ~1_val~
            list.Add(1060636); // exceptional
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                return;
            }

            from.SendLocalizedMessage(1010018); // What do you want to use this on?
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
            writer.Write((int)m_UsesRemaining);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            m_UsesRemaining = reader.ReadInt();
        }
    }

    public class ShieldOfTheForge : BaseShield
    {
        public override int BasePhysicalResistance { get { return 8; } }
        public override int BaseFireResistance { get { return 15; } }
        public override int BaseColdResistance { get { return 3; } }
        public override int BasePoisonResistance { get { return 3; } }
        public override int BaseEnergyResistance { get { return 3; } }

        public override int InitMinHits { get { return 255; } }
        public override int InitMaxHits { get { return 255; } }

        public override int AosStrReq { get { return 90; } }

        public override int ArmorBase { get { return 30; } }

        [Constructable]
        public ShieldOfTheForge() : base(0x1B7B)
        {
            Weight = 8.0;
            Name = "Shield of the Forge";
            Hue = 1161; // A fiery red hue

            Attributes.DefendChance = 15;
            Attributes.BonusStr = 10;
            Attributes.RegenHits = 3;
            Attributes.WeaponDamage = 20;

            FireBonus = 15;
        }

        public ShieldOfTheForge(Serial serial) : base(serial)
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