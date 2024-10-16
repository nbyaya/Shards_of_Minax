using System;
using Server;
using Server.Mobiles;
using Server.Network; // Add this for MessageType

namespace Server.Engines.XmlSpawner2
{
    public class XmlSilentGale : XmlAttachment
    {
        private TimeSpan m_Refractory = TimeSpan.FromSeconds(30); // Time between activations
        private DateTime m_EndTime; // Declare m_EndTime

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Refractory { get { return m_Refractory; } set { m_Refractory = value; } }

        public XmlSilentGale(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlSilentGale() { }

        [Attachable]
        public XmlSilentGale(double refractory)
        {
            Refractory = TimeSpan.FromSeconds(refractory);
        }

        public void ExecuteSilentGale(Mobile owner)
        {
            if (DateTime.Now >= m_EndTime)
            {
                owner.Skills[SkillName.Stealth].Base = 100.0; // Adjust as needed
                owner.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Moves silently through the air! *");
                m_EndTime = DateTime.Now + m_Refractory; // Recalculate next activation
            }
        }
    }
}
