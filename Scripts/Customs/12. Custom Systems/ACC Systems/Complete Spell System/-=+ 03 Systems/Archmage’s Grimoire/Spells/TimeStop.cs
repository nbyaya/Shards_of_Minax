using System;
using System.Collections;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Items;

namespace Server.ACC.CSS.Systems.MageryMagic
{
    public class TimeStop : MagerySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Time Stop", "Vas An Grav",
            21004,
            9300
        );

        public override SpellCircle Circle => SpellCircle.Seventh; // Assuming it's a high-level spell

        public override double CastDelay => 0.2;
        public override double RequiredSkill => 80.0;
        public override int RequiredMana => 50;

        public TimeStop(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            // Radius around the caster within which enemies will be paralyzed
            int radius = 8;

            // Check if the spell can be cast
            if (CheckSequence())
            {
                // Apply effects to all mobiles in range
                ArrayList targets = new ArrayList();

                foreach (Mobile m in Caster.GetMobilesInRange(radius))
                {
                    if (Caster.CanBeHarmful(m, false))
                    {
                        targets.Add(m);
                    }
                }

                // Play sound and visual effects at the caster's location
                Caster.PlaySound(0x213); // A magical sound effect
                Effects.SendLocationParticles(EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration), 0x376A, 1, 29, 1153, 4, 9952, 0); // Flashy visual effect

                // Paralyze each target in range
                foreach (Mobile m in targets)
                {
                    Caster.DoHarmful(m);
                    m.Paralyze(TimeSpan.FromSeconds(5.0 + Caster.Skills[SkillName.Magery].Value * 0.1)); // Duration scales with caster's skill

                    // Additional visual effect on each paralyzed target
                    m.FixedParticles(0x376A, 1, 29, 1153, 4, 9952, 0);
                    m.PlaySound(0x204); // Paralyze sound effect
                }
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(3.0);
        }
    }
}
