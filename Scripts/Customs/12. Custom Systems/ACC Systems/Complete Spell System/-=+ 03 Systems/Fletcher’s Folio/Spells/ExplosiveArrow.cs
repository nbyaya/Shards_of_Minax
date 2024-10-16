using System;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Spells;
using Server.Items;
using Server.Misc;

namespace Server.ACC.CSS.Systems.FletchingMagic
{
    public class ExplosiveArrow : FletchingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Explosive Arrow", "Vas Rel Por Flam",
            // Custom properties for skill system
            21005,
            9400
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; } // Adjust as necessary
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 25; } }

        public ExplosiveArrow(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private ExplosiveArrow m_Owner;

            public InternalTarget(ExplosiveArrow owner) : base(12, false, TargetFlags.Harmful)
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
                        SpellHelper.Turn(from, target);

                        // Effect to represent the arrow firing
                        from.MovingEffect(target, 0xF42, 7, 1, false, false, 0, 0);

                        Timer.DelayCall(TimeSpan.FromSeconds(1.0), () =>
                        {
                            if (!target.Alive || target.Deleted || !from.CanBeHarmful(target))
                                return;

                            // Area of Effect Damage
                            from.DoHarmful(target);
                            AOS.Damage(target, from, Utility.RandomMinMax(30, 40), 100, 0, 0, 0, 0);

                            // Ignite Chance
                            if (Utility.RandomDouble() < 0.3) // 30% chance to ignite
                            {
                                target.SendMessage("You are ignited by the explosive arrow!");
                                target.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);
                                target.PlaySound(0x208);
                                target.Damage(Utility.RandomMinMax(5, 10), from); // Initial ignite damage

                                Timer igniteTimer = new IgniteTimer(target, from);
                                igniteTimer.Start();
                            }

                            // Visual and sound effects
                            Effects.SendLocationEffect(target.Location, target.Map, 0x36BD, 20, 10);
                            Effects.PlaySound(target.Location, target.Map, 0x307);
                        });
                    }

                    m_Owner.FinishSequence();
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        private class IgniteTimer : Timer
        {
            private Mobile m_Target;
            private Mobile m_Caster;
            private int m_Ticks;

            public IgniteTimer(Mobile target, Mobile caster) : base(TimeSpan.FromSeconds(2.0), TimeSpan.FromSeconds(2.0))
            {
                m_Target = target;
                m_Caster = caster;
                m_Ticks = 5; // Ignite lasts for 5 ticks (10 seconds total)
                Priority = TimerPriority.TwoFiftyMS;
            }

            protected override void OnTick()
            {
                if (m_Ticks > 0 && m_Target.Alive && !m_Target.Deleted)
                {
                    m_Target.Damage(Utility.RandomMinMax(2, 5), m_Caster);
                    m_Target.SendMessage("You suffer from burning flames!");
                    m_Target.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);
                    m_Ticks--;
                }
                else
                {
                    Stop();
                }
            }
        }
    }
}
