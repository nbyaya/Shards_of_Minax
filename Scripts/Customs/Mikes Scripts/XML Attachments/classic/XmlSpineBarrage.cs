using System;
using System.Collections.Generic; // Required for List<>
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Engines.XmlSpawner2
{
    public class XmlSpineBarrage : XmlAttachment
    {
        private int m_Damage = 30; // Default damage for Spine Barrage
        private int m_Radius = 8; // Radius of the ability in tiles
        private TimeSpan m_Cooldown = TimeSpan.FromSeconds(10); // Cooldown between uses
        private DateTime m_LastUsed;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Damage { get { return m_Damage; } set { m_Damage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Radius { get { return m_Radius; } set { m_Radius = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Cooldown { get { return m_Cooldown; } set { m_Cooldown = value; } }

        public XmlSpineBarrage(ASerial serial) : base(serial) { }

        public XmlSpineBarrage() { }

        public XmlSpineBarrage(double cooldown, int damage, int radius)
        {
            Cooldown = TimeSpan.FromSeconds(cooldown);
            Damage = damage;
            Radius = radius;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Damage);
            writer.Write(m_Radius);
            writer.Write(m_Cooldown);
            writer.WriteDeltaTime(m_LastUsed);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            if (version >= 0)
            {
                m_Damage = reader.ReadInt();
                m_Radius = reader.ReadInt();
                Cooldown = reader.ReadTimeSpan();
                m_LastUsed = reader.ReadDeltaTime();
            }
        }

        public override string OnIdentify(Mobile from)
        {
            return String.Format("Spine Barrage: Fires projectiles dealing {0} damage within {1} tiles radius every {2} seconds.", m_Damage, m_Radius, m_Cooldown.TotalSeconds);
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.Now >= m_LastUsed + Cooldown)
            {
                PerformSpineBarrage(attacker);
				attacker.Say("Spine Barrage!");
                m_LastUsed = DateTime.Now;
            }
        }

        private void PerformSpineBarrage(Mobile owner)
        {
            if (owner == null || DateTime.Now < m_LastUsed + Cooldown || owner.Map == null)
                return;

            Map map = owner.Map;
            IPooledEnumerable eable = map.GetMobilesInRange(owner.Location, m_Radius);
            int targetsHit = 0;
            bool firstTarget = true;

            foreach (Mobile m in eable)
            {
                if (m != owner && (m is PlayerMobile || m is BaseCreature) && owner.InLOS(m) && targetsHit < 6)
                {
                    int damageToDeal = m_Damage;
                    if (firstTarget)
                    {
                        damageToDeal += (int)(m_Damage * 0.5); // Increase damage for the first target
                        firstTarget = false;
                    }
                    m.Damage(damageToDeal, owner);
                    targetsHit++;
                }
            }

            eable.Free(); // Ensure to free the pooled enumerable to prevent memory leaks
            m_LastUsed = DateTime.Now;
        }
    }
}
