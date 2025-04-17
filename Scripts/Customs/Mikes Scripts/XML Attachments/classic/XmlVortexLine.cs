using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Engines.XmlSpawner2
{
    public class XmlVortexLine : XmlAttachment
    {
        private int m_Damage = 30;
        private int m_Range = 10;
        private TimeSpan m_Refractory = TimeSpan.FromSeconds(1);
        private DateTime m_EndTime;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Damage { get { return m_Damage; } set { m_Damage = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Range { get { return m_Range; } set { m_Range = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Refractory { get { return m_Refractory; } set { m_Refractory = value; } }

        public XmlVortexLine(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlVortexLine() { }

        [Attachable]
        public XmlVortexLine(double refractory, int damage, int range)
        {
            Refractory = TimeSpan.FromSeconds(refractory);
            Damage = damage;
            Range = range;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
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
            return String.Format("Line Breath Attack: {0} damage over {1} tiles every {2} seconds.", m_Damage, m_Range, m_Refractory.TotalSeconds);
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            if (DateTime.Now >= m_EndTime)
            {
                PerformLineBreathAttack(attacker);
                m_EndTime = DateTime.Now + Refractory;
            }
        }

		public void PerformLineBreathAttack(Mobile owner)
		{
			if (owner == null || DateTime.Now < m_EndTime || owner.Map == null)
				return;

			Map map = owner.Map;
			Direction d = owner.Direction;
			int dx = 0, dy = 0;
			int offset = 1;

			// Determine primary direction vector
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

			Point3D start = new Point3D(owner.X + offset * dx, owner.Y + offset * dy, owner.Z);

			// Determine perpendicular vectors for thickness calculation
			int perpDx = dy;
			int perpDy = -dx;

			for (int i = 0; i < m_Range; i++)
			{
				int targetX = start.X + i * dx;
				int targetY = start.Y + i * dy;

				// Iterate over thickness range (-1, 0, 1) to get three tiles thick line
				for (int thickness = -1; thickness <= 1; thickness++)
				{
					int thickX = targetX + thickness * perpDx;
					int thickY = targetY + thickness * perpDy;

					if (map.CanFit(thickX, thickY, start.Z, 16, false, false))
					{
						Point3D p = new Point3D(thickX, thickY, start.Z);
						int flameHue = 0;  // Or adjust hue if needed
						Effects.SendLocationEffect(p, map, 0x3789, 16, 10, flameHue, 0);

						IPooledEnumerable eable = map.GetMobilesInRange(p, 0);
						foreach (Mobile m in eable)
						{
							if (m is PlayerMobile || m is BaseCreature)
							{
								m.Damage(m_Damage, owner);
							}
						}
						eable.Free();
					}
				}
			}

			m_EndTime = DateTime.Now + m_Refractory;
		}

    }
}
