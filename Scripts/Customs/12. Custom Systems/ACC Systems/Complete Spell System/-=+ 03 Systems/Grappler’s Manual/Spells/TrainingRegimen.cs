using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;
using Server;

namespace Server.ACC.CSS.Systems.WrestlingMagic
{
    public class TrainingRegimen : WrestlingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Training Regimen", "Teneo Strenui",
            21004, // Effect sound ID
            9300 // Effect animation ID
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.2; } } // 1.5 seconds delay before casting
        public override double RequiredSkill { get { return 50.0; } } // Skill required to cast
        public override int RequiredMana { get { return 15; } } // Mana cost

        public TrainingRegimen(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Visual and sound effects
                Caster.FixedParticles(0x375A, 1, 15, 9909, EffectLayer.Waist); // A burst of particles around the caster
                Caster.PlaySound(0x213); // Play sound effect

                // Temporarily increase Wrestling skill
                int bonus = 30;
                Caster.SendMessage("You feel your muscles surge with strength as your wrestling skill temporarily increases!");

                SkillMod mod = new DefaultSkillMod(SkillName.Wrestling, true, bonus);
                Caster.AddSkillMod(mod);

                // Duration of the effect (10 seconds)
                Timer.DelayCall(TimeSpan.FromSeconds(10), () =>
                {
                    // Remove the skill bonus after the duration
                    Caster.RemoveSkillMod(mod);
                    Caster.SendMessage("The effects of your Training Regimen wear off, and your wrestling skill returns to normal.");
                });
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.5); // Total casting delay
        }
    }
}
