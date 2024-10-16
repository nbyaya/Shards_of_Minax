using System;
using System.Collections;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.ACC.CSS.Systems.StealthMagic
{
    public class SmokeBomb : StealthSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Smoke Bomb", "Vas In Flam",
            21013,
            9212
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 20; } }

        private static readonly TimeSpan EffectDuration = TimeSpan.FromSeconds(15.0);
        private static readonly TimeSpan CooldownDuration = TimeSpan.FromMinutes(1.0);

        public SmokeBomb(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(IPoint3D p)
        {
            if (!Caster.CanSee(p))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
            }
            else if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
            {
                if (this.Scroll != null)
                    Scroll.Consume();

                SpellHelper.Turn(Caster, p);
                SpellHelper.GetSurfaceTop(ref p);

                Point3D loc = new Point3D(p);
                Map map = Caster.Map;

                Effects.SendLocationEffect(loc, map, 0x375A, 20, 10, 0, 0); // Smoke effect
                Effects.PlaySound(loc, map, 0x22F); // Explosion sound

                new SmokeBombEffect(Caster, loc, map, EffectDuration);
            }

            FinishSequence();
        }

        private class SmokeBombEffect
        {
            private Mobile m_Caster;
            private Point3D m_Location;
            private Map m_Map;
            private DateTime m_End;
            private Timer m_Timer;

            public SmokeBombEffect(Mobile caster, Point3D location, Map map, TimeSpan duration)
            {
                m_Caster = caster;
                m_Location = location;
                m_Map = map;
                m_End = DateTime.Now + duration;

                m_Timer = new EffectTimer(this, TimeSpan.FromSeconds(0.5), TimeSpan.FromSeconds(1.0));
                m_Timer.Start();
            }

            private class EffectTimer : Timer
            {
                private SmokeBombEffect m_Effect;

                public EffectTimer(SmokeBombEffect effect, TimeSpan delay, TimeSpan interval) : base(delay, interval)
                {
                    m_Effect = effect;
                }

                protected override void OnTick()
                {
                    if (DateTime.Now > m_Effect.m_End)
                    {
                        Stop();
                        return;
                    }

                    ArrayList targets = new ArrayList();

                    foreach (Mobile m in m_Effect.m_Map.GetMobilesInRange(m_Effect.m_Location, 5))
                    {
                        if (m != m_Effect.m_Caster && m.Alive && !m.IsDeadBondedPet)
                        {
                            if (m is BaseCreature bc && bc.Controlled)
                                continue; // Skip controlled creatures

                            targets.Add(m);
                        }
                    }

                    foreach (Mobile target in targets)
                    {
                        // Apply accuracy reduction effect
                        target.FixedEffect(0x376A, 10, 20);
                        target.SendMessage("You are blinded by the smoke and your accuracy is reduced!");

                        if (target is PlayerMobile)
                        {
                            target.BeginAction(typeof(SmokeBomb));
                            Timer.DelayCall(TimeSpan.FromSeconds(3.0), () =>
                            {
                                target.EndAction(typeof(SmokeBomb));
                                target.SendMessage("Your vision clears up.");
                            });
                        }

                        // Additional visual/sound effects if needed
                        Effects.SendLocationEffect(target.Location, target.Map, 0x376A, 20, 10);
                        Effects.PlaySound(target.Location, target.Map, 0x1F8);
                    }
                }
            }
        }

        private class InternalTarget : Target
        {
            private SmokeBomb m_Owner;

            public InternalTarget(SmokeBomb owner) : base(10, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is IPoint3D p)
                    m_Owner.Target(p);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
