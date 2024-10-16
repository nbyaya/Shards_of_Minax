using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using System.Collections;
using Server.Items; // Added this line

namespace Server.ACC.CSS.Systems.InscribeMagic
{
    public class MarkOfTheBerserker : InscribeSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Mark of the Berserker", "Vas Uus Bersk",
            21004, // Item ID for icon
            9300,  // Animation or effect ID
            false,
            Reagent.Bloodmoss
        );

        public override SpellCircle Circle => SpellCircle.Second;

        public override double CastDelay => 1.5;
        public override double RequiredSkill => 60.0;
        public override int RequiredMana => 30;

        public MarkOfTheBerserker(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private MarkOfTheBerserker m_Owner;

            public InternalTarget(MarkOfTheBerserker owner) : base(12, false, TargetFlags.Beneficial)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile target && from.CanBeBeneficial(target, false))
                {
                    from.DoBeneficial(target);

                    if (m_Owner.CheckSequence())
                    {
                        target.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);
                        target.PlaySound(0x1F5);

                        BuffInfo.AddBuff(target, new BuffInfo(BuffIcon.Berserk, 1075815, 1075816, TimeSpan.FromSeconds(30), from));
                        target.SendMessage(0x35, "You feel a surge of berserk fury!");

                        // Apply the buff effect using a timer
                        new BuffTimer(target, TimeSpan.FromSeconds(30)).Start();
                    }

                    m_Owner.FinishSequence();
                }
                else
                {
                    from.SendLocalizedMessage(500237); // Target cannot be seen or is invalid.
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        private class BuffTimer : Timer
        {
            private Mobile m_Target;
            private DateTime m_EndTime;
            private int m_OriginalStrength; // Store original strength
            private int m_StrengthBonus; // Strength bonus to be applied

            public BuffTimer(Mobile target, TimeSpan duration) : base(TimeSpan.Zero, TimeSpan.FromSeconds(1.0))
            {
                m_Target = target;
                m_EndTime = DateTime.UtcNow + duration;

                m_OriginalStrength = target.Str; // Save the original strength
                m_StrengthBonus = (int)(target.Str * 0.15); // Calculate a 15% strength increase

                target.Str += m_StrengthBonus; // Apply the strength bonus
                target.Delta(MobileDelta.Stat); // Notify the server of the stat change

                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (DateTime.UtcNow >= m_EndTime || m_Target.Deleted || !m_Target.Alive)
                {
                    BuffInfo.RemoveBuff(m_Target, BuffIcon.Berserk);
                    m_Target.SendMessage(0x22, "The mark of the berserker fades away.");

                    // Revert the strength back to the original value
                    m_Target.Str = m_OriginalStrength;
                    m_Target.Delta(MobileDelta.Stat); // Notify the server of the stat change

                    Stop();
                }
            }
        }
    }
}
