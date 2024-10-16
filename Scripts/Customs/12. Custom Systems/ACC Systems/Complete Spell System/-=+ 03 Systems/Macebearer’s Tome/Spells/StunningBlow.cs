using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.MacingMagic
{
    public class StunningBlow : MacingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Stunning Blow", "Stupor Maximus",
            21004, // Animation
            9300,  // Effect
            false, // Resistable
            Reagent.BlackPearl, // Example reagents if needed
            Reagent.MandrakeRoot
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 20; } }

        public StunningBlow(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private StunningBlow m_Owner;

            public InternalTarget(StunningBlow owner) : base(10, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile target)
                {
                    if (!from.CanBeHarmful(target))
                    {
                        from.SendLocalizedMessage(1060508); // Cannot harm your target.
                    }
                    else if (m_Owner.CheckSequence())
                    {
                        from.DoHarmful(target);
                        SpellHelper.Turn(from, target);

                        // Apply stunning effect
                        target.Freeze(TimeSpan.FromSeconds(5.0)); // Stun for 5 seconds
                        target.SendMessage("You have been stunned!");

                        // Apply visual effects
                        Effects.SendTargetParticles(target, 0x374A, 10, 15, 5029, EffectLayer.Waist);
                        target.PlaySound(0x213);

                        // Additional damage or effects can be added here

                        // We finish the sequence
                        m_Owner.FinishSequence();
                    }
                }
                else
                {
                    from.SendLocalizedMessage(1060508); // Cannot harm your target.
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0);
        }
    }
}
