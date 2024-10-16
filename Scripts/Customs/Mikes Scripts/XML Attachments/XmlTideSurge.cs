using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlTideSurge : XmlAttachment
    {
        private int m_Damage = 5; // Default damage for the Tide Surge
        private TimeSpan m_Cooldown = TimeSpan.FromSeconds(30); // Cooldown time
        private DateTime m_NextTideSurge;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Damage { get { return m_Damage; } set { m_Damage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Cooldown { get { return m_Cooldown; } set { m_Cooldown = value; } }

        public XmlTideSurge(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlTideSurge() { }

        [Attachable]
        public XmlTideSurge(int damage, double cooldown)
        {
            Damage = damage;
            Cooldown = TimeSpan.FromSeconds(cooldown);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Damage);
            writer.Write(m_Cooldown);
            writer.WriteDeltaTime(m_NextTideSurge);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_Damage = reader.ReadInt();
                m_Cooldown = reader.ReadTimeSpan();
                m_NextTideSurge = reader.ReadDeltaTime();
            }
        }

        public override string OnIdentify(Mobile from)
        {
            return String.Format("Tide Surge: Deals {0} damage with a cooldown of {1} seconds.", m_Damage, m_Cooldown.TotalSeconds);
        }

        public void PerformTideSurge(Mobile owner)
        {
            if (owner == null || DateTime.UtcNow < m_NextTideSurge || owner.Map == null)
                return;

            foreach (Mobile m in owner.GetMobilesInRange(5))
            {
                if (m != owner && m.Player)
                {
                    m.SendMessage("A surge of water crashes over you, pushing you away!");
                    m.Damage(m_Damage, owner);
                    m.Location = new Point3D(m.X + Utility.RandomMinMax(-2, 2), m.Y + Utility.RandomMinMax(-2, 2), m.Z);
                }
            }

            m_NextTideSurge = DateTime.UtcNow + m_Cooldown; // Reset cooldown
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            PerformTideSurge(attacker);
        }
    }
}
