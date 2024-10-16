using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.DetectHiddenMagic
{
    public class IlluminatingBurst : DetectHiddenSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Illuminating Burst", "Ex Lux",
            21004, // Animation ID
            9300,  // Animation speed
            false,
            Reagent.Garlic
        );

        public override SpellCircle Circle { get { return SpellCircle.Sixth; } }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 70.0; } }
        public override int RequiredMana { get { return 30; } }

        public IlluminatingBurst(Mobile caster, Item scroll) : base(caster, scroll, m_Info) { }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(IPoint3D p)
        {
            if (!Caster.CanSee(p))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
            {
                SpellHelper.Turn(Caster, p);

                Effects.PlaySound(Caster.Location, Caster.Map, 0x209); // Play light burst sound
                Caster.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.Head); // Burst of light visual effect

                IPooledEnumerable mobiles = Caster.GetMobilesInRange(8); // Cone radius
                List<Mobile> affectedMobs = new List<Mobile>();

                foreach (Mobile m in mobiles)
                {
                    if (m != Caster && m.Alive && Caster.InLOS(m) && IsInCone(Caster.Location, m.Location, 90)) // 90-degree cone
                    {
                        if (m.Hidden)
                        {
                            m.RevealingAction(); // Reveal hidden mobiles
                            m.SendMessage("You have been revealed by a burst of light!");
                        }

                        m.SendMessage("You are blinded by a burst of light!");
                        m.FixedParticles(0x3779, 1, 15, 9943, 6, 2, EffectLayer.Head); // Blinded effect visual
                        m.PlaySound(0x1F2); // Blind sound effect
                        affectedMobs.Add(m);

                        // Apply debuff
                        m.BeginAction(typeof(IlluminatingBurst));
                        new TimerState(m, TimeSpan.FromSeconds(5.0), ReduceAccuracy, EndEffect).Start(); // 5 seconds duration
                    }
                }

                mobiles.Free();
            }

            FinishSequence();
        }

        private static bool IsInCone(Point3D start, Point3D target, int angle)
        {
            // Basic calculation to check if the target is within a cone area
            Point3D relative = new Point3D(target.X - start.X, target.Y - start.Y, 0);
            double angleBetween = Math.Atan2(relative.Y, relative.X) * 180 / Math.PI;
            return Math.Abs(angleBetween) <= (angle / 2.0);
        }

        private void ReduceAccuracy(Mobile m)
        {
            m.SendMessage("Your vision is impaired, and your accuracy is reduced!");
            m.VirtualArmorMod += -20; // Reduce accuracy by lowering armor rating (example debuff)
        }

        private void EndEffect(Mobile m)
        {
            m.SendMessage("Your vision returns to normal.");
            m.VirtualArmorMod -= -20; // Remove the debuff
            m.EndAction(typeof(IlluminatingBurst));
        }

        private class InternalTarget : Target
        {
            private IlluminatingBurst m_Owner;

            public InternalTarget(IlluminatingBurst owner) : base(12, false, TargetFlags.Harmful)
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

        private class TimerState : Timer
        {
            private Mobile m_Mobile;
            private Action<Mobile> m_Callback;
            private Action<Mobile> m_EndCallback;

            public TimerState(Mobile m, TimeSpan duration, Action<Mobile> callback, Action<Mobile> endCallback)
                : base(duration)
            {
                m_Mobile = m;
                m_Callback = callback;
                m_EndCallback = endCallback;
            }

            protected override void OnTick()
            {
                m_Callback?.Invoke(m_Mobile);
                new EndEffectTimer(m_Mobile, TimeSpan.FromSeconds(5.0), m_EndCallback).Start(); // Additional duration for the end effect
                Stop();
            }

            private class EndEffectTimer : Timer
            {
                private Mobile m_Mobile;
                private Action<Mobile> m_EndCallback;

                public EndEffectTimer(Mobile m, TimeSpan duration, Action<Mobile> endCallback) : base(duration)
                {
                    m_Mobile = m;
                    m_EndCallback = endCallback;
                }

                protected override void OnTick()
                {
                    m_EndCallback?.Invoke(m_Mobile);
                    Stop();
                }
            }
        }
    }
}
