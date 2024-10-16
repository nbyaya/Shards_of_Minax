using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using System.Collections;

namespace Server.ACC.CSS.Systems.FencingMagic
{
    public class PrecisionParry : FencingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Precision Parry", "Deflectus Precise",
            21005,
            9301
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; } // Adjust the spell circle as needed
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 20.0; } }
        public override int RequiredMana { get { return 20; } }

        public PrecisionParry(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Buff the caster's parry skill temporarily
                Caster.SendMessage("You feel a surge of precision in your parrying ability!");
                Caster.PlaySound(0x1ED); // Play sound effect
                Caster.FixedParticles(0x376A, 10, 15, 5018, EffectLayer.Waist); // Play particle effect
                
                // Increase Parry skill by 20
                BuffInfo.AddBuff(Caster, new BuffInfo(BuffIcon.Paralyze, 1075834, 1075835, TimeSpan.FromSeconds(10), Caster, "Parry +20"));

                // Apply temporary skill increase
                Caster.Skills[SkillName.Parry].Base += 20;

                // Timer to revert the skill after 10 seconds
                Timer.DelayCall(TimeSpan.FromSeconds(10), () => EndPrecisionParry(Caster));
            }

            FinishSequence();
        }

        private void EndPrecisionParry(Mobile caster)
        {
            if (caster != null && !caster.Deleted)
            {
                caster.Skills[SkillName.Parry].Base -= 20; // Revert Parry skill increase
                caster.SendMessage("The surge of precision fades away.");
                caster.PlaySound(0x1F8); // Play sound effect to signal end
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.5); // Adjust cast delay as needed
        }
    }
}
