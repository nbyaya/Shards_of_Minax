using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
    public class LifeLeechAugmentCrystal : Item
    {
        [Constructable]
        public LifeLeechAugmentCrystal() : base(0x1F1C)  // You can change the graphic if desired.
        {
            Name = "Life Leech Augment Crystal";
            Hue = 1153; // Random hue for differentiation, can be changed to your liking.
        }

        public LifeLeechAugmentCrystal(Serial serial) : base(serial)
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

            from.SendMessage("Which weapon would you like to augment for Life Leech?");
            from.Target = new LifeLeechAugmentTarget(this);
        }

        private class LifeLeechAugmentTarget : Target
        {
            private LifeLeechAugmentCrystal m_Crystal;

            public LifeLeechAugmentTarget(LifeLeechAugmentCrystal crystal) : base(1, false, TargetFlags.None)
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
                    weapon.WeaponAttributes.HitLeechHits += bonus; // Setting the random bonus for Life Leech
                    from.SendMessage(String.Format("The weapon has been augmented with a +{0} Life Leech.", bonus));
                    m_Crystal.Delete();
                }
                else
                {
                    from.SendMessage("You can't augment that with Life Leech.");
                }
            }
        }
    }
}
