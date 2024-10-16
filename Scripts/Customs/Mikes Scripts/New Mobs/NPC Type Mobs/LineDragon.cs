using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using System.Collections.Generic;

namespace Server.Mobiles
{
    public class LineDragon : BaseCreature
    {
        private DateTime m_NextBreathTime;
		[Constructable]
        public LineDragon() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "LineDragon";
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

			// Calculate offsets based on the dragon's direction
			int dx = 0, dy = 0;
			int offset = 1; // To avoid hitting the dragon itself in diagonal directions
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
				case Direction.Right: // North-East
					dx = 1; dy = -1;
					break;
				case Direction.Down: // South-East
					dx = 1; dy = 1;
					break;
				case Direction.Left: // South-West
					dx = -1; dy = 1;
					break;
				case Direction.Up: // North-West
					dx = -1; dy = -1;
					break;
			}

			// Starting point adjustment for diagonal directions
			Point3D start = new Point3D(this.X + offset * dx, this.Y + offset * dy, this.Z);

			for (int i = 0; i < range; i++)
			{
				int targetX = start.X + i * dx;
				int targetY = start.Y + i * dy;

				if (map.CanFit(targetX, targetY, start.Z, 16, false, false))
				{
					Point3D p = new Point3D(targetX, targetY, start.Z);
					int flameHue = 2543; // Flamestrike effect hue value.
					Effects.SendLocationEffect(p, map, 0x36BD, 16, 10, flameHue, 0); // Flamestrike animation

					IPooledEnumerable eable = map.GetMobilesInRange(p, 0);
					foreach (Mobile m in eable)
					{
						if (m is PlayerMobile || m is BaseCreature)
						{
							m.Damage(damage, this);
						}
					}
					eable.Free(); // Always free the enumerable to avoid memory leaks
				}
			}
		}



        public LineDragon(Serial serial) : base(serial)
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
