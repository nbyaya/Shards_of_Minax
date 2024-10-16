using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections;

namespace Server.ACC.CSS.Systems.ForensicsMagic
{
    public class PainAmplifier : ForensicsSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Pain Amplifier", "Dolore Amplifico",
            21004,
            9300,
            false,
            Reagent.BatWing,
            Reagent.SpidersSilk
        );

        public override SpellCircle Circle { get { return SpellCircle.Third; } }
        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 25; } }

        public PainAmplifier(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private PainAmplifier m_Owner;

            public InternalTarget(PainAmplifier owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                {
                    Mobile target = (Mobile)o;

                    if (from.CanBeHarmful(target) && m_Owner.CheckSequence())
                    {
                        from.DoHarmful(target);
                        m_Owner.CastPainAmplifier(target);
                    }
                }

                m_Owner.FinishSequence();
            }
        }

        public void CastPainAmplifier(Mobile target)
        {
            target.PlaySound(0x208); // Play a painful sound
            target.FixedParticles(0x374A, 10, 15, 5036, EffectLayer.Waist); // Apply visual effect
            target.SendMessage("You feel an intense pain coursing through your body!");

            new PainAmplifierEffect(target, Caster).Start();
        }

        private class PainAmplifierEffect : Timer
        {
            private Mobile m_Target;
            private Mobile m_Caster;
            private int m_Ticks;
            private static readonly TimeSpan Delay = TimeSpan.FromSeconds(1.0);
            private const int MaxTicks = 10; // Lasts for 10 seconds

            public PainAmplifierEffect(Mobile target, Mobile caster) : base(Delay, Delay)
            {
                m_Target = target;
                m_Caster = caster;
                m_Ticks = 0;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Target.Deleted || !m_Target.Alive || m_Ticks >= MaxTicks)
                {
                    Stop();
                    return;
                }

                int damage = Utility.RandomMinMax(5, 10) + (int)(m_Caster.Skills[SkillName.Forensics].Value / 10); // Damage based on caster's skill
                m_Target.Damage(damage, m_Caster);
                m_Target.FixedEffect(0x376A, 10, 16); // Small burst effect on the target
                m_Target.PlaySound(0x1FB); // Sound for each tick of damage

                m_Ticks++;
            }
        }
    }
}
