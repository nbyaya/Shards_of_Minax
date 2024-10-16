using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using Server;

namespace Server.ACC.CSS.Systems.MeditationMagic
{
    public class FocusBeam : MeditationSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Focus Beam", "Ex Por Grav",
            21004, // Placeholder graphic ID for spell animation
            9300   // Placeholder sound ID for spell sound
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; } // Adjust as needed
        }

        public override double CastDelay { get { return 0.2; } } // Delay before casting
        public override double RequiredSkill { get { return 50.0; } } // Required Meditation skill level
        public override int RequiredMana { get { return 20; } } // Mana cost

        public FocusBeam(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private FocusBeam m_Owner;

            public InternalTarget(FocusBeam owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile target)
                {
                    if (from.CanBeHarmful(target))
                    {
                        from.DoHarmful(target);
                        m_Owner.Target(target);
                    }
                }
            }
        }

        public void Target(Mobile target)
        {
            if (CheckSequence())
            {
                // Visual and sound effects for the spell
                Caster.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head); // Blue beam visual
                Caster.PlaySound(0x208); // Sound effect for casting

                // Calculate damage based on Meditation skill level
                double meditationSkill = Caster.Skills[SkillName.Meditation].Value;
                double damage = Utility.RandomMinMax(10, 20) + (meditationSkill * 0.2);

                // Deal damage to the target
                AOS.Damage(target, Caster, (int)damage, 100, 0, 0, 0, 0); // Pure energy damage

                // Additional visual effect on the target
                target.FixedParticles(0x374A, 10, 15, 5021, EffectLayer.Waist); // Target hit visual
                target.PlaySound(0x1FB); // Target hit sound
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0);
        }
    }
}
