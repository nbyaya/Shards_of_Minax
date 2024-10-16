using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Spells;

namespace Server.Engines.XmlSpawner2
{
    public class XmlSkeletonCircle : XmlAttachment
    {
        private int m_Damage = 20; // Default damage for the circle fire attack
        private int m_Radius = 5; // Default radius of the circular attack
        private int m_Thickness = 4; // Default thickness of the attack
        private TimeSpan m_Refractory = TimeSpan.FromSeconds(10); // Time between activations
        private DateTime m_EndTime;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Damage { get { return m_Damage; } set { m_Damage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Radius { get { return m_Radius; } set { m_Radius = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Thickness { get { return m_Thickness; } set { m_Thickness = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Refractory { get { return m_Refractory; } set { m_Refractory = value; } }

        public XmlSkeletonCircle(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlSkeletonCircle() { }

        [Attachable]
        public XmlSkeletonCircle(double refractory, int damage, int radius, int thickness)
        {
            Refractory = TimeSpan.FromSeconds(refractory);
            Damage = damage;
            Radius = radius;
            Thickness = thickness;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Damage);
            writer.Write(m_Radius);
            writer.Write(m_Thickness);
            writer.Write(m_Refractory);
            writer.WriteDeltaTime(m_EndTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_Damage = reader.ReadInt();
                m_Radius = reader.ReadInt();
                m_Thickness = reader.ReadInt();
                Refractory = reader.ReadTimeSpan();
                m_EndTime = reader.ReadDeltaTime();
            }
        }

        public override string OnIdentify(Mobile from)
        {
            return String.Format("Circle Fire Attack: {0} damage over {1} tiles every {2} seconds.", m_Damage, m_Radius, m_Refractory.TotalSeconds);
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.Now >= m_EndTime)
            {
                PerformCircleFireAttack(attacker);
                m_EndTime = DateTime.Now + Refractory;
            }
        }

        public void PerformCircleFireAttack(Mobile owner)
        {
            if (owner == null || DateTime.Now < m_EndTime || owner.Map == null)
                return;

            Map map = owner.Map;

            for (int i = -m_Radius; i <= m_Radius; i++)
            {
                for (int j = -m_Radius; j <= m_Radius; j++)
                {
                    if (i * i + j * j <= m_Radius * m_Radius && i * i + j * j >= (m_Radius - m_Thickness) * (m_Radius - m_Thickness))
                    {
                        Point3D p = new Point3D(owner.X + i, owner.Y + j, owner.Z);
                        if (map.CanFit(p.X, p.Y, p.Z, 16, false, false))
                        {
                            Effects.SendLocationEffect(p, map, 0x1B7F, 16, 10, 0, 0); // Flamestrike animation with default hue
                            DealDamageAtPoint(p, map, owner);
                        }
                    }
                }
            }
        }

        private void DealDamageAtPoint(Point3D p, Map map, Mobile owner)
        {
            IPooledEnumerable eable = map.GetMobilesInRange(p, 0);
            foreach (Mobile m in eable)
            {
                if (m is PlayerMobile || m is BaseCreature)
                {
                    m.Damage(m_Damage, owner); // Pass the owner (Mobile) instead of this (XmlSkeletonCircle)
                }
            }
            eable.Free();
        }
    }
}
