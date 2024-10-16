using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
    public class LumberjackingAugmentCrystal : Item
    {
        [Constructable]
        public LumberjackingAugmentCrystal() : base(0x1F1D)  // Change the item graphic.
        {
            Name = "Lumberjacking Augment Crystal";
            Hue = 1281; // Change hue for differentiation.
        }

        public LumberjackingAugmentCrystal(Serial serial) : base(serial)
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

            from.SendMessage("Which armor or clothing item would you like to augment for lumberjacking?");
            from.Target = new LumberjackingAugmentTarget(this);
        }

        private class LumberjackingAugmentTarget : Target
        {
            private LumberjackingAugmentCrystal m_Crystal;

            public LumberjackingAugmentTarget(LumberjackingAugmentCrystal crystal) : base(1, false, TargetFlags.None)
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
                    armor.SkillBonuses.SetValues(0, SkillName.Lumberjacking, bonus); // Setting the random bonus.
                    from.SendMessage(String.Format("The armor has been augmented with a +{0} lumberjacking bonus.", bonus));
                    m_Crystal.Delete();
                }
                else if (targeted is BaseClothing)
                {
                    BaseClothing clothing = targeted as BaseClothing;
                    clothing.SkillBonuses.SetValues(0, SkillName.Lumberjacking, bonus); // Setting the random bonus.
                    from.SendMessage(String.Format("The clothing has been augmented with a +{0} lumberjacking bonus.", bonus));
                    m_Crystal.Delete();
                }
                else
                {
                    from.SendMessage("You can't augment that for lumberjacking.");
                }
            }
        }
    }
}
