using System;
using Server;
using Server.Mobiles;
using Server.Network; // Add this for MessageType
using Server.Items; // Add this for EffectItem

namespace Server.Engines.XmlSpawner2
{
    public class XmlAirborneEscape : XmlAttachment
    {
        private TimeSpan m_Refractory = TimeSpan.FromMinutes(1); // Time between activations
        private DateTime m_EndTime; // Declare m_EndTime

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Refractory { get { return m_Refractory; } set { m_Refractory = value; } }

        public XmlAirborneEscape(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlAirborneEscape() { }

        [Attachable]
        public XmlAirborneEscape(double refractory)
        {
            Refractory = TimeSpan.FromMinutes(refractory);
        }

        public void ExecuteAirborneEscape(Mobile owner)
        {
            if (DateTime.Now >= m_EndTime && owner.Combatant != null)
            {
                Point3D newLocation = GetSpawnPosition(10, owner);
                if (newLocation != Point3D.Zero)
                {
                    owner.Location = newLocation;
                    owner.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Evades with a swift maneuver! *");
                    Effects.SendLocationParticles(EffectItem.Create(owner.Location, owner.Map, EffectItem.DefaultDuration), 0x3709, 10, 30, 1153);
                    m_EndTime = DateTime.Now + m_Refractory; // Recalculate next activation
                }
            }
        }

        private Point3D GetSpawnPosition(int range, Mobile owner)
        {
            for (int i = 0; i < 10; i++)
            {
                int x = owner.X + Utility.RandomMinMax(-range, range);
                int y = owner.Y + Utility.RandomMinMax(-range, range);
                int z = owner.Map.GetAverageZ(x, y);

                Point3D p = new Point3D(x, y, z);

                if (owner.Map.CanSpawnMobile(p))
                    return p;
            }

            return Point3D.Zero;
        }
    }
}
