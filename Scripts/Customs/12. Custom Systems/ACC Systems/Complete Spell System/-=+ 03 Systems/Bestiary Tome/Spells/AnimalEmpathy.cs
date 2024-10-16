using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.AnimalLoreMagic
{
    public class AnimalEmpathy : AnimalLoreSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Animal Empathy", "Pacifto Beastus",
            //SpellCircle.Fourth,
            21005,
            9400
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 12; } }

        public AnimalEmpathy(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                try
                {
                    // Play visual effects and sound
                    Caster.FixedParticles(0x373A, 10, 30, 5013, EffectLayer.Head);
                    Caster.PlaySound(0x213);

                    List<Mobile> toParalyze = new List<Mobile>();

                    // Find all creatures within 3 tiles
                    foreach (Mobile m in Caster.GetMobilesInRange(3))
                    {
                        if (m != Caster && m is BaseCreature && Caster.CanBeHarmful(m, false))
                        {
                            toParalyze.Add(m);
                        }
                    }

                    // Paralyze each creature
                    foreach (Mobile m in toParalyze)
                    {
                        Caster.DoHarmful(m);
                        m.Paralyze(TimeSpan.FromSeconds(10.0));

                        // Add visual effects to paralyzed creatures
                        m.FixedEffect(0x376A, 10, 16);
                        m.PlaySound(0x204);
                    }
                }
                catch
                {
                }
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(5.0);
        }
    }
}
