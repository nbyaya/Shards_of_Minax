using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.SkillHandlers;
using Server.Items;

namespace Server.ACC.CSS.Systems.ParryMagic
{
    public class EmergencyEvasion : ParrySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Emergency Evasion", "Escape",
            // This is the index for the spell in the SpellInfo table
            21005,
            9301
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override int RequiredMana { get { return 30; } }
        public override double RequiredSkill { get { return 0.0; } } // No specific skill requirement for this ability
        public override double CastDelay { get { return 0.1; } } // 1-second cast delay

        public EmergencyEvasion(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.PlaySound(0x5C1); // Play a swift whooshing sound
                Caster.FixedParticles(0x375A, 10, 15, 5018, EffectLayer.Waist); // Visual effect for ability activation

                // Apply the +100 Parry skill bonus
                Caster.SendMessage("You feel more agile!");
                SkillMod parryMod = new DefaultSkillMod(SkillName.Parry, true, 100.0);
                Caster.AddSkillMod(parryMod);

                Timer.DelayCall(TimeSpan.FromSeconds(3.0), () =>
                {
                    Caster.SendMessage("Your agility boost has faded.");
                    Caster.RemoveSkillMod(parryMod); // Remove the skill mod after 3 seconds

                    // Additional visual and sound effect for when the ability ends
                    Caster.PlaySound(0x1F8); // Play a different sound to indicate the effect ended
                    Caster.FixedParticles(0x373A, 10, 15, 5012, EffectLayer.Waist);
                });
            }

            FinishSequence();
        }
    }
}
