using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using System.Collections;

namespace Server.ACC.CSS.Systems.MeditationMagic
{
    public class MeditativeHeal : MeditationSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Meditative Heal", "Sanctuary",
                                                        21004,
                                                        9300,
                                                        false
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.2; } } // Time it takes to cast the spell
        public override double RequiredSkill { get { return 50.0; } } // Minimum Meditation skill required
        public override int RequiredMana { get { return 25; } } // Mana cost

        public MeditativeHeal(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(Mobile target)
        {
            if (!Caster.CanSee(target))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
            }
            else if (CheckSequence() && target is Mobile)
            {
                if (this.Scroll != null)
                    Scroll.Consume();

                Caster.Mana -= RequiredMana;

                // Visual and sound effects for casting
                Caster.FixedParticles(0x375A, 10, 30, 5037, EffectLayer.Waist);
                Caster.PlaySound(0x1F2);

                double healingAmount = Caster.Skills[SkillName.Meditation].Value / 5; // Basic healing calculation
                TimeSpan duration = TimeSpan.FromSeconds(10.0); // Duration of the healing effect
                Timer timer = new InternalTimer(target, healingAmount, duration);
                timer.Start();

                FinishSequence();
            }
        }

        private class InternalTimer : Timer
        {
            private Mobile m_Target;
            private double m_HealingAmount;
            private DateTime m_End;

            public InternalTimer(Mobile target, double healingAmount, TimeSpan duration) : base(TimeSpan.FromSeconds(2.0), TimeSpan.FromSeconds(2.0))
            {
                m_Target = target;
                m_HealingAmount = healingAmount;
                m_End = DateTime.Now + duration;
                Priority = TimerPriority.TwoFiftyMS;
            }

            protected override void OnTick()
            {
                if (m_Target.Deleted || !m_Target.Alive || DateTime.Now > m_End)
                {
                    Stop();
                    return;
                }

                m_Target.Hits += (int)m_HealingAmount;

                // Visual and sound effects for each healing tick
                m_Target.FixedParticles(0x376A, 9, 32, 5005, EffectLayer.Waist);
                m_Target.PlaySound(0x1F3);

                if (DateTime.Now > m_End)
                {
                    Stop();
                }
            }
        }

        private class InternalTarget : Target
        {
            private MeditativeHeal m_Owner;

            public InternalTarget(MeditativeHeal owner) : base(12, false, TargetFlags.Beneficial)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                    m_Owner.Target((Mobile)o);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
