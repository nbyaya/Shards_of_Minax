using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.ACC.CSS.Systems.FencingMagic
{
    public class Disengage : FencingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Disengage", "Vas Rel",
            21005, // Effect ID
            9301   // Animation ID
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.05; } }
        public override double RequiredSkill { get { return 0.0; } } // No specific skill required
        public override int RequiredMana { get { return 20; } }

        public Disengage(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                try
                {
                    // Calculate the position to teleport to
                    Point3D newLocation = GetBackwardLocation(Caster.Location, Caster.Direction, 4);
                    
                    // Check if the new location is valid (not blocked)
                    if (newLocation != Point3D.Zero && Caster.Map.CanFit(newLocation, 16, false, false))
                    {
                        // Play teleport sound and effect
                        Caster.PlaySound(0x1FE); // Teleport sound
                        Effects.SendLocationEffect(Caster.Location, Caster.Map, 0x3728, 13, 10, 0, 0); // Departure effect

                        // Move caster to new location
                        Caster.MoveToWorld(newLocation, Caster.Map);

                        // Play arrival effect
                        Effects.SendLocationEffect(newLocation, Caster.Map, 0x3728, 13, 10, 0, 0); // Arrival effect

                        // Optional: Flashy visual effect on caster after teleport
                        Caster.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);
                    }
                    else
                    {
                        Caster.SendLocalizedMessage(502138); // That location is blocked.
                    }
                }
                catch
                {
                    Caster.SendMessage("Something went wrong while trying to teleport.");
                }
            }

            FinishSequence();
        }

        // Helper method to calculate the new position
        private Point3D GetBackwardLocation(Point3D currentLocation, Direction direction, int distance)
        {
            int x = currentLocation.X;
            int y = currentLocation.Y;

            switch (direction)
            {
                case Direction.North:
                    y -= distance;
                    break;
                case Direction.South:
                    y += distance;
                    break;
                case Direction.East:
                    x += distance;
                    break;
                case Direction.West:
                    x -= distance;
                    break;
                case Direction.Up:
                    x -= distance;
                    y -= distance;
                    break;
                case Direction.Down:
                    x += distance;
                    y += distance;
                    break;
                case Direction.Left:
                    x -= distance;
                    y += distance;
                    break;
                case Direction.Right:
                    x += distance;
                    y -= distance;
                    break;
            }

            return new Point3D(x, y, currentLocation.Z);
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.0);
        }
    }
}
