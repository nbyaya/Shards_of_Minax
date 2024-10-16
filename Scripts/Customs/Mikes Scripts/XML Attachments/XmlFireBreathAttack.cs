using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Spells;
using Server.Network;
using Server.Spells.Fourth;

namespace Server.Engines.XmlSpawner2
{
    public class XmlFireBreathAttack : XmlAttachment
    {
        private int m_BaseDamage = 45; // Default base fire damage
        private int m_Range = 10; // Default range of the cone in tiles
        private TimeSpan m_Delay = TimeSpan.FromSeconds(0.5); // Delay for damage application

        [CommandProperty(AccessLevel.GameMaster)]
        public int BaseDamage { get { return m_BaseDamage; } set { m_BaseDamage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Range { get { return m_Range; } set { m_Range = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Delay { get { return m_Delay; } set { m_Delay = value; } }

        public XmlFireBreathAttack(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlFireBreathAttack() { }

        [Attachable]
        public XmlFireBreathAttack(int baseDamage, int range, double delay)
        {
            BaseDamage = baseDamage;
            Range = range;
            Delay = TimeSpan.FromSeconds(delay);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_BaseDamage);
            writer.Write(m_Range);
            writer.Write(m_Delay);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 0)
            {
                m_BaseDamage = reader.ReadInt();
                m_Range = reader.ReadInt();
                m_Delay = reader.ReadTimeSpan();
            }
        }

        public void PerformBreathAttack(Mobile attacker)
        {
            Map map = attacker.Map;
            if (map == null) return;

            attacker.Animate(11, 5, 1, true, false, 0); // Attack animation
            attacker.PlaySound(0x227); // Fire breath sound

            Direction d = attacker.Direction;
            List<Point3D> fireTiles = new List<Point3D>();

            int[] coneWidths = { 1, 1, 3, 3, 3, 5 };

            for (int i = 1; i <= m_Range; i++)
            {
                int currentWidth = coneWidths[Math.Min(i - 1, coneWidths.Length - 1)];

                for (int j = -currentWidth / 2; j <= currentWidth / 2; j++)
                {
                    int targetX = attacker.X;
                    int targetY = attacker.Y;

                    // Adjust positions based on direction
                    switch (d & Direction.Mask)
                    {
                        case Direction.North:
                            targetY -= i; targetX += j; break;
                        case Direction.East:
                            targetX += i; targetY += j; break;
                        case Direction.South:
                            targetY += i; targetX += j; break;
                        case Direction.West:
                            targetX -= i; targetY += j; break;
                        case Direction.Right: // North-East
                            targetX += i; targetY -= i; targetX += j; break;
                        case Direction.Down: // South-East
                            targetX += i; targetY += i; targetY += j; break;
                        case Direction.Left: // South-West
                            targetX -= i; targetY += i; targetX += j; break;
                        case Direction.Up: // North-West
                            targetX -= i; targetY -= i; targetX += j; break;
                    }

                    if (map.CanFit(targetX, targetY, attacker.Z, 16, false, false))
                    {
                        fireTiles.Add(new Point3D(targetX, targetY, attacker.Z));
                        ApplyDamage(targetX, targetY, attacker);
                    }
                }
            }

            // Apply fire field effect to the ground
            foreach (Point3D p in fireTiles)
            {
                SpawnFireField(p, map);
            }
        }

        private void ApplyDamage(int targetX, int targetY, Mobile attacker)
        {
            IPooledEnumerable nearbyMobiles = attacker.Map.GetMobilesInRange(new Point3D(targetX, targetY, attacker.Z), 1);
            List<Mobile> affectedTargets = new List<Mobile>(); // Track targets affected by damage

            foreach (Mobile m in nearbyMobiles)
            {
                if (m != null && m.Alive && m.CanBeHarmful(attacker))
                {
                    double damageReductionFactor = 1.0 - (m.GetResistance(ResistanceType.Fire) / 100.0);
                    int finalDamage = (int)(m_BaseDamage * damageReductionFactor);

                    Timer.DelayCall(m_Delay, () =>
                    {
                        if (m.Alive && m.CanBeHarmful(attacker))
                            m.Damage(finalDamage, attacker);
                    });

                    affectedTargets.Add(m);
                }
            }
            nearbyMobiles.Free();
        }

		private void SpawnFireField(Point3D location, Map map)
		{
			// Ensure the map is valid before creating the fire field
			if (map == null)
			{
				Console.WriteLine("Map is null. Cannot spawn fire field.");
				return;
			}

			// Check if the location can fit the fire field
			if (!map.CanFit(location.X, location.Y, location.Z, 16, false, false))
			{
				Console.WriteLine($"Cannot fit fire field at {location.X}, {location.Y}, {location.Z}.");
				return;
			}

			// Ensure the Z coordinate is adjusted correctly for uneven terrain
			int z = map.GetAverageZ(location.X, location.Y);

			// Adjust location with valid Z if needed
			Point3D validLocation = new Point3D(location.X, location.Y, z);

			// Create the fire field at the adjusted location
			FireFieldSpell.FireFieldItem fireField = new FireFieldSpell.FireFieldItem(0x398C, validLocation, null, map, TimeSpan.FromSeconds(30), 20);
			
			
			// Log to verify
			Console.WriteLine($"Fire field spawned at {validLocation.X}, {validLocation.Y}, {validLocation.Z}.");
		}

    }
}
