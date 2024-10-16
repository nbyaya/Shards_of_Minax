using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.ParryMagic
{
    public class ShieldBash : ParrySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Shield Bash", "Shield Bash", // Ability Name
            21005, // Sound ID
            9301 // Effect ID
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 50.0; } } // Adjust the skill requirement as needed
        public override int RequiredMana { get { return 20; } } // Mana Cost

        public ShieldBash(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private ShieldBash m_Owner;

            public InternalTarget(ShieldBash owner) : base(2, false, TargetFlags.Harmful) // Range of 2 tiles, harmful target
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile)
                {
                    Mobile target = (Mobile)targeted;

                    if (from.CanBeHarmful(target) && from.InRange(target, 2)) // Ensure the target is in range and can be harmed
                    {
                        from.DoHarmful(target);
                        m_Owner.ParalyzeTarget(target);
                    }
                    else
                    {
                        from.SendLocalizedMessage(500237); // Target is out of range or cannot be harmed
                    }
                }

                m_Owner.FinishSequence();
            }
        }

        private void ParalyzeTarget(Mobile target)
        {
            if (CheckSequence())
            {
                target.Paralyze(TimeSpan.FromSeconds(8)); // Paralyze the target for 8 seconds
                target.FixedParticles(0x376A, 10, 15, 5038, EffectLayer.Head); // Visual effect on the target
                target.PlaySound(0x1F7); // Play paralyze sound

                Caster.SendMessage("You perform a powerful Shield Bash, paralyzing your target!");
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.0); // Adjust cast delay as needed
        }
    }
}
