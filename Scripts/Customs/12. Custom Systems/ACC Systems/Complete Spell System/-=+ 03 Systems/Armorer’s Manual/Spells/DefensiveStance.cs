using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using System.Collections;

namespace Server.ACC.CSS.Systems.ArmsLoreMagic
{
    public class DefensiveStance : ArmsLoreSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Defensive Stance", "Immaune to damage for a short period",
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; } // Adjust based on your system's spell circles
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 25; } }

        private static readonly TimeSpan ImmunityDuration = TimeSpan.FromSeconds(5.0); // 5 seconds of immunity

        public DefensiveStance(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.SendMessage("You brace yourself and enter a defensive stance!");

                // Visual effects
                Caster.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist); // Visual shield effect
                Caster.PlaySound(0x1ED); // Play a defensive sound

                // Apply immunity
                Caster.Blessed = true;

                // Start a timer to remove the immunity after the duration
                Timer.DelayCall(ImmunityDuration, () =>
                {
                    Caster.Blessed = false;
                    Caster.SendMessage("Your defensive stance fades, leaving you vulnerable again.");
                });
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.5);
        }
    }
}
