using System;
using System.Collections;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.SkillHandlers;
using Server.Items;

namespace Server.ACC.CSS.Systems.DiscordanceMagic
{
    public class DiscordantEchoes : DiscordanceSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Discordant Echoes", "Sonus Discordia",
            0, // Adjust this parameter as needed.
            0, // Adjust this parameter as needed.
            typeof(DiscordantEchoes) // Adjust this parameter to the required Type if needed
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Seventh; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 30; } }

        public DiscordantEchoes(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Mobile caster = Caster;

                if (caster != null && !Discordance.UnderEffects(caster))
                {
                    // Apply the temporary Discordance skill boost
                    int skillBoost = 25;
                    caster.Skills[SkillName.Discordance].Base += skillBoost;

                    // Display flashy visuals and play sound
                    caster.FixedParticles(0x376A, 1, 32, 9942, 1153, 3, EffectLayer.Waist); // Visual Effect
                    caster.PlaySound(0x5C4); // Sound Effect

                    // Timer to remove the skill boost after a duration
                    Timer.DelayCall(TimeSpan.FromSeconds(30), () =>
                    {
                        if (caster != null && caster.Skills[SkillName.Discordance].Base >= skillBoost)
                        {
                            caster.Skills[SkillName.Discordance].Base -= skillBoost;

                            // Another visual and sound effect for the end of the boost
                            caster.FixedParticles(0x374A, 10, 30, 5052, EffectLayer.Waist);
                            caster.PlaySound(0x1F8);
                        }
                    });

                    caster.SendMessage("You feel a surge of discordant energy flowing through you, enhancing your Discordance skill temporarily!");
                }
                else
                {
                    caster.SendMessage("The Discordant Echoes have no effect on you.");
                }
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(7.5);
        }
    }
}
