using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using System.Collections.Generic;
using Server.Items;

namespace Server.ACC.CSS.Systems.WrestlingMagic
{
    public class SweepingKick : WrestlingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Sweeping Kick", "Sailor Kick!",
            21004, // Effect sound ID
            9300 // Animation ID
        );

        public override SpellCircle Circle { get { return SpellCircle.First; } } // Define the spell circle as First for simplicity
        public override double CastDelay { get { return 0.1; } } // 1 second cast delay
        public override double RequiredSkill { get { return 20.0; } } // Requires 20 skill level
        public override int RequiredMana { get { return 15; } } // Requires 15 mana

        public SweepingKick(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Get all mobiles within 1 tile of the caster
                List<Mobile> targets = new List<Mobile>();

                foreach (Mobile m in Caster.GetMobilesInRange(1))
                {
                    if (m != Caster && Caster.CanBeHarmful(m, false))
                    {
                        targets.Add(m);
                    }
                }

                // Apply paralyze effect and play visual and sound effects
                foreach (Mobile target in targets)
                {
                    Caster.DoHarmful(target);

                    // Paralyze the target for 4 seconds
                    target.Paralyze(TimeSpan.FromSeconds(4.0));

                    // Add a visual effect (dust cloud around feet) and play a sound
                    Effects.SendLocationParticles(EffectItem.Create(target.Location, target.Map, EffectItem.DefaultDuration), 0x36BD, 20, 10, 5044);
                    target.PlaySound(0x1FB);
                }

                // Play a visual effect on the caster to show the sweeping kick
                Effects.SendLocationParticles(EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration), 0x3728, 10, 15, 5044);
                Caster.PlaySound(21004); // Play a sweeping kick sound

                FinishSequence();
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.0); // Casting delay of 1 second
        }
    }
}
