using System;
using System.Collections;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Items;

namespace Server.ACC.CSS.Systems.ForensicsMagic
{
    public class TraumaticSurge : ForensicsSpell
    {
        private static readonly SpellInfo m_Info = new SpellInfo(
            "Traumatic Surge", "Paralizio!",
            21004, 9300
        );

        public override SpellCircle Circle => SpellCircle.Sixth; // Example, adjust as needed

        public override double CastDelay => 0.1; // Casting delay
        public override double RequiredSkill => 80.0; // Required skill level
        public override int RequiredMana => 30; // Mana cost

        public TraumaticSurge(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                ArrayList targets = new ArrayList();

                // Collect all creatures within 5-tile radius
                foreach (Mobile m in Caster.GetMobilesInRange(5))
                {
                    if (m != Caster && m.Alive && Caster.CanBeHarmful(m, false))
                    {
                        targets.Add(m);
                    }
                }

                if (targets.Count > 0)
                {
                    Effects.PlaySound(Caster.Location, Caster.Map, 0x204); // Play a magical sound
                    Caster.FixedParticles(0x3779, 1, 15, 9950, 92, 3, EffectLayer.Head); // Visual effect

                    foreach (Mobile m in targets)
                    {
                        Caster.DoHarmful(m);
                        m.Paralyze(TimeSpan.FromSeconds(1.0)); // Paralyze for 1 second
                        m.FixedParticles(0x3779, 1, 15, 9950, 92, 3, EffectLayer.Head); // Individual visual effect on each target
                    }
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
