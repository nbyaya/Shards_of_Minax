using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlTeleport : XmlAttachment
    {
        private TimeSpan m_Refractory = TimeSpan.FromSeconds(7); // Cooldown for the teleport ability
        private DateTime m_NextTeleportTime;

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Refractory { get { return m_Refractory; } set { m_Refractory = value; } }

        public XmlTeleport(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlTeleport() { }

        [Attachable]
        public XmlTeleport(double refractory)
        {
            Refractory = TimeSpan.FromMinutes(refractory);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Refractory);
            writer.WriteDeltaTime(m_NextTeleportTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_Refractory = reader.ReadTimeSpan();
                m_NextTeleportTime = reader.ReadDeltaTime();
            }
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_NextTeleportTime && defender != null)
            {
                PerformTeleport(attacker);
                m_NextTeleportTime = DateTime.UtcNow + m_Refractory;
            }
        }

        public void PerformTeleport(Mobile owner)
        {
            if (owner == null || owner.Map == null)
                return;

            Point3D newLocation = new Point3D(owner.X + Utility.RandomMinMax(-10, 10), owner.Y + Utility.RandomMinMax(-10, 10), owner.Z);
            if (owner.Map.CanSpawnMobile(newLocation))
            {
                owner.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Teleports with a burst *");
                Effects.SendLocationEffect(owner.Location, owner.Map, 0x376A, 10, 16); // Fixed effect for teleport

                owner.MoveToWorld(newLocation, owner.Map);

                foreach (Mobile m in owner.GetMobilesInRange(5))
                {
                    if (m != owner && m.Alive)
                    {
                        m.SendMessage("A creature disappears in a puff and reappears somewhere else!");
                    }
                }
            }
        }
    }
}
