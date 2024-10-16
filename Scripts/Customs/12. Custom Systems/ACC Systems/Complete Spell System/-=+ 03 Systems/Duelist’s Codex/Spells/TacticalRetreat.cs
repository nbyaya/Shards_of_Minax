using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.FencingMagic
{
    public class TacticalRetreat : FencingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Tactical Retreat", "Relocatus Randomus",
            // SpellCircle.First,
            21005,
            9301
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 20.0; } }
        public override int RequiredMana { get { return 20; } }

        public TacticalRetreat(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

		public override void OnCast()
		{
			if (CheckSequence())
			{
				// Play flashy visual and sound effect at the caster's current location
				Effects.SendLocationEffect(Caster.Location, Caster.Map, 0x3709, 30, 10, 1153, 0);
				Caster.PlaySound(0x5C3);

				// Calculate random location within a 6-tile radius
				Point3D targetLocation = GetRandomLocation(Caster.Location, 6);

				// Ensure the target location is valid and not blocked
				if (targetLocation != Point3D.Zero && SpellHelper.CanRevealCaster(Caster))  // Adjusted line
				{
					Caster.MoveToWorld(targetLocation, Caster.Map);
					Effects.SendLocationEffect(Caster.Location, Caster.Map, 0x373A, 30, 10, 1153, 0);
					Caster.PlaySound(0x1FE);
				}
				else
				{
					Caster.SendMessage("You can't teleport to that location.");
				}
			}

			FinishSequence();
		}


        private Point3D GetRandomLocation(Point3D origin, int radius)
        {
            Map map = Caster.Map;

            for (int i = 0; i < 10; i++) // Attempt 10 times to find a valid location
            {
                int x = origin.X + Utility.RandomMinMax(-radius, radius);
                int y = origin.Y + Utility.RandomMinMax(-radius, radius);
                int z = map.GetAverageZ(x, y);

                if (map.CanFit(x, y, z, 16, true, false, false))
                {
                    return new Point3D(x, y, z);
                }
            }

            return Point3D.Zero; // If no valid location found, return an invalid point
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0);
        }
    }
}
