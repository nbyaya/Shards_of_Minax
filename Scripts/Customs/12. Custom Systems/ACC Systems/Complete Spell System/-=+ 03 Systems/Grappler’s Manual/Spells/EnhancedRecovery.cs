using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.WrestlingMagic
{
    public class EnhancedRecovery : WrestlingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Enhanced Recovery", "Sanus Recovitus",
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.2; } } // Casting delay of 1.5 seconds
        public override double RequiredSkill { get { return 0.0; } } // No specific skill requirement
        public override int RequiredMana { get { return 15; } } // Mana cost of 15

        public EnhancedRecovery(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Effect: Increase Healing skill by 25 points temporarily
                Caster.SendMessage("You feel a surge of healing power!");

                // Apply visual and sound effects
                Effects.SendLocationParticles(
                    EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration),
                    0x373A,  // Visual effect
                    1,       // Speed
                    15,      // Duration
                    1153,    // Hue
                    3,       // Render mode
                    9912,    // Unknown
                    0        // Unknown
                );

                Caster.PlaySound(0x5C2); // Play a sound effect

                // Temporarily boost Healing skill using DefaultSkillMod
                DefaultSkillMod healingBoost = new DefaultSkillMod(SkillName.Healing, true, 25.0);
                Caster.AddSkillMod(healingBoost);

                // Start a timer to remove the skill boost after a short period (10 seconds)
                Timer.DelayCall(TimeSpan.FromSeconds(10), () =>
                {
                    Caster.RemoveSkillMod(healingBoost);
                    Caster.SendMessage("The enhanced healing effect has worn off.");
                });
            }

            FinishSequence();
        }
    }
}
