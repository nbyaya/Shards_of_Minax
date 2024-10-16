using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections;

namespace Server.ACC.CSS.Systems.ParryMagic
{
    public class FortifiedDefense : ParrySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Fortified Defense", "For Defendo",
            21005, 9301, // Icon and cast sound
            false, // Doesn't require reagents
            Reagent.BlackPearl
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 25; } }

        public FortifiedDefense(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Apply visual and sound effects
                Caster.PlaySound(0x208); // Defensive spell sound
                Caster.FixedParticles(0x375A, 10, 15, 5037, EffectLayer.Waist); // Blue swirls effect

                // Apply parry skill boost
                double duration = Caster.Skills[SkillName.Parry].Value * 0.1; // Duration based on skill
                int boostAmount = 25;
                Caster.SendMessage("You feel your defenses strengthen!");

                Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
                {
                    Caster.Skills[SkillName.Parry].Base += boostAmount;
                    Caster.SendMessage("Your Parry skill has been temporarily increased by 25 points.");
                });

                // Reset the skill boost after duration
                Timer.DelayCall(TimeSpan.FromSeconds(duration), () =>
                {
                    if (Caster != null && !Caster.Deleted)
                    {
                        Caster.Skills[SkillName.Parry].Base -= boostAmount;
                        Caster.SendMessage("Your Parry skill boost has worn off.");
                        Caster.PlaySound(0x1F8); // Sound when effect ends
                    }
                });
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.5); // Slight delay before casting
        }
    }
}
