using System;
using System.Collections.Generic; // Added this line
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.StealthMagic
{
    public class Camouflage : StealthSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Camouflage", "In Sanct An Wis",
                                                        21008,
                                                        9207
                                                       );

        private static TimeSpan CooldownPeriod = TimeSpan.FromMinutes(1.0); // Cooldown period
        private static Dictionary<Mobile, DateTime> LastCastTimes = new Dictionary<Mobile, DateTime>();

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 40.0; } }
        public override int RequiredMana { get { return 25; } }

        public Camouflage(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            DateTime now = DateTime.UtcNow;

            if (LastCastTimes.TryGetValue(Caster, out DateTime lastCastTime))
            {
                if (now - lastCastTime < CooldownPeriod)
                {
                    Caster.SendMessage("You must wait before using this spell again.");
                    return; // Exit if still in cooldown
                }
            }

            if (CheckSequence())
            {
                Caster.Hidden = true; // Hides the caster
                Caster.FixedParticles(0x376A, 10, 15, 5032, EffectLayer.Waist); // Visual effect
                Caster.PlaySound(0x1F3); // Sound effect

                new InternalTimer(Caster, TimeSpan.FromMinutes(1.0)).Start(); // Duration of 1 minute

                // Update the last cast time
                LastCastTimes[Caster] = now;
            }

            FinishSequence();
        }

        private class InternalTimer : Timer
        {
            private Mobile m_Caster;

            public InternalTimer(Mobile caster, TimeSpan duration) : base(duration)
            {
                m_Caster = caster;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Caster.Hidden)
                {
                    m_Caster.Hidden = false; // Reveals the caster after the duration ends
                    m_Caster.FixedParticles(0x373A, 10, 15, 5032, EffectLayer.Waist); // Ending visual effect
                    m_Caster.PlaySound(0x1F2); // Ending sound effect
                }
            }
        }
    }
}
