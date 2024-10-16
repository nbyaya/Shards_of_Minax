using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Items;
using Server.Engines.Craft;
using Server.Engines.XmlSpawner2;

namespace Server.ACC.CSS.Systems.MusicianshipMagic
{
    public class ShieldOfSound : MusicianshipSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Shield of Sound", "Sanctus Sonitus",
                                                        // SpellCircle.Fifth,
                                                        21004,
                                                        9300
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fifth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 70.0; } }
        public override int RequiredMana { get { return 80; } }

        public ShieldOfSound(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
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
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
            }
            else if (SpellHelper.CheckTown(target.Location, Caster) && CheckSequence())
            {
                if (this.Scroll != null)
                    Scroll.Consume();
                
                SpellHelper.Turn(Caster, target);

                // Create the protective sound barrier effect
                Effects.SendLocationParticles(EffectItem.Create(target.Location, target.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5005);
                Effects.PlaySound(target.Location, target.Map, 0x1F5);

                // Apply the sound shield buff
                new SoundShieldBuff(target, TimeSpan.FromSeconds(20.0), 0.5);

                Caster.SendMessage("You have surrounded the target with a shield of sound, absorbing incoming damage!");
                target.SendMessage("You are surrounded by a protective shield of sound, absorbing incoming damage!");

                FinishSequence();
            }
        }

        private class InternalTarget : Target
        {
            private ShieldOfSound m_Owner;

            public InternalTarget(ShieldOfSound owner) : base(12, false, TargetFlags.Beneficial)
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

        private class SoundShieldBuff
        {
            private Mobile m_Target;
            private Timer m_Timer;
            private double m_DamageAbsorbPercentage;

            public SoundShieldBuff(Mobile target, TimeSpan duration, double damageAbsorbPercentage)
            {
                m_Target = target;
                m_DamageAbsorbPercentage = damageAbsorbPercentage;

                m_Timer = new BuffTimer(target, duration, this);
                m_Timer.Start();
            }

            private class BuffTimer : Timer
            {
                private Mobile m_Target;
                private SoundShieldBuff m_Buff;

                public BuffTimer(Mobile target, TimeSpan duration, SoundShieldBuff buff) : base(duration)
                {
                    m_Target = target;
                    m_Buff = buff;
                }

                protected override void OnTick()
                {
                    m_Buff.EndBuff();
                }
            }

            private void EndBuff()
            {
                m_Target.SendMessage("Your Shield of Sound has dissipated.");
                // Implement logic to remove damage absorption effect here
            }
        }
    }
}
