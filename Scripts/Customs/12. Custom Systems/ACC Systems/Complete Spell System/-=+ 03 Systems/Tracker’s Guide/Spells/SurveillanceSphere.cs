using System;
using System.Collections;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Items;

namespace Server.ACC.CSS.Systems.TrackingMagic
{
    public class SurveillanceSphere : TrackingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Surveillance Sphere", "Reveal Enemies!",
            21004,
            9300,
            false
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fifth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 40; } }

        public SurveillanceSphere(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private SurveillanceSphere m_Owner;

            public InternalTarget(SurveillanceSphere owner) : base(12, true, TargetFlags.None)
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

        public void Target(IPoint3D p)
        {
            if (!Caster.CanSee(p))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
            {
                SpellHelper.Turn(Caster, p);
                SpellHelper.GetSurfaceTop(ref p);

                Point3D loc = new Point3D(p);

                InternalItem sphere = new InternalItem(loc, Caster.Map, Caster);
                sphere.MoveToWorld(loc, Caster.Map);

                Effects.PlaySound(loc, Caster.Map, 0x208); // Sphere sound
                Effects.SendLocationParticles(EffectItem.Create(loc, Caster.Map, EffectItem.DefaultDuration), 0x375A, 1, 15, 1153, 3, 9914, 0); // Sphere visual effect

                FinishSequence();
            }
        }

        private class InternalItem : Item
        {
            private Timer m_Timer;
            private Mobile m_Caster;

            public InternalItem(Point3D loc, Map map, Mobile caster) : base(0x0F0C)
            {
                Movable = false;
                Visible = true;
                MoveToWorld(loc, map);
                m_Caster = caster;

                m_Timer = new InternalTimer(this, TimeSpan.FromSeconds(30.0));
                m_Timer.Start();
            }

            public InternalItem(Serial serial) : base(serial)
            {
            }

            public override void Serialize(GenericWriter writer)
            {
                base.Serialize(writer);
                writer.Write((int)0); // version
            }

            public override void Deserialize(GenericReader reader)
            {
                base.Deserialize(reader);
                int version = reader.ReadInt();
            }

            public override void OnAfterDelete()
            {
                base.OnAfterDelete();

                if (m_Timer != null)
                    m_Timer.Stop();
            }

            private class InternalTimer : Timer
            {
                private InternalItem m_Item;

                public InternalTimer(InternalItem item, TimeSpan duration) : base(duration)
                {
                    m_Item = item;
                }

                protected override void OnTick()
                {
                    ArrayList targets = new ArrayList();

                    foreach (Mobile m in m_Item.GetMobilesInRange(5))
                    {
                        if (m is BaseCreature && ((BaseCreature)m).Controlled || m.Player)
                        {
                            targets.Add(m);
                        }
                    }

                    for (int i = 0; i < targets.Count; ++i)
                    {
                        Mobile m = (Mobile)targets[i];
                        m.RevealingAction();
                        m.SendMessage("You have been revealed by the Surveillance Sphere!");
                        m.FixedParticles(0x374A, 10, 15, 5036, EffectLayer.Head); // Reveal visual effect
                    }

                    m_Item.Delete();
                }
            }
        }
    }
}
