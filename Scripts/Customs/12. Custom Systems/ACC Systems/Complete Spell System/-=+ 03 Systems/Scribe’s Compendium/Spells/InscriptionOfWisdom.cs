using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.InscribeMagic
{
    public class InscriptionOfWisdom : InscribeSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Inscription of Wisdom", "Wisdom Instilled",
            // SpellCircle.First, // This can be adjusted as per the system's circle definition
            21004,
            9300,
            false,
            Reagent.Bloodmoss,
            Reagent.MandrakeRoot
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 25; } }

        public InscriptionOfWisdom(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.PlaySound(0x1E9); // Sound effect for casting
                Caster.FixedParticles(0x375A, 10, 15, 5012, 1153, 2, EffectLayer.Head); // Visual effect

                int duration = 30 + (int)(Caster.Skills[SkillName.Magery].Value * 0.2); // Duration based on caster's skill level

                // Intelligence boost
                int intelBoost = (int)(Caster.Skills[SkillName.Inscribe].Value * 0.1);
                Caster.RawInt += intelBoost;
                Caster.SendMessage("You feel a surge of wisdom flow through you.");

                Timer.DelayCall(TimeSpan.FromSeconds(duration), () =>
                {
                    Caster.RawInt -= intelBoost; // Revert intelligence boost after duration
                    Caster.SendMessage("The surge of wisdom fades away.");
                });

                // Skill effectiveness boost
                double skillBoost = 0.1; // 10% skill effectiveness boost
                List<Skill> skillsToBoost = new List<Skill>
                {
                    Caster.Skills[SkillName.Magery],
                    Caster.Skills[SkillName.EvalInt],
                    Caster.Skills[SkillName.Meditation],
                    Caster.Skills[SkillName.Inscribe]
                };

                foreach (var skill in skillsToBoost)
                {
                    skill.Base += skill.Base * skillBoost;
                }

                Timer.DelayCall(TimeSpan.FromSeconds(duration), () =>
                {
                    foreach (var skill in skillsToBoost)
                    {
                        skill.Base -= skill.Base * skillBoost; // Revert skill boost after duration
                    }
                    Caster.SendMessage("Your enhanced skills return to normal.");
                });
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(5.0);
        }
    }
}
