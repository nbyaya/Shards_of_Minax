using System;
using System.Collections.Generic; // Required for List<>
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.Engines.XmlSpawner2
{
    public class XmlMushroomBreath : XmlAttachment
    {
        private int m_Damage = 30; // Default damage for the breath attack
        private int m_Range = 10; // Range of the attack in tiles
        private TimeSpan m_Refractory = TimeSpan.FromSeconds(1); // Time between activations
        private DateTime m_EndTime;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Damage { get { return m_Damage; } set { m_Damage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Range { get { return m_Range; } set { m_Range = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Refractory { get { return m_Refractory; } set { m_Refractory = value; } }

        public XmlMushroomBreath(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlMushroomBreath() { }

        [Attachable]
        public XmlMushroomBreath(double refractory, int damage, int range)
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
            return String.Format("Breath Attack: {0} damage over {1} tiles every {2} seconds.", m_Damage, m_Range, m_Refractory.TotalSeconds);
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.Now >= m_EndTime)
            {
                PerformBreathAttack(attacker);
                m_EndTime = DateTime.Now + Refractory;
            }
        }

        public void PerformBreathAttack(Mobile owner)
        {
            if (owner == null || DateTime.Now < m_EndTime || owner.Map == null)
                return;

            Map map = owner.Map;
            Direction d = owner.Direction;
            List<Point3D>[] targetRows = new List<Point3D>[m_Range]; // An array to hold lists of targets for each row

            for (int i = 0; i < m_Range; i++)
            {
                targetRows[i] = new List<Point3D>();
            }

            int dx = 0, dy = 0;
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
                // Handling intercardinal directions
                case Direction.Right: // North-East
                    dx = 1;
                    dy = -1;
                    break;
                case Direction.Down: // South-East
                    dx = 1;
                    dy = 1;
                    break;
                case Direction.Left: // South-West
                    dx = -1;
                    dy = 1;
                    break;
                case Direction.Up: // North-West
                    dx = -1;
                    dy = -1;
                    break;
            }

            for (int i = 1; i <= m_Range; i++)
            {
                int baseX = owner.X + i * dx;
                int baseY = owner.Y + i * dy;
                int perpendicularRange = i / 2; // Reduced perpendicular range for balance

                for (int j = -perpendicularRange; j <= perpendicularRange; j++)
                {
                    int targetX = baseX, targetY = baseY;

                    if (dx == 0)
                        targetX += j;
                    else if (dy == 0)
                        targetY += j;
                    else
                    {
                        targetX += j;
                        targetY += (dx * dy > 0) ? -j : j;
                    }

                    Point3D p = new Point3D(targetX, targetY, owner.Z);
                    if (map.CanFit(p.X, p.Y, p.Z, 16, false, false))
                        targetRows[i - 1].Add(p);
                    else
                    {
                        int targetZ = map.GetAverageZ(targetX, targetY);
                        if (map.CanFit(targetX, targetY, targetZ, 16, false, false))
                            targetRows[i - 1].Add(new Point3D(targetX, targetY, targetZ));
                    }
                }
            }

            TimeSpan delay = TimeSpan.Zero;
            TimeSpan step = TimeSpan.FromSeconds(0.1); // Adjust the step duration as needed

            foreach (List<Point3D> row in targetRows)
            {
                Timer.DelayCall(delay, () =>
                {
                    foreach (Point3D p in row)
                    {
                        Effects.SendLocationEffect(p, map, 0x1126, 30, 10, 0, 0); // Use a suitable effect for the breath attack
                        foreach (Mobile m in map.GetMobilesInRange(p, 0))
                        {
                            if (m != owner && (m is PlayerMobile || m is BaseCreature) && owner.InLOS(m))
                            {
                                // Optionally check for friend/foe here
                                m.Damage(m_Damage, owner);
                            }
                        }
                    }
                });
                delay += step;
            }

            m_EndTime = DateTime.Now + Refractory;
        }
    }
}
