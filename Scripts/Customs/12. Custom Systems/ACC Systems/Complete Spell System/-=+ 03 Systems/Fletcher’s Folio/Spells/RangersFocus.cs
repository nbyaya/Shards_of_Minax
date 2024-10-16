using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;
using System.Collections.Generic;


namespace Server.ACC.CSS.Systems.FletchingMagic
{
    public class RangersFocus : FletchingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Rangers Focus", "Focus Arcanum",
                                                        21005,
                                                        9400
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 15; } }

        public RangersFocus(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.PlaySound(0x20E); // Play a mystical sound effect
                Caster.FixedParticles(0x375A, 10, 30, 5037, EffectLayer.Head); // Visual effect around caster's head

                double duration = 10.0 + (Caster.Skills[CastSkill].Value * 0.1); // Duration based on caster's skill

                // Apply the boost effect
                Caster.SendMessage("Your focus sharpens, enhancing your critical strike chance and damage!");

                // Add custom buff
                new RangersFocusBuff(Caster, TimeSpan.FromSeconds(duration));

                // End the sequence
                FinishSequence();
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(3.0); // Reduced cast delay for faster usability
        }

        private class RangersFocusBuff
        {
            private Timer m_Timer;
            private Mobile m_Caster;
            private TimeSpan m_Duration;

            public RangersFocusBuff(Mobile caster, TimeSpan duration)
            {
                m_Caster = caster;
                m_Duration = duration;

                // Send message to the caster
                m_Caster.SendMessage("You are now under the effect of Ranger's Focus.");

                // Start a timer for the buff duration
                m_Timer = Timer.DelayCall(TimeSpan.Zero, TimeSpan.FromSeconds(1.0), OnTick);
            }

            private void OnTick()
            {
                m_Duration -= TimeSpan.FromSeconds(1.0);

                if (m_Duration <= TimeSpan.Zero)
                {
                    StopBuff();
                    return;
                }

                // Apply periodic effects if needed here
            }

            public void StopBuff()
            {
                m_Caster.SendMessage("Your focus fades, and your concentration returns to normal.");
                m_Timer.Stop();
                // Perform additional cleanup if needed
            }
        }
    }
}
