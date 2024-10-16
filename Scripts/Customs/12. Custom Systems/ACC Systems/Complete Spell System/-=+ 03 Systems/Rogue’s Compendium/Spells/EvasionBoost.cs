using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.StealingMagic
{
    public class EvasionBoost : StealingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Evasion Boost", "Evaso Boostio",
                                                        21004,
                                                        9300
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 20.0; } }
        public override int RequiredMana { get { return 10; } }

        public EvasionBoost(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Temporary dodge and evasion boost
                TimeSpan duration = TimeSpan.FromSeconds(10 + Caster.Skills[CastSkill].Value / 2); // Duration increases with skill level
                double evasionBonus = 0.2 + (Caster.Skills[CastSkill].Value / 200); // Evasion bonus increases with skill level

                Caster.SendMessage("You feel nimble and light on your feet!");

                // Apply visual and sound effects
                Caster.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist); // Sparkle effect around caster
                Caster.PlaySound(0x1F7); // Evasion boost sound

                // Add skill modifiers for evasion and dodge
                SkillMod evasionMod = new DefaultSkillMod(SkillName.Stealing, true, evasionBonus);
                SkillMod dodgeMod = new DefaultSkillMod(SkillName.Begging, true, evasionBonus);

                Caster.AddSkillMod(evasionMod);
                Caster.AddSkillMod(dodgeMod);

                Timer.DelayCall(duration, () =>
                {
                    Caster.SendMessage("Your enhanced evasion fades away.");
                    Caster.RemoveSkillMod(evasionMod);
                    Caster.RemoveSkillMod(dodgeMod);

                    // Ending visual and sound effects
                    Caster.FixedParticles(0x3735, 1, 30, 9950, EffectLayer.Waist); // Fading effect around caster
                    Caster.PlaySound(0x1F8); // Sound indicating effect has ended
                });
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0);
        }
    }
}
