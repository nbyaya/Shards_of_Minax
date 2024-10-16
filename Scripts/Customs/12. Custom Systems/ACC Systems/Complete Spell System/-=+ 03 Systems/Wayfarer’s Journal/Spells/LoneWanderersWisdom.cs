using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using System.Collections;

namespace Server.ACC.CSS.Systems.CampingMagic
{
    public class LoneWanderersWisdom : CampingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Lone Wanderer's Wisdom", "Vita Manus",
            // SpellCircle.Third,
            21005,
            9301,
            false
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 40; } }

        public LoneWanderersWisdom(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.SendMessage("You feel the wisdom of the Lone Wanderer flowing through you...");

                // Apply temporary skill boost
                double skillBoost = 10.0; // Increase by 10 skill points
                TimeSpan duration = TimeSpan.FromMinutes(5);
                BuffSkills(Caster, skillBoost, duration);

                // Play visual and sound effects
                Effects.SendTargetParticles(Caster, 0x373A, 10, 15, 5018, EffectLayer.Waist);
                Caster.PlaySound(0x1F7); // Play a mystical sound

                // Apply cooldown
                Timer.DelayCall(TimeSpan.FromMinutes(20), () =>
                {
                    Caster.SendMessage("Lone Wanderer's Wisdom is ready to be used again.");
                });

                FinishSequence();
            }
        }

        private void BuffSkills(Mobile caster, double amount, TimeSpan duration)
        {
            ArrayList skills = new ArrayList()
            {
                SkillName.Archery,
                SkillName.Forensics,
                SkillName.Tracking
            };

            foreach (SkillName skillName in skills)
            {
                Skill skill = caster.Skills[skillName];
                if (skill != null)
                {
                    skill.Base += amount; // Apply the skill boost

                    // Schedule a timer to remove the buff after duration
                    Timer.DelayCall(duration, () =>
                    {
                        skill.Base -= amount;
                        caster.SendMessage($"{skillName} boost has worn off.");
                    });
                }
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0);
        }
    }
}
