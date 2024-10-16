using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using System.Collections;

namespace Server.ACC.CSS.Systems.AnimalTamingMagic
{
    public class TrackPrey : AnimalTamingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Track Prey", "In Wis Lora",
            // You can replace with appropriate SpellCircle if required
            21004,
            9300,
            false
        );

        public override SpellCircle Circle => SpellCircle.First;

        public override double CastDelay => 0.1;
        public override double RequiredSkill => 0.0;
        public override int RequiredMana => 20;

        public TrackPrey(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Mobile caster = Caster;
                
                if (caster == null || caster.Deleted)
                    return;

                caster.SendMessage("You focus your senses, trying to track your prey.");
                caster.PlaySound(0x2CE); // Example sound for spell casting
                caster.FixedParticles(0x375A, 10, 15, 5037, EffectLayer.Head); // Example visual effect

                // Temporarily increase Tracking skill by 20 points
                Skill tracking = caster.Skills[SkillName.Tracking];

                if (tracking != null)
                {
                    tracking.Base += 20; // Increase skill

                    // Start a timer to revert the skill after a duration
                    Timer.DelayCall(TimeSpan.FromSeconds(30), () =>
                    {
                        if (caster != null && !caster.Deleted && tracking.Base > 0)
                        {
                            tracking.Base -= 20;
                            caster.SendMessage("The effects of Track Prey wear off.");
                        }
                    });
                }

                // Additional flashy effects
                caster.FixedParticles(0x373A, 10, 15, 5012, EffectLayer.Waist);
                caster.PlaySound(0x1FB);

                FinishSequence();
            }
        }
    }
}
