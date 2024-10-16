using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.DiscordanceMagic
{
    public class HarmonicConfusion : DiscordanceSpell
    {
        private static readonly SpellInfo m_Info = new SpellInfo(
            "Harmonic Confusion", "Resonantia Confunditur",
            21004, 9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Seventh; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }

        public HarmonicConfusion(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.SendMessage("You invoke the power of harmonic confusion!");

                // Visual and sound effects for the caster
                Caster.PlaySound(0x5C3); // Harmonic sound
                Caster.FixedParticles(0x374A, 10, 15, 5031, EffectLayer.Head);

                List<Mobile> targets = new List<Mobile>();

                // Get all mobiles within 3 tile radius
                foreach (Mobile m in Caster.GetMobilesInRange(3))
                {
                    if (m != Caster && m is BaseCreature) // Only affect creatures, not players
                    {
                        targets.Add(m);
                    }
                }

                // Apply effects to each target
                foreach (Mobile m in targets)
                {
                    // Apply visual effects to each target
                    m.FixedParticles(0x375A, 9, 20, 5027, EffectLayer.Waist);
                    m.PlaySound(0x5C9); // Confusion sound

                    // Temporarily bless the creature
                    m.Blessed = true;

                    Timer.DelayCall(TimeSpan.FromSeconds(3), () => RemoveBless(m));
                }
            }

            FinishSequence();
        }

        private void RemoveBless(Mobile m)
        {
            if (m != null && !m.Deleted)
            {
                m.Blessed = false;
                m.FixedParticles(0x374A, 10, 15, 5031, EffectLayer.Head); // Another visual effect for the end of blessing
                m.PlaySound(0x5C1); // End of blessing sound
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(5.0);
        }
    }
}
