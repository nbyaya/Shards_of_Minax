using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Misc;
using Server.Items;
using System.Collections;

namespace Server.ACC.CSS.Systems.MeditationMagic
{
    public class EnlightenedVision : MeditationSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Enlightened Vision", "Re Vel Veritas",
            //SpellCircle.Second,
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 20; } }

        public EnlightenedVision(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private EnlightenedVision m_Owner;

            public InternalTarget(EnlightenedVision owner) : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile target)
                {
                    if (m_Owner.CheckSequence())
                    {
                        Effects.PlaySound(target.Location, target.Map, 0x1ED); // Sound effect for casting

                        target.FixedParticles(0x375A, 10, 15, 5013, EffectLayer.Waist); // Particle effect on the target
                        target.SendMessage("You feel a sudden clarity in your vision.");

                        if (target is PlayerMobile pm)
                        {
                            pm.Hidden = false; // Remove hidden state if they are hidden
                        }

                        // Apply "Reveal Hidden" effect for a short time
                        Timer.DelayCall(TimeSpan.FromSeconds(30.0), () =>
                        {
                            target.SendMessage("The clarity in your vision fades.");
                        });

                        m_Owner.FinishSequence();
                    }
                }
                else
                {
                    from.SendLocalizedMessage(500237); // Target can not be seen.
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(5.0);
        }
    }
}
