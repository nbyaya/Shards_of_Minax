using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
    public class ThrowingAugmentCrystal : Item
    {
        [Constructable]
        public ThrowingAugmentCrystal() : base(0x1F1C) // Random item graphic, you can change this.
        {
            Name = "Throwing Augment Crystal";
            Hue = 1509; // Random hue for differentiation, can be changed to your liking.
        }

        public ThrowingAugmentCrystal(Serial serial) : base(serial)
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

            from.SendMessage("Which throwing weapon would you like to augment?");
            from.Target = new ThrowingAugmentTarget(this);
        }

        private class ThrowingAugmentTarget : Target
        {
            private ThrowingAugmentCrystal m_Crystal;

            public ThrowingAugmentTarget(ThrowingAugmentCrystal crystal) : base(1, false, TargetFlags.None)
            {
                m_Crystal = crystal;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (m_Crystal.Deleted || m_Crystal.RootParent != from)
                    return;

                int bonus = Utility.Random(1, 25); // Generates a random number between 1 and 25.

                if (targeted is BaseWeapon && ((BaseWeapon)targeted).Skill == SkillName.Throwing)
                {
                    BaseWeapon weapon = targeted as BaseWeapon;
                    weapon.Attributes.WeaponSpeed = bonus; // Setting the random bonus as weapon speed.
                    from.SendMessage(String.Format("The weapon has been augmented with a {0} bonus to weapon speed.", bonus));
                    m_Crystal.Delete();
                }
                else
                {
                    from.SendMessage("You can't augment that with a throwing bonus.");
                }
            }
        }
    }
}
