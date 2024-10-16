using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
    public class WeaponSpeedAugmentCrystal : Item
    {
        [Constructable]
        public WeaponSpeedAugmentCrystal() : base(0x1F1C)  // Change the graphic if desired
        {
            Name = "Weapon Speed Augment Crystal";
            Hue = 1154; // Random hue for differentiation, can be changed to your liking
        }

        public WeaponSpeedAugmentCrystal(Serial serial) : base(serial)
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
                from.SendMessage("The crystal needs to be in your pack.");
                return;
            }

            from.SendMessage("Which weapon would you like to augment for Mana Leech?");
            from.Target = new ManaLeechAugmentTarget(this);
        }

        private class ManaLeechAugmentTarget : Target
        {
            private WeaponSpeedAugmentCrystal m_Crystal;

            public ManaLeechAugmentTarget(WeaponSpeedAugmentCrystal crystal) : base(1, false, TargetFlags.None)
            {
                m_Crystal = crystal;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (m_Crystal.Deleted || m_Crystal.RootParent != from)
                    return;

                int bonus = Utility.Random(1, 10);  // Generates a random number between 1 and 10

                if (targeted is BaseWeapon)
                {
                    BaseWeapon weapon = targeted as BaseWeapon;
                    weapon.Attributes.WeaponSpeed += bonus;
                    from.SendMessage(String.Format("The weapon has been augmented with a +{0} Hit Mana Leech.", bonus));
                    m_Crystal.Delete();
                }
                else
                {
                    from.SendMessage("You can't augment that for Mana Leech.");
                }
            }
        }
    }
}
