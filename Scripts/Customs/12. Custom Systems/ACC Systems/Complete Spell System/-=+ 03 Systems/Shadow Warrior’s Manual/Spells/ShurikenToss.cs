using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.NinjitsuMagic
{
    public class ShurikenToss : NinjitsuSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Shuriken Toss", "Sha Ken",
            21004, // Animation ID
            9300   // Effect ID
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }

        public ShurikenToss(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private ShurikenToss m_Owner;

            public InternalTarget(ShurikenToss owner) : base(10, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile target)
                {
                    if (m_Owner.CheckHSequence(target))
                    {
                        SpellHelper.Turn(from, target);

                        // Display the throwing animation and sound effect
                        from.Animate(9, 7, 1, true, false, 0);
                        from.PlaySound(0x23B); // Sound for throwing

                        // Display a series of shurikens flying towards the target
                        for (int i = 0; i < 3; i++)
                        {
                            Timer.DelayCall(TimeSpan.FromMilliseconds(200 * i), () =>
                            {
                                Effects.SendMovingEffect(from, target, 0x27AC, 7, 0, false, false, 0, 0); // Shuriken visual effect
                                Effects.PlaySound(target.Location, target.Map, 0x2E3); // Shuriken hit sound
                            });
                        }

                        // Deal damage and apply the disorienting effect
                        AOS.Damage(target, from, Utility.RandomMinMax(15, 25), 0, 100, 0, 0, 0); // Physical damage

                        // Apply disorienting debuff to reduce attack speed
                        target.SendMessage("You feel disoriented as the shurikens strike you!");
                        target.FixedParticles(0x374A, 10, 30, 5054, EffectLayer.Head); // Visual disorienting effect
                        target.PlaySound(0x1F7); // Sound for disorientation

                        // Apply custom debuff effect
                        new DebuffTimer(target).Start();
                    }
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        private class DebuffTimer : Timer
        {
            private Mobile m_Target;

            public DebuffTimer(Mobile target) : base(TimeSpan.FromSeconds(0.1), TimeSpan.FromSeconds(5.0))
            {
                m_Target = target;
            }

            protected override void OnTick()
            {
                if (m_Target != null)
                {
                    // Apply debuff effect: reduce attack speed
                    // Implement custom logic to reduce attack speed or perform other debuff actions
                    m_Target.SendMessage("The disorientation effect wears off.");
                }
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.5); // Adjust cast delay if needed
        }
    }
}
