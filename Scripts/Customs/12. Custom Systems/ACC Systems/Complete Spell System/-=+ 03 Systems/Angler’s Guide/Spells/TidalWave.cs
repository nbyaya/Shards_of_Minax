using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using Server.Misc;
using System.Collections;

namespace Server.ACC.CSS.Systems.FishingMagic
{
    public class TidalWave : FishingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Tidal Wave", "Aqua Maximus",
                                                        //SpellCircle.Sixth,
                                                        21004,
                                                        9300
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 65.0; } }
        public override int RequiredMana { get { return 15; } }

        public TidalWave(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(IPoint3D p)
        {
            if (!Caster.CanSee(p))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
            }
            else if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
            {
                SpellHelper.Turn(Caster, p);
                SpellHelper.GetSurfaceTop(ref p);

                // Visual and sound effects
                Effects.PlaySound(p, Caster.Map, 0x026); // Sound of water wave
                Effects.SendLocationEffect(new Point3D(p), Caster.Map, 0x352D, 20, 10, 1153, 0); // Blue water effect

                // Knockback and stun logic
                var targets = Caster.GetMobilesInRange(3); // Range of the wave effect
                foreach (Mobile m in targets)
                {
                    if (m != Caster && Caster.CanBeHarmful(m, false))
                    {
                        Caster.DoHarmful(m);

                        // Calculate knockback location
                        Point3D knockbackLocation = CalculateKnockbackLocation(m, Caster.GetDirectionTo(m), 2);

                        // Move target to knockback location
                        m.MoveToWorld(knockbackLocation, m.Map);

                        // Stun effect
                        m.Freeze(TimeSpan.FromSeconds(3)); // Stun for 3 seconds

                        // Additional visual and sound effects for each target
                        m.FixedParticles(0x374A, 10, 15, 5038, EffectLayer.Head); // Water splash effect
                        m.PlaySound(0x013); // Splash sound
                    }
                }
            }

            FinishSequence();
        }

        private Point3D CalculateKnockbackLocation(Mobile target, Direction direction, int distance)
        {
            int x = target.X, y = target.Y;

            switch (direction)
            {
                case Direction.North: y -= distance; break;
                case Direction.South: y += distance; break;
                case Direction.East: x += distance; break;
                case Direction.West: x -= distance; break;
                case Direction.Right: x += distance; y -= distance; break; // Northeast
                case Direction.Left: x -= distance; y += distance; break; // Southwest
                case Direction.Up: x -= distance; y -= distance; break; // Northwest
                case Direction.Down: x += distance; y += distance; break; // Southeast
            }

            return new Point3D(x, y, target.Z);
        }

        private class InternalTarget : Target
        {
            private TidalWave m_Owner;

            public InternalTarget(TidalWave owner) : base(12, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is IPoint3D)
                    m_Owner.Target((IPoint3D)o);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
