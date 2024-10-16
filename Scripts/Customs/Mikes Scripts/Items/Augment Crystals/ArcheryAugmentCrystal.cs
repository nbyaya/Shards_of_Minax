using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
    public class ArcheryAugmentCrystal : Item
    {
        [Constructable]
        public ArcheryAugmentCrystal() : base(0x1F1D)  // Random item graphic, you can change this.
        {
            Name = "Archery Augment Crystal";
            Hue = 1177; // Random hue for differentiation, can be changed to your liking.
        }

        public ArcheryAugmentCrystal(Serial serial) : base(serial)
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

            from.SendMessage("Which armor or clothing item would you like to augment for archery?");
            from.Target = new ArcheryAugmentTarget(this);
        }

        private class ArcheryAugmentTarget : Target
        {
            private ArcheryAugmentCrystal m_Crystal;

            public ArcheryAugmentTarget(ArcheryAugmentCrystal crystal) : base(1, false, TargetFlags.None)
            {
                m_Crystal = crystal;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (m_Crystal.Deleted || m_Crystal.RootParent != from)
                    return;

                int bonus = Utility.Random(1, 25);  // Generates a random number between 1 and 25.

                if (targeted is BaseArmor)
                {
                    BaseArmor armor = targeted as BaseArmor;
                    armor.SkillBonuses.SetValues(0, SkillName.Archery, bonus); // Setting the random bonus.
                    from.SendMessage(String.Format("The armor has been augmented with a +{0} archery bonus.", bonus));
                    m_Crystal.Delete();
                }
                else if (targeted is BaseClothing)
                {
                    BaseClothing clothing = targeted as BaseClothing;
                    clothing.SkillBonuses.SetValues(0, SkillName.Archery, bonus); // Setting the random bonus.
                    from.SendMessage(String.Format("The clothing has been augmented with a +{0} archery bonus.", bonus));
                    m_Crystal.Delete();
                }
                else
                {
                    from.SendMessage("You can't augment that for archery.");
                }
            }
        }
    }
}
