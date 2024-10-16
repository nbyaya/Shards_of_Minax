using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;


namespace Server.ACC.CSS.Systems.AnimalTamingMagic
{
    public class WildHarmony : AnimalTamingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Wild Harmony", "Anu Hono",
            // SpellCircle.Seventh, // Uncomment if you use spell circles
            21004, // Icon
            9300 // Cast animation
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Seventh; } // Assuming it's a Seventh circle spell; adjust as needed
        }

        public override double CastDelay { get { return 2.5; } }
        public override double RequiredSkill { get { return 80.0; } }
        public override int RequiredMana { get { return 20; } }

        public WildHarmony(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Play casting effects and sound
                Caster.PlaySound(0x1FB); // Custom sound for the spell casting
                Caster.FixedParticles(0x374A, 10, 15, 5037, EffectLayer.Head); // Custom casting visual

                // Get targets in range
                List<Mobile> targets = new List<Mobile>();
                IPooledEnumerable eable = Caster.GetMobilesInRange(5);
                foreach (Mobile m in eable)
                {
                    if (m is BaseCreature && Caster.CanBeHarmful(m, false))
                        targets.Add(m);
                }
                eable.Free();

                // Paralyze all targets
                foreach (Mobile target in targets)
                {
                    Caster.DoHarmful(target);
                    target.Paralyze(TimeSpan.FromSeconds(5.0)); // Paralyze for 5 seconds
                    target.FixedEffect(0x376A, 1, 16, 1153, 3); // Paralyze visual effect
                    target.PlaySound(0x204); // Custom sound for being paralyzed
                }
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.0); // Adjust casting delay as needed
        }
    }
}
