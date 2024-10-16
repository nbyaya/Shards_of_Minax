using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using System.Collections.Generic;

namespace Server.Mobiles
{
    public class MegaDragon : BaseCreature
    {
        private DateTime m_NextBreathTime;
		[Constructable]
        public MegaDragon() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "MegaDragon";
            Body = 49; // Adjust to appropriate dragon model
            BaseSoundID = 362;

            SetStr(1096, 1185);
            SetDex(86, 175);
            SetInt(686, 775);

            SetHits(658, 711);

            SetDamage(29, 35);
			
			m_NextBreathTime = DateTime.UtcNow; // Initialize the cooldown

            // Adjust skills, stats, and resistances as needed
        }

        public override void OnThink()
        {
            base.OnThink();

            // Example trigger for the special attack, adjust as needed
            if (Combatant != null && DateTime.UtcNow >= m_NextBreathTime)
            {
                BreathSpecialAttack();
				m_NextBreathTime = DateTime.UtcNow + TimeSpan.FromSeconds(5);
            }
        }

        public void BreathSpecialAttack()
		{
			Map map = this.Map;

			if (map == null)
				return;

			Direction d = this.Direction;
			int range = 10; // Range of the attack in tiles
			int damage = 30; // Damage for the breath attack
			List<Point3D> targets = new List<Point3D>();

			// Calculate offsets based on the dragon's direction
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
				// Include cases for other directions if needed (e.g., down, left, up, right)
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

			for (int i = 1; i <= range; i++)
			{
				int perpendicularRange = i;
				int baseX = this.X + i * dx;
				int baseY = this.Y + i * dy;

				for (int j = -perpendicularRange; j <= perpendicularRange; j++)
				{
					int targetX = baseX;
					int targetY = baseY;

					if (Math.Abs(dx) != Math.Abs(dy))
					{
						// Handling cardinal directions (North, East, South, West)
						if (dx == 0)
						{
							targetX += j;
						}
						else if (dy == 0)
						{
							targetY += j;
						}
					}
					else
					{
						// Handling intercardinal directions (NE, SE, SW, NW)
						if (dx * dy > 0) // Moving in SE or NW direction
						{
							targetX += j;
							targetY -= j; // Invert the increment for one axis to spread perpendicularly
						}
						else // Moving in NE or SW direction
						{
							targetX += j;
							targetY += j; // Both axes increment in the same direction for NE and SW
						}
					}

					if (map.CanFit(targetX, targetY, this.Z, 16, false, false))
						targets.Add(new Point3D(targetX, targetY, this.Z));
					else
					{
						int targetZ = map.GetAverageZ(targetX, targetY);
						if (map.CanFit(targetX, targetY, targetZ, 16, false, false))
							targets.Add(new Point3D(targetX, targetY, targetZ));
					}
				}
			}

			foreach (Point3D p in targets)
			{
				int flameHue = 2543; // Set this to the hue value you want for the flamestrike effect.
				Effects.SendLocationEffect(p, map, 0x36BD, 16, 10, flameHue, 0); // Flamestrike animation
				IPooledEnumerable eable = map.GetMobilesInRange(p, 0); // Only target mobiles in the same tile
				foreach (Mobile m in eable)
				{
					if (m is PlayerMobile || m is BaseCreature)
					{
						// We can check if the mobile is a friend or foe before applying damage
						// This example will damage all mobiles, but you could check for allegiances or factions as needed
						m.Damage(damage, this);
					}
				}
				eable.Free(); // Always free the enumerable when done to avoid memory leaks
			}
		}


        public MegaDragon(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
