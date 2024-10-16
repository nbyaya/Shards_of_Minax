using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlEruption : XmlAttachment
    {
        private DateTime m_NextEruption; // Changed from TimeSpan to DateTime
        private int m_Count = 3; // Number of eruptions to spawn
        private TimeSpan m_MinDelay = TimeSpan.FromSeconds(120); // Minimum delay before next eruption
        private TimeSpan m_MaxDelay = TimeSpan.FromSeconds(180); // Maximum delay before next eruption

        [CommandProperty(AccessLevel.GameMaster)]
        public int Count { get { return m_Count; } set { m_Count = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan MinDelay { get { return m_MinDelay; } set { m_MinDelay = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan MaxDelay { get { return m_MaxDelay; } set { m_MaxDelay = value; } }

        public XmlEruption(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlEruption() { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Count);
            writer.Write(m_MinDelay);
            writer.Write(m_MaxDelay);
            writer.Write(m_NextEruption); // Write DateTime directly
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_Count = reader.ReadInt();
                m_MinDelay = reader.ReadTimeSpan();
                m_MaxDelay = reader.ReadTimeSpan();
                m_NextEruption = reader.ReadDateTime(); // Read DateTime directly
            }
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.UtcNow >= m_NextEruption)
            {
                TriggerEruption(attacker);
                Random rand = new Random();
                m_NextEruption = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next((int)m_MinDelay.TotalSeconds, (int)m_MaxDelay.TotalSeconds));
            }
        }

        private void TriggerEruption(Mobile owner)
        {
            if (owner == null || owner.Map == null)
                return;

            owner.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* A volcanic eruption occurs! *");

            for (int i = 0; i < m_Count; i++)
            {
                Point3D loc = GetSpawnPosition(5, owner);
                if (loc != Point3D.Zero)
                {
                    HotLavaTile fireball = new HotLavaTile();
                    fireball.MoveToWorld(loc, owner.Map);
                }
            }
        }

        private Point3D GetSpawnPosition(int range, Mobile owner)
        {
            int x = owner.X + Utility.RandomMinMax(-range, range);
            int y = owner.Y + Utility.RandomMinMax(-range, range);
            int z = owner.Z; // Keep the same height as the owner
            return new Point3D(x, y, z);
        }
    }
}
