using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections;

namespace Server.ACC.CSS.Systems.ForensicsMagic
{
    public class TraumaInfliction : ForensicsSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Trauma Infliction", "Ro Sham Bo",
                                                        21004,
                                                        9300
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 30; } }

        public TraumaInfliction(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private TraumaInfliction m_Owner;

            public InternalTarget(TraumaInfliction owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile target)
                {
                    if (from.CanBeHarmful(target) && m_Owner.CheckSequence())
                    {
                        from.DoHarmful(target);

                        // Temporary reduction of strength by 90%
                        double originalStrength = target.Str;
                        double reducedStrength = originalStrength * 0.1;
                        target.Str = (int)reducedStrength;

                        // Visual effects
                        Effects.SendLocationEffect(target.Location, target.Map, 0x3709, 30, 10, 0, 0); // Earthquake effect
                        Effects.PlaySound(target.Location, target.Map, 0x2F3); // Sound effect

                        // Add a timer to restore the original strength
                        Timer.DelayCall(TimeSpan.FromSeconds(30.0), () => RestoreStrength(target, (int)originalStrength));

                        // Visual feedback on the target
                        target.FixedParticles(0x374A, 10, 15, 5032, EffectLayer.Waist);
                        target.PlaySound(0x1FA);
                    }
                }

                m_Owner.FinishSequence();
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }

            private static void RestoreStrength(Mobile target, int originalStrength)
            {
                if (target != null && !target.Deleted)
                {
                    target.Str = originalStrength;
                    target.FixedParticles(0x375A, 10, 15, 5032, EffectLayer.Waist); // Restoration visual effect
                    target.PlaySound(0x1FD); // Sound effect on restoration
                }
            }
        }
    }
}
