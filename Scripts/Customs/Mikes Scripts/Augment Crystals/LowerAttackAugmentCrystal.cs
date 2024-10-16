using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
    public class LowerAttackAugmentCrystal : Item
    {
        [Constructable]
        public LowerAttackAugmentCrystal() : base(0x1F1C)  // You can change the graphic if desired.
        {
            Name = "Lower Attack Augment Crystal";
            Hue = 1154; // Different hue for differentiation, can be changed to your liking.
        }

        public LowerAttackAugmentCrystal(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Version.
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

            from.SendMessage("Which weapon would you like to augment for Lower Attack?");
            from.Target = new LowerAttackAugmentTarget(this);
        }

        private class LowerAttackAugmentTarget : Target
        {
            private LowerAttackAugmentCrystal m_Crystal;

            public LowerAttackAugmentTarget(LowerAttackAugmentCrystal crystal) : base(1, false, TargetFlags.None)
            {
                m_Crystal = crystal;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (m_Crystal.Deleted || m_Crystal.RootParent != from)
                    return;

                int bonus = Utility.Random(1, 10);  // Generates a random number between 1 and 10.

                if (targeted is BaseWeapon)
                {
                    BaseWeapon weapon = targeted as BaseWeapon;
                    weapon.WeaponAttributes.HitLowerAttack += bonus; // Setting the random bonus.
                    from.SendMessage(String.Format("The weapon has been augmented with a +{0} Hit Lower Attack.", bonus));
                    m_Crystal.Delete();
                }
                else
                {
                    from.SendMessage("You can't augment that with Lower Attack.");
                }
            }
        }
    }
}
