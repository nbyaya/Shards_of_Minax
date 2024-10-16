using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.AnimalTamingMagic
{
    public class InstinctiveStrike : AnimalTamingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Instinctive Strike", "Ex Mori Venenum",
                                                        // SpellCircle.Fifth,
                                                        21004,
                                                        9300
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fifth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 70.0; } }
        public override int RequiredMana { get { return 20; } }

        public InstinctiveStrike(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            // Apply poison to all targets within 5 tiles
            if (CheckSequence())
            {
                Effects.PlaySound(Caster.Location, Caster.Map, 0x20E); // Sound for the ability

                List<Mobile> targets = new List<Mobile>();

                foreach (Mobile m in Caster.GetMobilesInRange(5))
                {
                    if (m != Caster && Caster.CanBeHarmful(m, false))
                    {
                        targets.Add(m);
                    }
                }

                foreach (Mobile target in targets)
                {
                    Caster.DoHarmful(target);

                    // Randomly select a poison level
                    Poison randomPoison = Poison.GetPoison(Utility.Random(1, 4)); // Random poison level 1 to 4
                    target.ApplyPoison(Caster, randomPoison);

                    // Display effects on the target
                    target.FixedParticles(0x374A, 10, 30, 5052, EffectLayer.Waist); // Green poison effect
                    target.PlaySound(0x474); // Poison sound effect
                }
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0);
        }
    }
}
