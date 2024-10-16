using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections;

namespace Server.ACC.CSS.Systems.AnimalTamingMagic
{
    public class HealingTouch : AnimalTamingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Healing Touch", "In Vas Maxi",
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 40.0; } }
        public override int RequiredMana { get { return 20; } }

        public HealingTouch(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(Mobile m)
        {
            if (m == null || !Caster.CanSee(m) || !Caster.CanBeBeneficial(m, false))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
            }
            else if (CheckSequence())
            {
                Caster.DoBeneficial(m);

                Effects.SendLocationParticles(
                    EffectItem.Create(m.Location, m.Map, EffectItem.DefaultDuration),
                    0x376A, 9, 32, 5008
                );
                m.PlaySound(0x1F2);

                new HealingTimer(m, Caster).Start();
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private HealingTouch m_Owner;

            public InternalTarget(HealingTouch owner) : base(12, false, TargetFlags.Beneficial)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile)
                    m_Owner.Target((Mobile)targeted);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        private class HealingTimer : Timer
        {
            private Mobile m_Target;
            private Mobile m_Caster;
            private int m_HealAmount;
            private int m_Ticks;
            private int m_MaxTicks;

            public HealingTimer(Mobile target, Mobile caster) : base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0))
            {
                m_Target = target;
                m_Caster = caster;
                m_HealAmount = (int)(m_Caster.Skills[SkillName.Magery].Value / 10) + 5; // Base heal amount + skill scaling
                m_MaxTicks = 10; // Heal for 10 seconds
                m_Ticks = 0;
                Priority = TimerPriority.TwoFiftyMS;
            }

            protected override void OnTick()
            {
                if (m_Target.Deleted || !m_Target.Alive || m_Target.Hits >= m_Target.HitsMax)
                {
                    Stop();
                    return;
                }

                if (m_Ticks < m_MaxTicks)
                {
                    m_Ticks++;
                    m_Target.Hits += m_HealAmount;
                    m_Target.FixedParticles(0x376A, 1, 15, 1108, 7, 9921, 0); // Healing effect
                    m_Target.PlaySound(0x1F2); // Healing sound
                }
                else
                {
                    Stop();
                }
            }
        }
    }
}
