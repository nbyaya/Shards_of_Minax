using System;
using Server;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Engines.XmlSpawner2
{
    public class XmlInvisibility : XmlAttachment
    {
        private TimeSpan m_Duration = TimeSpan.FromSeconds(10); // Default duration for invisibility
        private TimeSpan m_Refractory = TimeSpan.FromSeconds(60); // Default cooldown between uses
        private DateTime m_InvisibilityEnd;
        private DateTime m_NextInvisibility;

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Duration { get { return m_Duration; } set { m_Duration = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Refractory { get { return m_Refractory; } set { m_Refractory = value; } }

        public XmlInvisibility(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlInvisibility() { }

        [Attachable]
        public XmlInvisibility(double duration, double refractory)
        {
            Duration = TimeSpan.FromSeconds(duration);
            Refractory = TimeSpan.FromSeconds(refractory);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Duration);
            writer.Write(m_Refractory);
            writer.WriteDeltaTime(m_InvisibilityEnd);
            writer.WriteDeltaTime(m_NextInvisibility);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Duration = reader.ReadTimeSpan();
            m_Refractory = reader.ReadTimeSpan();
            m_InvisibilityEnd = reader.ReadDeltaTime();
            m_NextInvisibility = reader.ReadDeltaTime();
        }

        public void CheckInvisibility(Mobile owner)
        {
            if (DateTime.UtcNow >= m_NextInvisibility && !owner.Hidden)
            {
                DoInvisibility(owner);
                m_NextInvisibility = DateTime.UtcNow + Refractory;
            }

            if (DateTime.UtcNow >= m_InvisibilityEnd && owner.Hidden)
            {
                owner.Hidden = false;
                owner.PublicOverheadMessage(Network.MessageType.Regular, 0x3B2, false, "* Reappears *");
            }
        }

        private void DoInvisibility(Mobile owner)
        {
            if (owner == null || owner.Hidden || owner.Map == null)
                return;

            owner.PublicOverheadMessage(Network.MessageType.Regular, 0x3B2, false, "* Vanishes *");
            owner.PlaySound(0x22F);
            owner.Hidden = true;

            m_InvisibilityEnd = DateTime.UtcNow + Duration;
        }

        public override string OnIdentify(Mobile from)
        {
            return $"Grants invisibility for {Duration.TotalSeconds} seconds every {Refractory.TotalSeconds} seconds.";
        }
    }
}
