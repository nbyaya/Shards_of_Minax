using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.ACC.CSS.Systems.FencingMagic
{
    public class FencingMastery : FencingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Fencing Mastery", "Fencing Power",
            // SpellCircle.Third, (Commented out as no circle is defined here)
            21005,
            9301
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 20; } }

        public FencingMastery(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Mobile caster = Caster;
                int skillBoost = 20; // Boost fencing skill by 20 points
                double duration = 30.0; // Duration of the skill boost in seconds

                // Boost the fencing skill temporarily
                caster.SendMessage("You feel a surge of fencing mastery flow through you!");
                Effects.PlaySound(caster.Location, caster.Map, 0x64B); // Play sound effect
                caster.FixedParticles(0x376A, 9, 32, 5005, EffectLayer.Waist); // Play visual effect

                SkillMod mod = new DefaultSkillMod(SkillName.Fencing, true, skillBoost);
                caster.AddSkillMod(mod);

                // Timer to remove the skill mod after the duration
                Timer.DelayCall(TimeSpan.FromSeconds(duration), () =>
                {
                    caster.RemoveSkillMod(mod);
                    caster.SendMessage("The surge of fencing mastery fades away.");
                    Effects.PlaySound(caster.Location, caster.Map, 0x64C); // Play sound effect for skill boost end
                });
            }

            FinishSequence();
        }
    }
}
