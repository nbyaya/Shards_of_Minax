using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using System.Collections;
using Server.Items;

namespace Server.ACC.CSS.Systems.FencingMagic
{
    public class PrecisionStrike : FencingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Precision Strike", "Vas Flam Hur",
            21005,
            9301
        );

        public override SpellCircle Circle { get { return SpellCircle.Third; } }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }

        public PrecisionStrike(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private PrecisionStrike m_Owner;

            public InternalTarget(PrecisionStrike owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile target)
                {
                    if (from.CanBeHarmful(target) && m_Owner.CheckSequence())
                    {
                        from.DoHarmful(target);

                        // Play a blood splatter visual effect and sound
                        target.FixedParticles(0x374A, 10, 15, 5032, EffectLayer.Waist);
                        target.PlaySound(0x1BA);

                        // Apply bleeding effect
                        new BleedTimer(target, from).Start();

                        m_Owner.FinishSequence();
                    }
                }
            }
        }

        private class BleedTimer : Timer
        {
            private Mobile m_Target;
            private Mobile m_From;
            private int m_Ticks;

            public BleedTimer(Mobile target, Mobile from) : base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(2.0))
            {
                m_Target = target;
                m_From = from;
                m_Ticks = 0;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Ticks >= 5 || m_Target == null || m_Target.Deleted || !m_Target.Alive)
                {
                    Stop();
                    return;
                }

                if (m_From.CanBeHarmful(m_Target))
                {
                    int damage = Utility.RandomMinMax(5, 10); // Random bleeding damage
                    m_From.DoHarmful(m_Target);
                    m_Target.Damage(damage, m_From);

                    // Apply a smaller blood splatter visual effect for each tick
                    m_Target.FixedParticles(0x374A, 10, 15, 5032, EffectLayer.Waist);
                    m_Target.PlaySound(0x1BA);
                }

                m_Ticks++;
            }
        }
    }
}
