using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.ChivalryMagic
{
    public class ValorsCharge : ChivalrySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Valor's Charge", "Io Jux Sanct",
                                                        //SpellCircle.Fourth,
                                                        21004,
                                                        9300
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 20; } }

        public ValorsCharge(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private ValorsCharge m_Owner;

            public InternalTarget(ValorsCharge owner) : base(8, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                {
                    Mobile target = (Mobile)o;

                    if (!m_Owner.CheckHSequence(target))
                        return;

                    from.Direction = from.GetDirectionTo(target); // Face the target

                    // Apply effects
                    Effects.SendLocationParticles(EffectItem.Create(from.Location, from.Map, EffectItem.DefaultDuration), 0x3728, 10, 15, 5042); // Flashy effect on cast
                    from.PlaySound(0x20E); // Charging sound effect

                    // Calculate distance and move towards target
                    int distance = (int)from.GetDistanceToSqrt(target);

                    for (int i = 0; i < distance; i++)
                    {
                        Point3D step = new Point3D(
                            from.X + i * GetOffsetX(from.Direction),
                            from.Y + i * GetOffsetY(from.Direction),
                            from.Z
                        );
                        from.Location = step;
                        from.ProcessDelta();

                        // If an enemy is in the path, knock them down and deal damage
                        foreach (Mobile m in from.GetMobilesInRange(1))
                        {
                            if (m != from && m is BaseCreature && m.CanBeHarmful(from))
                            {
                                m.Damage(Utility.RandomMinMax(20, 40), from); // Bonus damage
                                m.SendMessage("You are knocked down by the force of the charge!");
                                m.Animate(21, 5, 1, true, false, 0); // Knockdown animation
                                m.Freeze(TimeSpan.FromSeconds(2)); // Stun effect
                                Effects.SendLocationParticles(EffectItem.Create(m.Location, m.Map, EffectItem.DefaultDuration), 0x36BD, 20, 10, 5044); // Impact effect
                                m.PlaySound(0x214); // Impact sound effect
                            }
                        }
                    }

                    m_Owner.FinishSequence();
                }
            }

            private int GetOffsetX(Direction direction)
            {
                switch (direction)
                {
                    case Direction.North: return 0;
                    case Direction.East: return 1;
                    case Direction.South: return 0;
                    case Direction.West: return -1;
                    default: return 0;
                }
            }

            private int GetOffsetY(Direction direction)
            {
                switch (direction)
                {
                    case Direction.North: return -1;
                    case Direction.East: return 0;
                    case Direction.South: return 1;
                    case Direction.West: return 0;
                    default: return 0;
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
