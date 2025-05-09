using System;
using Server;
using Server.Targeting;
using Server.Items;

namespace Server.Items
{
    public class MasterWeaponOil : Item
    {
        [Constructable]
        public MasterWeaponOil() : base(0x0E26)
        {
            Name = "Master Weapon Oil";
            Hue = 0; // Adjust the color as needed
            Weight = 1.0;
        }

        public MasterWeaponOil(Serial serial) : base(serial)
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

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendMessage("This must be in your backpack to use.");
                return;
            }

            from.SendMessage("Which weapon do you wish to apply this oil to?");
            from.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private MasterWeaponOil m_Oil;

            public InternalTarget(MasterWeaponOil oil) : base(1, false, TargetFlags.None)
            {
                m_Oil = oil;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (m_Oil.Deleted || m_Oil.RootParent != from)
                    return;

                if (targeted is BaseWeapon)
                {
                    BaseWeapon weapon = (BaseWeapon)targeted;

                    if (!weapon.IsChildOf(from.Backpack))
                    {
                        from.SendMessage("The weapon must be in your backpack.");
                        return;
                    }

                    // Check to ensure we do not exceed the weapon's max damage
                    if (weapon.MinDamage < weapon.MaxDamage)
                    {
                        weapon.MinDamage += 1;
                        from.SendMessage("You apply the master oil to the weapon, enhancing its precision.");
                        m_Oil.Consume(); // Remove the oil from the player's inventory
                    }
                    else
                    {
                        from.SendMessage("This weapon cannot become any more powerful.");
                    }
                }
                else
                {
                    from.SendMessage("That is not a weapon.");
                }
            }
        }
    }
}
