using System;
using System.Collections;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.MusicianshipMagic
{
    public class CadenceOfConfusion : MusicianshipSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Cadence of Confusion", "Caden Confusio",
            // SpellCircle.Third,
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 30; } }

        public CadenceOfConfusion(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
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
                SpellHelper.Turn(Caster, p);
                SpellHelper.GetSurfaceTop(ref p);

                Point3D loc = new Point3D(p.X, p.Y, p.Z);
                Map map = Caster.Map;

                Effects.PlaySound(loc, map, 0x5D2); // Sound effect for confusion

                // Confusion effect
                foreach (Mobile m in Caster.GetMobilesInRange(5))
                {
                    if (m != Caster && Caster.CanBeHarmful(m))
                    {
                        Caster.DoHarmful(m);

                        if (Utility.RandomDouble() < 0.5) // 50% chance to attack allies
                        {
                            m.Target = new InternalAttackTarget(m);
                            m.SendMessage("You feel confused and lash out at your allies!");
                        }
                        else // 50% chance to miss their attacks
                        {
                            // Implement custom logic or use alternative stats
                            m.SendMessage("You feel disoriented and find it hard to land your attacks.");
                        }

                        m.FixedParticles(0x374A, 10, 15, 5036, EffectLayer.Head); // Visual effect for confusion
                        m.PlaySound(0x1D3); // Sound effect when confused
                    }
                }
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private CadenceOfConfusion m_Owner;

            public InternalTarget(CadenceOfConfusion owner) : base(12, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is IPoint3D)
                    m_Owner.Target((IPoint3D)o);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        private class InternalAttackTarget : Target
        {
            private Mobile m_Owner;

            public InternalAttackTarget(Mobile owner) : base(-1, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile target)
                {
                    if (m_Owner.CanBeHarmful(target))
                    {
                        m_Owner.RevealingAction();
                        m_Owner.Direction = m_Owner.GetDirectionTo(target);
                        m_Owner.Combatant = target;
                        m_Owner.Attack(target);
                    }
                }
            }
        }
    }
}
