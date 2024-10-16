using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.ForensicsMagic
{
    public class TraceAnalysis : ForensicsSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Trace Analysis", "Analyse Tracis",
            21004, // Icon ID
            9300,  // Cast sound
            false,
            Reagent.BlackPearl,
            Reagent.Bloodmoss
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 60.0; } }  // Example required skill level
        public override int RequiredMana { get { return 20; } }

        public TraceAnalysis(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.SendMessage("You focus your senses to analyze the traces around you...");

                // Add a temporary tracking skill boost
                Caster.Skills[SkillName.Tracking].Base += 25;
                Caster.FixedParticles(0x376A, 9, 32, 5005, EffectLayer.Waist); // Visual effect
                Caster.PlaySound(0x213); // Sound effect

                // Timer to revert the skill boost after a short duration
                Timer.DelayCall(TimeSpan.FromSeconds(30), () =>
                {
                    Caster.Skills[SkillName.Tracking].Base -= 25;
                    Caster.SendMessage("The effects of Trace Analysis fade away.");
                    Caster.FixedParticles(0x3735, 1, 15, 9909, 32, 2, EffectLayer.Head); // Visual effect when buff ends
                    Caster.PlaySound(0x1F8); // Sound effect when buff ends
                });
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.5); // Adjusted for balance
        }
    }
}
