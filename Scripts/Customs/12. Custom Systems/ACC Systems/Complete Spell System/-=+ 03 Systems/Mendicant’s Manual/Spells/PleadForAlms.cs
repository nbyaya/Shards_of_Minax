using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.BeggingMagic
{
    public class PleadForAlms : BeggingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Plead for Alms", "Dona Nobis Pacem",
            21004,
            9300
        );

        public override SpellCircle Circle => SpellCircle.First; // Assuming a low-level skill for "Begging"

        public override double CastDelay => 1.5; // Slight delay to cast
        public override double RequiredSkill => 0.0; // No skill required to use
        public override int RequiredMana => 10; // Mana cost of the spell

        public PleadForAlms(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Play visual effects
                Caster.FixedParticles(0x376A, 10, 15, 5013, 1153, 2, EffectLayer.Waist); // Shimmering light around the caster
                Caster.PlaySound(0x5A1); // Sound of a soft chime

                // Increase the Begging skill temporarily
                Skill beggingSkill = Caster.Skills[SkillName.Begging];

                if (beggingSkill != null)
                {
                    double originalValue = beggingSkill.Base;
                    beggingSkill.Base += 20.0; // Increase Begging skill by +20

                    Timer.DelayCall(TimeSpan.FromSeconds(30), () =>
                    {
                        // After 30 seconds, revert the skill back to its original value
                        beggingSkill.Base = originalValue;
                        Caster.SendMessage("Your plea for alms effect has worn off.");
                    });

                    Caster.SendMessage("You plead for alms, temporarily increasing your begging skill!");
                }
                else
                {
                    Caster.SendMessage("You do not have the Begging skill.");
                }
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.5); // Cast delay
        }
    }
}
