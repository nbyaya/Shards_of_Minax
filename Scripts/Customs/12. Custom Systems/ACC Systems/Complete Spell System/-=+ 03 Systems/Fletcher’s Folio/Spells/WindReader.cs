using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.FletchingMagic
{
    public class WindReader : FletchingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Wind Reader", "Ventus Lectio",
            21005, // Effect ID for visuals
            9400,  // Sound ID
            false,
            Reagent.BlackPearl
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 12; } }

        public WindReader(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private WindReader m_Owner;

            public InternalTarget(WindReader owner) : base(12, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                {
                    Mobile target = (Mobile)o;

                    if (m_Owner.CheckSequence())
                    {
                        // Applying the effect: Increased accuracy for a duration
                        target.SendMessage("You feel a surge of confidence as you read the wind currents!");

                        Effects.PlaySound(target.Location, target.Map, 0x64C); // Sound effect for casting


                        // Apply buff or effect
                        new WindReaderEffect(target).Start();

                        from.SendMessage("You have successfully cast Wind Reader on your target.");
                    }
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        private class WindReaderEffect : Timer
        {
            private Mobile m_Target;
            private DateTime m_End;

            public WindReaderEffect(Mobile target) : base(TimeSpan.Zero, TimeSpan.FromSeconds(1.0))
            {
                m_Target = target;
                m_End = DateTime.Now + TimeSpan.FromSeconds(30); // Effect lasts 30 seconds
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (DateTime.Now >= m_End || m_Target == null || m_Target.Deleted)
                {
                    Stop();
                    m_Target.SendMessage("The effects of Wind Reader have worn off.");
                }
                else
                {
                    // Apply accuracy increase
                    if (m_Target is BaseCreature)
                    {
                        BaseCreature creature = (BaseCreature)m_Target;
                        creature.Hits += 5; // Simple example of buffing archery accuracy (5% more damage)
                    }

                    m_Target.FixedParticles(0x375A, 9, 20, 5027, EffectLayer.Waist); // Visual effect
                    m_Target.PlaySound(0x1F2); // Sound effect
                }
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(5.0); // Slightly longer delay due to complexity of effect
        }
    }
}
