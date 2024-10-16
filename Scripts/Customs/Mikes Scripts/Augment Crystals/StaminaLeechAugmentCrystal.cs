using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
    public class StaminaLeechAugmentCrystal : Item
    {
        [Constructable]
        public StaminaLeechAugmentCrystal() : base(0x1F1C)  // You can change the graphic if desired.
        {
            Name = "Stamina Leech Augment Crystal";
            Hue = 1154; // Different hue for differentiation, can be changed to your liking.
        }

        public StaminaLeechAugmentCrystal(Serial serial) : base(serial)
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

            from.SendMessage("Which weapon would you like to augment for Stamina Leech?");
            from.Target = new StaminaLeechAugmentTarget(this);
        }

        private class StaminaLeechAugmentTarget : Target
        {
            private StaminaLeechAugmentCrystal m_Crystal;

            public StaminaLeechAugmentTarget(StaminaLeechAugmentCrystal crystal) : base(1, false, TargetFlags.None)
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
                    weapon.WeaponAttributes.HitLeechStam += bonus; // Setting the random bonus.
                    from.SendMessage(String.Format("The weapon has been augmented with +{0} Hit Stamina Leech.", bonus));
                    m_Crystal.Delete();
                }
                else
                {
                    from.SendMessage("You can't augment that with Stamina Leech.");
                }
            }
        }
    }
}
