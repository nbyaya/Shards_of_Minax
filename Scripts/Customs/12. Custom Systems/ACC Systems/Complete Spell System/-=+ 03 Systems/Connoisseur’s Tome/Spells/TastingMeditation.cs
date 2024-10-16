using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.TasteIDMagic
{
    public class TastingMeditation : TasteIDSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Tasting Meditation", "Regenero Vitae et Vis",
                                                        21013, // Effect ID
                                                        9309   // Sound ID
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 15; } }

        public TastingMeditation(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Head); // Visual effect around caster
                Caster.PlaySound(0x5C3); // Meditation sound effect

                Caster.SendMessage("You enter a state of deep meditation, feeling your energy and vitality regenerate.");

                double regenBoost = 0.5 + (Caster.Skills[CastSkill].Value / 100.0); // Boost factor for regen, scaling with skill
                int duration = 30; // Duration in seconds

                Effects.SendTargetParticles(Caster, 0x375A, 9, 32, 5008, EffectLayer.Waist);

                // Apply a regeneration effect to health and mana
                Timer healthRegenTimer = new HealthRegenTimer(Caster, regenBoost, duration);
                healthRegenTimer.Start();

                Timer manaRegenTimer = new ManaRegenTimer(Caster, regenBoost, duration);
                manaRegenTimer.Start();
                
                Timer.DelayCall(TimeSpan.FromSeconds(duration), () => EndEffect(Caster)); // End effect after duration
            }

            FinishSequence();
        }

        private void EndEffect(Mobile caster)
        {
            caster.SendMessage("The effects of your meditation wear off.");
            caster.FixedParticles(0x375A, 9, 32, 5008, EffectLayer.Waist); // End visual effect
            caster.PlaySound(0x64B); // End sound effect
        }

        private class HealthRegenTimer : Timer
        {
            private Mobile m_Mobile;
            private double m_RegenBoost;
            private int m_Duration;

            public HealthRegenTimer(Mobile mobile, double regenBoost, int duration)
                : base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0))
            {
                m_Mobile = mobile;
                m_RegenBoost = regenBoost;
                m_Duration = duration;
            }

            protected override void OnTick()
            {
                if (m_Mobile != null && m_Mobile.Alive)
                {
                    m_Mobile.Hits = Math.Min(m_Mobile.HitsMax, m_Mobile.Hits + (int)(m_RegenBoost * 10));
                }
            }
        }

        private class ManaRegenTimer : Timer
        {
            private Mobile m_Mobile;
            private double m_RegenBoost;
            private int m_Duration;

            public ManaRegenTimer(Mobile mobile, double regenBoost, int duration)
                : base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0))
            {
                m_Mobile = mobile;
                m_RegenBoost = regenBoost;
                m_Duration = duration;
            }

            protected override void OnTick()
            {
                if (m_Mobile != null && m_Mobile.Alive)
                {
                    m_Mobile.Mana = Math.Min(m_Mobile.ManaMax, m_Mobile.Mana + (int)(m_RegenBoost * 10));
                }
            }
        }
    }
}
