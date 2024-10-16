using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.BeggingMagic
{
    public class PleadForMercy : BeggingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Plead for Mercy", "An exclamation of desperation",
            21004, // Gump ID for the spell icon
            9300   // Hue for the spell effect
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 16; } }

        public PleadForMercy(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist); // Particle effect
                Caster.PlaySound(0x1FB); // Sound effect

                List<Mobile> targets = new List<Mobile>();

                foreach (Mobile m in Caster.GetMobilesInRange(5))
                {
                    if (Caster.CanBeHarmful(m, false) && m != Caster)
                        targets.Add(m);
                }

                foreach (Mobile m in targets)
                {
                    if (m is BaseCreature)
                    {
                        Caster.DoHarmful(m);
                        m.Paralyze(TimeSpan.FromSeconds(5.0 + (Caster.Skills[SkillName.Meditation].Value / 10.0)));
                        m.FixedEffect(0x376A, 10, 16); // Paralyze visual effect
                        m.PlaySound(0x204); // Paralyze sound effect
                    }
                }
            }

            FinishSequence();
        }
    }
}
