using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Items;

namespace Server.ACC.CSS.Systems.VeterinaryMagic
{
    public class ProtectiveStance : VeterinarySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Protective Stance", "Defensus Postura",
            21005, // Icon ID
            9301, // Sound ID
            false
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }

        public ProtectiveStance(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(Mobile m)
        {
            if (!Caster.CanSee(m))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
            }
            else if (CheckSequence())
            {
                // Play visual effects and sound
                m.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);
                m.PlaySound(0x1ED);

                // Apply defensive buff
                m.SendMessage("You assume a defensive stance, reducing incoming damage.");
                new ProtectiveStanceBuff(m, TimeSpan.FromSeconds(30)).Apply();

                FinishSequence();
            }
        }

        private class InternalTarget : Target
        {
            private ProtectiveStance m_Owner;

            public InternalTarget(ProtectiveStance owner) : base(12, false, TargetFlags.Beneficial)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile)
                    m_Owner.Target((Mobile)targeted);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        private class ProtectiveStanceBuff
        {
            private Mobile m_Mobile;
            private TimeSpan m_Duration;
            private Timer m_Timer;
            private double m_DamageMod;

            public ProtectiveStanceBuff(Mobile mobile, TimeSpan duration)
            {
                m_Mobile = mobile;
                m_Duration = duration;
                m_DamageMod = 0.5; // Example damage reduction value

                Apply();
                m_Timer = new ExpireTimer(mobile, this);
                m_Timer.Start();
            }

            public void Apply()
            {
                // Apply damage reduction effect here
                m_Mobile.SendMessage("Your defensive stance is active.");
            }

            public void Remove()
            {
                // Remove damage reduction effect here
                m_Mobile.SendMessage("Your protective stance has worn off.");
            }

            private class ExpireTimer : Timer
            {
                private Mobile m_Mobile;
                private ProtectiveStanceBuff m_Buff;

                public ExpireTimer(Mobile mobile, ProtectiveStanceBuff buff) : base(buff.m_Duration)
                {
                    m_Mobile = mobile;
                    m_Buff = buff;
                }

                protected override void OnTick()
                {
                    m_Buff.Remove();
                }
            }
        }
    }
}
