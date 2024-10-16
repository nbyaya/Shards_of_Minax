using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using System.Collections;

namespace Server.ACC.CSS.Systems.TrackingMagic
{
    public class Camouflage : TrackingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Camouflage", "Invisus Naturae",
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 25; } }

        public Camouflage(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(Mobile target)
        {
            if (target == null || !target.CanSee(Caster))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
            }
            else if (CheckSequence())
            {
                if (this.Scroll != null)
                    Scroll.Consume();

                SpellHelper.Turn(Caster, target);
                Caster.PlaySound(0x29);
                Caster.FixedParticles(0x376A, 10, 15, 5037, EffectLayer.Waist);

                Caster.Hidden = true;
                Caster.SendMessage("You blend into the surroundings, becoming harder to spot while stationary.");
                new InternalTimer(Caster).Start();
            }

            FinishSequence();
        }

        private class InternalTimer : Timer
        {
            private Mobile m_Caster;
            private DateTime m_EndTime;

            public InternalTimer(Mobile caster) : base(TimeSpan.FromSeconds(0.5), TimeSpan.FromSeconds(1.0))
            {
                m_Caster = caster;
                m_EndTime = DateTime.Now + TimeSpan.FromSeconds(15.0); // Duration of Camouflage effect
                Priority = TimerPriority.FiftyMS;
            }

            protected override void OnTick()
            {
                if (m_Caster.Deleted || DateTime.Now > m_EndTime)
                {
                    m_Caster.Hidden = false;
                    m_Caster.SendMessage("The Camouflage effect wears off, making you visible again.");
                    Stop();
                }
                else if (!m_Caster.Frozen && !m_Caster.Hidden)
                {
                    Stop();
                }
            }
        }

        private class InternalTarget : Target
        {
            private Camouflage m_Owner;

            public InternalTarget(Camouflage owner) : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile)
                    m_Owner.Target((Mobile)targeted);
                else
                    from.SendLocalizedMessage(500237); // Target cannot be seen.
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
