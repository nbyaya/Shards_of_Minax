using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Items;
using Server.Spells;

namespace Server.ACC.CSS.Systems.StealingMagic
{
    public class Backstab : StealingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Backstab", "DIE!",
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.5; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 10; } }

        public Backstab(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private Backstab m_Owner;

            public InternalTarget(Backstab owner) : base(1, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                {
                    Mobile target = (Mobile)o;

                    if (!from.CanBeHarmful(target))
                    {
                        from.SendLocalizedMessage(1008313); // You can't do that.
                        return;
                    }

                    if (from.InRange(target, 1) && from.CanSee(target))
                    {
                        if (from.Direction == from.GetDirectionTo(target))
                        {
                            from.SendMessage("You must be behind your target to backstab.");
                            return;
                        }

                        if (m_Owner.CheckSequence())
                        {
                            from.DoHarmful(target);
                            double damageBonus = 1.5; // 50% increased damage
                            int baseDamage = Utility.RandomMinMax(15, 25);
                            int finalDamage = (int)(baseDamage * damageBonus);

                            target.Damage(finalDamage, from);
                            from.SendMessage("You backstab your target for {0} damage!", finalDamage);

                            // Apply bleed effect
                            TimeSpan duration = TimeSpan.FromSeconds(10);
                            ApplyBleedEffect(target, duration);

                            // Visual and sound effects
                            Effects.SendLocationParticles(EffectItem.Create(target.Location, target.Map, EffectItem.DefaultDuration), 0x37CC, 1, 13, 1153, 2, 9962, 0);
                            Effects.PlaySound(target.Location, target.Map, 0x1E5);
                        }
                    }
                    else
                    {
                        from.SendLocalizedMessage(500326); // That is too far away.
                    }
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.0);
        }

        public static void ApplyBleedEffect(Mobile target, TimeSpan duration)
        {
            if (target == null || target.Deleted || !target.Alive)
                return;

            if (target.BeginAction(typeof(Backstab)))
            {
                target.SendMessage("You are bleeding!");

                Timer timer = new BleedTimer(target, duration);
                timer.Start();
            }
        }

        private class BleedTimer : Timer
        {
            private Mobile m_Target;

            public BleedTimer(Mobile target, TimeSpan duration) : base(TimeSpan.Zero, TimeSpan.FromSeconds(2.0))
            {
                m_Target = target;
                Delay = duration;
                Priority = TimerPriority.TwoFiftyMS;
            }

            protected override void OnTick()
            {
                if (m_Target == null || m_Target.Deleted || !m_Target.Alive)
                {
                    Stop();
                    return;
                }

                m_Target.PlaySound(0x133);
                m_Target.SendMessage("You are bleeding!");
                m_Target.Damage(Utility.RandomMinMax(2, 4));
            }
        }
    }
}
