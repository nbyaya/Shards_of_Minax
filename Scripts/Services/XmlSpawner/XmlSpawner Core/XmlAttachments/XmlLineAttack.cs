using System;
using System.Collections.Generic; // Required for List<>
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Engines.XmlSpawner2
{
    public class XmlLineAttack : XmlAttachment
    {
        private int m_Damage = 30; // Default damage for the line attack
        private int m_Range = 10; // Range of the attack in tiles
        private TimeSpan m_Refractory = TimeSpan.FromSeconds(1); // Time between activations
        private DateTime m_EndTime;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Damage { get { return m_Damage; } set { m_Damage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Range { get { return m_Range; } set { m_Range = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Refractory { get { return m_Refractory; } set { m_Refractory = value; } }

        public XmlLineAttack(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlLineAttack() { }

        [Attachable]
        public XmlLineAttack(double refractory, int damage, int range)
        {
            Refractory = TimeSpan.FromSeconds(refractory);
            Damage = damage;
            Range = range;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_Damage);
            writer.Write(m_Range);
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
                m_Range = reader.ReadInt();
                Refractory = reader.ReadTimeSpan();
                m_EndTime = reader.ReadDeltaTime();
            }
        }

        public override string OnIdentify(Mobile from)
        {
            return String.Format("Line Attack: {0} damage along {1} tiles every {2} seconds.", m_Damage, m_Range, m_Refractory.TotalSeconds);
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.Now >= m_EndTime)
            {
                PerformLineAttack(attacker);
                m_EndTime = DateTime.Now + Refractory;
            }
        }

        public void PerformLineAttack(Mobile owner)
        {
            if (owner == null || DateTime.Now < m_EndTime || owner.Map == null)
                return;

            Map map = owner.Map;

            Direction d = owner.Direction;
            List<Point3D> targets = new List<Point3D>();

            int dx = 0, dy = 0;
            // Determine direction for the line
            switch (d & Direction.Mask)
            {
                case Direction.North:
                    dy = -1;
                    break;
                case Direction.East:
                    dx = 1;
                    break;
                case Direction.South:
                    dy = 1;
                    break;
                case Direction.West:
                    dx = -1;
                    break;
                // For simplicity, diagonal attacks could either not be allowed, or treated as one of the cardinal directions
            }

            // Generate line targets
            for (int i = 1; i <= m_Range; i++)
            {
                int targetX = owner.X + i * dx;
                int targetY = owner.Y + i * dy;

                Point3D p = new Point3D(targetX, targetY, owner.Z);
                if (map.CanFit(p.X, p.Y, p.Z, 16, false, false))
                    targets.Add(p);
                else
                {
                    int targetZ = map.GetAverageZ(targetX, targetY);
                    if (map.CanFit(targetX, targetY, targetZ, 16, false, false))
                        targets.Add(new Point3D(targetX, targetY, targetZ));
                }
            }

            // Apply effect and damage
            foreach (Point3D p in targets)
            {
                Effects.SendLocationEffect(p, map, 0x3709, 30, 10, 0, 0); // Suitable effect for the line attack
                foreach (Mobile m in map.GetMobilesInRange(p, 0))
                {
                    if (m != owner && (m is PlayerMobile || m is BaseCreature) && owner.InLOS(m))
                    {
                        m.Damage(m_Damage, owner);
                    }
                }
            }

            m_EndTime = DateTime.Now + Refractory;
        }
    }
}
