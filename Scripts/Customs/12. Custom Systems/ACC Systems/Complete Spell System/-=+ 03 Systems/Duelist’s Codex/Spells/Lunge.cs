using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.FencingMagic
{
    public class Lunge : FencingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Lunge", "Lungus Impactus",
                                                        21005,
                                                        9301
                                                       );

        public override SpellCircle Circle => SpellCircle.First;
        public override double CastDelay => 0.5;
        public override double RequiredSkill => 0.0;
        public override int RequiredMana => 20;

        public Lunge(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private Lunge m_Owner;

            public InternalTarget(Lunge owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile target && from.CanBeHarmful(target))
                {
                    m_Owner.LungeEffect(target);
                }
                else
                {
                    from.SendLocalizedMessage(500237); // Target cannot be seen or is not valid.
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public void LungeEffect(Mobile target)
        {
            if (CheckSequence())
            {
                // Damage calculation
                int damage = Utility.RandomMinMax(30, 50); // Heavy damage range
                Caster.DoHarmful(target);
                AOS.Damage(target, Caster, damage, 100, 0, 0, 0, 0);

                // Visual and sound effects
                Effects.SendLocationParticles(EffectItem.Create(target.Location, target.Map, TimeSpan.FromSeconds(10)), 0x3709, 1, 30, 1153, 7, 9502, 0);
                target.PlaySound(0x1F7); // Sound effect for the lunge impact

                // Additional visual feedback
                target.FixedParticles(0x3779, 1, 15, 9502, 1153, 7, EffectLayer.Waist);
                target.SendMessage("You have been hit by a powerful lunge!");

                // Additional knockback effect (optional)
                int knockbackDistance = 2; // Distance to knockback
                Direction dir = Caster.GetDirectionTo(target);
                Point3D knockbackLocation = GetNewLocation(target.Location, dir, knockbackDistance);
                
                if (target.Map.CanSpawnMobile(knockbackLocation))
                {
                    target.Location = knockbackLocation;
                    target.SendMessage("You are knocked back by the force of the lunge!");
                }
            }

            FinishSequence();
        }

        private Point3D GetNewLocation(Point3D start, Direction direction, int distance)
        {
            int x = start.X, y = start.Y, z = start.Z;

            switch (direction)
            {
                case Direction.North:
                    y -= distance;
                    break;
                case Direction.South:
                    y += distance;
                    break;
                case Direction.West:
                    x -= distance;
                    break;
                case Direction.East:
                    x += distance;
                    break;
                case Direction.Right:
                    x += distance;
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
                case Direction.Up:
                    x -= distance;
                    y -= distance;
                    break;
            }

            return new Point3D(x, y, z);
        }
    }
}
