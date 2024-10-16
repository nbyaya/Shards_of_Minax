using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.BlacksmithMagic
{
    public class ForgeMastersBlessing : BlacksmithSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Forge Masters Blessing", "In Ben Rot",
            21020,
            9316
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 40; } }

        public ForgeMastersBlessing(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.SendMessage("You feel the power of the Forge Masters course through your veins!");
                Caster.FixedParticles(0x375A, 10, 30, 5037, EffectLayer.Waist); // Visual effect
                Caster.PlaySound(0x1F2); // Sound effect

                // Apply cosmetic effects
                BuffEffect buffEffect = new BuffEffect(Caster);
                buffEffect.Start();

                // Consume mana
                Caster.Mana -= RequiredMana;
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0);
        }

        private class BuffEffect
        {
            private Mobile m_Caster;
            private Timer m_Timer;
            private DateTime m_EndTime;

            public BuffEffect(Mobile caster)
            {
                m_Caster = caster;
                m_EndTime = DateTime.Now + TimeSpan.FromSeconds(30.0); // Effect lasts for 30 seconds
            }

            public void Start()
            {
                m_Timer = new BuffTimer(this);
                m_Timer.Start();
            }

            public void Stop()
            {
                if (m_Timer != null)
                    m_Timer.Stop();

                EndEffect();
            }

            private void EndEffect()
            {
                m_Caster.SendMessage("The blessing of the Forge Masters fades away.");
                m_Caster.FixedParticles(0x3735, 1, 30, 9932, EffectLayer.Head); // Ending visual effect
                m_Caster.PlaySound(0x1F8); // Ending sound effect
            }

            private class BuffTimer : Timer
            {
                private BuffEffect m_BuffEffect;

                public BuffTimer(BuffEffect effect) : base(TimeSpan.Zero, TimeSpan.FromSeconds(1.0))
                {
                    m_BuffEffect = effect;
                    Priority = TimerPriority.OneSecond;
                }

                protected override void OnTick()
                {
                    if (DateTime.Now >= m_BuffEffect.m_EndTime)
                    {
                        m_BuffEffect.Stop();
                        return;
                    }

                    // Apply cosmetic effects every second
                    if (Utility.RandomDouble() < 0.1)
                    {
                        m_BuffEffect.m_Caster.FixedEffect(0x375A, 1, 20, 1153, 3); // Intermittent glow effect
                    }
                }
            }
        }
    }
}
