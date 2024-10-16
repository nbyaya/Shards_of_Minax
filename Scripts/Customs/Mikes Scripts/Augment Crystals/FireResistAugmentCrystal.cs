using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
    public class FireResistAugmentCrystal : Item
    {
        [Constructable]
        public FireResistAugmentCrystal() : base(0x1F1C)  // You can change the graphic if desired.
        {
            Name = "Fire Resist Augment Crystal";
            Hue = 33; // Random hue for differentiation, can be changed to your liking.
        }

        public FireResistAugmentCrystal(Serial serial) : base(serial)
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

            from.SendMessage("Which armor piece would you like to augment for Fire Resist?");
            from.Target = new FireResistAugmentTarget(this);
        }

        private class FireResistAugmentTarget : Target
        {
            private FireResistAugmentCrystal m_Crystal;

            public FireResistAugmentTarget(FireResistAugmentCrystal crystal) : base(1, false, TargetFlags.None)
            {
                m_Crystal = crystal;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (m_Crystal.Deleted || m_Crystal.RootParent != from)
                    return;

                int bonus = Utility.Random(1, 5);  // Generates a random number between 1 and 5.

                if (targeted is BaseArmor)
                {
                    BaseArmor armor = targeted as BaseArmor;
                    armor.FireBonus += bonus; // Setting the random bonus.
                    from.SendMessage(String.Format("The armor has been augmented with a +{0} Fire Resist.", bonus));
                    m_Crystal.Delete();
                }
                else
                {
                    from.SendMessage("You can't augment that with Fire Resist.");
                }
            }
        }
    }
}
