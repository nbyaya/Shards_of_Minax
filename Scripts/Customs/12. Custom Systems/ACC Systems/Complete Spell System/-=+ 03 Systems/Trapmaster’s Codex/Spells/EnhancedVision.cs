using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.ACC.CSS.Systems.RemoveTrapMagic
{
    public class EnhancedVision : RemoveTrapSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Enhanced Vision", "In Vas Wis",
            21001, // Icon
            9200,  // Cast sound
            false,
            Reagent.BlackPearl,
            Reagent.Bloodmoss,
            Reagent.SpidersSilk
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 40.0; } }
        public override int RequiredMana { get { return 10; } }

        public EnhancedVision(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.SendMessage("You feel a surge of awareness flowing through your senses.");
                Caster.FixedParticles(0x375A, 10, 15, 5018, EffectLayer.Waist); // Visual effect
                Caster.PlaySound(0x1F2); // Sound effect

                EnhancedVisionEffect effect = new EnhancedVisionEffect(Caster);
                effect.Start();
            }

            FinishSequence();
        }

        private class EnhancedVisionEffect
        {
            private Mobile m_Caster;
            private Timer m_Timer;
            private SkillMod m_SkillMod;

            public EnhancedVisionEffect(Mobile caster)
            {
                m_Caster = caster;
                m_Timer = new InternalTimer(this, TimeSpan.FromSeconds(30)); // Duration of the effect
            }

            public void Start()
            {
                m_Caster.BeginAction(typeof(EnhancedVisionEffect));
                m_Timer.Start();
                m_Caster.SendMessage("Your vision sharpens, allowing you to detect hidden traps more effectively!");

                // Create and apply the skill mod
                m_SkillMod = new DefaultSkillMod(SkillName.Tracking, true, 20);
                m_Caster.AddSkillMod(m_SkillMod);
            }

            private void Stop()
            {
                // Remove the skill mod
                if (m_SkillMod != null)
                {
                    m_Caster.RemoveSkillMod(m_SkillMod);
                    m_SkillMod = null;
                }

                m_Caster.EndAction(typeof(EnhancedVisionEffect));
                m_Caster.SendMessage("Your vision returns to normal.");
            }

            private class InternalTimer : Timer
            {
                private EnhancedVisionEffect m_Effect;

                public InternalTimer(EnhancedVisionEffect effect, TimeSpan duration) : base(duration)
                {
                    m_Effect = effect;
                    Priority = TimerPriority.TwoFiftyMS;
                }

                protected override void OnTick()
                {
                    m_Effect.Stop();
                }
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(7.5);
        }
    }
}
