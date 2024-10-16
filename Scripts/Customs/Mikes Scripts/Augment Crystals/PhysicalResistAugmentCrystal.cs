using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
    public class PhysicalResistAugmentCrystal : Item
    {
        [Constructable]
        public PhysicalResistAugmentCrystal() : base(0x1F1C)  // You can change the graphic if desired.
        {
            Name = "Physical Resist Augment Crystal";
            Hue = 1155; // Random hue for differentiation, can be changed to your liking.
        }

        public PhysicalResistAugmentCrystal(Serial serial) : base(serial)
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

            from.SendMessage("Which armor piece would you like to augment for Physical Resist?");
            from.Target = new PhysicalResistAugmentTarget(this);
        }

        private class PhysicalResistAugmentTarget : Target
        {
            private PhysicalResistAugmentCrystal m_Crystal;

            public PhysicalResistAugmentTarget(PhysicalResistAugmentCrystal crystal) : base(1, false, TargetFlags.None)
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
                    armor.PhysicalBonus += bonus; // Setting the random bonus.
                    from.SendMessage(String.Format("The armor has been augmented with a +{0} Physical Resist.", bonus));
                    m_Crystal.Delete();
                }
                else
                {
                    from.SendMessage("You can't augment that with Physical Resist.");
                }
            }
        }
    }
}
