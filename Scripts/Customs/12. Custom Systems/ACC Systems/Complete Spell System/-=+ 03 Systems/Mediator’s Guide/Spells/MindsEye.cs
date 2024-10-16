using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.MeditationMagic
{
    public class MindsEye : MeditationSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Mind's Eye", "Mentem Oculos",
            //SpellCircle.Third,
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }

        public MindsEye(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(Mobile target)
        {
            if (!Caster.CanSee(target))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (CheckSequence())
            {
                // Consume Mana and scroll if applicable
                if (this.Scroll != null)
                    Scroll.Consume();
                
                Caster.Mana -= RequiredMana;

                // Apply perception buff to target
                Effects.PlaySound(target.Location, target.Map, 0x213); // Play sound effect
                target.FixedParticles(0x375A, 10, 15, 5010, EffectLayer.Head); // Visual effect on target
                
                // Custom buff effect (increase detection ability for hidden entities)
                BuffInfo.AddBuff(target, new BuffInfo(BuffIcon.Bless, 1044117, 1075841, TimeSpan.FromSeconds(30), target)); // Buff with a crystal ball icon, lasting 30 seconds
                
                target.SendMessage("Your perception is heightened, allowing you to detect hidden enemies or traps more easily!");

                // Start a timer to handle the duration of the perception buff
                Timer buffTimer = new PerceptionBuffTimer(target, TimeSpan.FromSeconds(30));
                buffTimer.Start();
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private MindsEye m_Owner;

            public InternalTarget(MindsEye owner) : base(12, false, TargetFlags.Beneficial)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                    m_Owner.Target((Mobile)o);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        private class PerceptionBuffTimer : Timer
        {
            private Mobile m_Target;

            public PerceptionBuffTimer(Mobile target, TimeSpan duration) : base(duration)
            {
                m_Target = target;
                Priority = TimerPriority.TwoFiftyMS;
            }

            protected override void OnTick()
            {
                if (m_Target != null && !m_Target.Deleted)
                {
                    m_Target.SendMessage("Your heightened perception fades.");
                    BuffInfo.RemoveBuff(m_Target, BuffIcon.Bless);
                }
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.0);
        }
    }
}
