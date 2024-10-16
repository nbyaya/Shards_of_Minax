using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.ACC.CSS.Systems.AnimalLoreMagic
{
    public class PredatorsFocus : AnimalLoreSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Predator’s Focus", "Fero Cerva",
            21005,
            9400
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 0.0; } }  // Assuming no required skill to cast this
        public override int RequiredMana { get { return 10; } }

        private static readonly int SkillBonus = 20;
        private static readonly TimeSpan Duration = TimeSpan.FromSeconds(30.0);

        public PredatorsFocus(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (Caster == null)
                return;

            if (CheckSequence())
            {
                Caster.PlaySound(0x44B); // Play a mystical sound
                Caster.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist); // Apply a glowing effect

                SkillMod trackingMod = new DefaultSkillMod(SkillName.Tracking, true, SkillBonus);
                Caster.AddSkillMod(trackingMod);

                Timer.DelayCall(Duration, () =>
                {
                    Caster.RemoveSkillMod(trackingMod);
                    Caster.PlaySound(0x1F8); // Play a sound indicating the effect has worn off
                    Caster.FixedParticles(0x3735, 1, 15, 1109, EffectLayer.Waist); // End effect visual
                });
            }

            FinishSequence();
        }
    }
}
