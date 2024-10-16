using System;
using Server.Network;

namespace Server.Items
{
    public class MysticalAlembic : Item
    {
        private int m_UsesRemaining;

        [CommandProperty(AccessLevel.GameMaster)]
        public int UsesRemaining
        {
            get { return m_UsesRemaining; }
            set { m_UsesRemaining = value; InvalidateProperties(); }
        }

        [Constructable]
        public MysticalAlembic() : base(0x1810)
        {
            Weight = 1.0;
            Hue = 0x2D;
            Name = "Mystical Alembic";
            m_UsesRemaining = 10;
        }

        public MysticalAlembic(Serial serial) : base(serial)
        {
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            list.Add(1060584, m_UsesRemaining.ToString()); // uses remaining: ~1_val~
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                return;
            }

            if (m_UsesRemaining > 0)
            {
                from.SendMessage("The Mystical Alembic enhances your next alchemical creation.");
                from.AddSkillMod(new TimedSkillMod(SkillName.Alchemy, true, 20.0, TimeSpan.FromMinutes(5)));
                from.PlaySound(0x240);
                m_UsesRemaining--;
                InvalidateProperties();
            }
            else
            {
                from.SendMessage("The Mystical Alembic has been depleted of its power.");
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write((int)m_UsesRemaining);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_UsesRemaining = reader.ReadInt();
        }
    }
}