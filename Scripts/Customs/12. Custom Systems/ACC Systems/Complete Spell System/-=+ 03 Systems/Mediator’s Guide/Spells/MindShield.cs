using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.MeditationMagic
{
    public class MindShield : MeditationSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Mind Shield", "Mento Lux",
            21004, // Effect Icon ID
            9300   // Sound ID
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.2; } } // Cast delay time
        public override double RequiredSkill { get { return 60.0; } } // Required skill level
        public override int RequiredMana { get { return 30; } } // Required mana

        public MindShield(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this); // Set the target for the spell
        }

        public void Target(Mobile target)
        {
            if (!Caster.CanSee(target))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen
            }
            else if (CheckSequence())
            {
                if (this.Scroll != null)
                    Scroll.Consume();

                SpellHelper.Turn(Caster, target);

                // Play sound and visual effect
                Effects.SendLocationParticles(
                    EffectItem.Create(target.Location, target.Map, EffectItem.DefaultDuration), 
                    0x375A, 1, 29, 1153, 4, 9912, 0
                );
                target.PlaySound(0x28E);

                // Apply Mind Shield effect
                BuffInfo.AddBuff(target, new BuffInfo(BuffIcon.Bless, 1075812, 1075813));

                target.SendMessage("You are surrounded by a protective barrier!");

                MindShieldEffect shieldEffect = new MindShieldEffect(target);
                shieldEffect.Start();

                FinishSequence();
            }
        }

        private class MindShieldEffect
        {
            private Mobile m_Target;
            private DateTime m_End;
            private Timer m_Timer;

            public MindShieldEffect(Mobile target)
            {
                m_Target = target;
                m_End = DateTime.Now + TimeSpan.FromSeconds(10.0); // Shield duration

                m_Timer = Timer.DelayCall(TimeSpan.Zero, TimeSpan.FromSeconds(0.1), new TimerCallback(OnTick));
            }

            public void Start()
            {
                m_Target.SendMessage("A magical barrier absorbs incoming damage!");
                m_Target.FixedParticles(0x375A, 1, 15, 1153, 4, 9912, 0);
                m_Timer.Start();
            }

            private void OnTick()
            {
                if (m_Target == null || m_Target.Deleted || DateTime.Now >= m_End)
                {
                    Stop();
                    return;
                }

                m_Target.VirtualArmorMod = 20; // Increase virtual armor by 20
            }

            public void Stop()
            {
                m_Target.VirtualArmorMod = 0; // Remove armor buff
                m_Target.SendMessage("The protective barrier fades away.");
                BuffInfo.RemoveBuff(m_Target, BuffIcon.Bless);
                m_Timer.Stop();
            }
        }

        private class InternalTarget : Target
        {
            private MindShield m_Owner;

            public InternalTarget(MindShield owner) : base(12, false, TargetFlags.Beneficial)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile)
                    m_Owner.Target((Mobile)targeted);
                else
                    from.SendLocalizedMessage(1060508); // You cannot target that.
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
