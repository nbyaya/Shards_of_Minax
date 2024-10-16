using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class FocusScroll : Item
    {
        private SkillName m_Skill;
        private double m_Value;

        [Constructable]
        public FocusScroll()
            : base(0x1F4C) // Item ID for a scroll
        {
            Name = "Focus Scroll";
            Hue = 0x47E; // Item color (optional)
            Weight = 1.0;

            m_Skill = SkillName.Focus;
            m_Value = 200.0;
        }

        public FocusScroll(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version

            writer.Write((int)m_Skill);
            writer.Write((double)m_Value);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_Skill = (SkillName)reader.ReadInt();
            m_Value = reader.ReadDouble();
        }
    }
}
