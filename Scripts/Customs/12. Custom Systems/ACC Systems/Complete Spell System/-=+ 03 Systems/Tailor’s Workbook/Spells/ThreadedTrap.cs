using System;
using System.Collections;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using Server.Misc;

namespace Server.ACC.CSS.Systems.TailoringMagic
{
    public class ThreadedTrap : TailoringSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Threaded Trap", "Arani Tela",
                                                        21006,
                                                        9300,
                                                        false
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 35; } }

        public ThreadedTrap(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private ThreadedTrap m_Owner;

            public InternalTarget(ThreadedTrap owner) : base(12, true, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is IPoint3D)
                {
                    IPoint3D p = (IPoint3D)targeted;
                    Point3D point = new Point3D(p); // Convert IPoint3D to Point3D

                    if (!m_Owner.Caster.CanSee(point))
                    {
                        m_Owner.Caster.SendLocalizedMessage(500237); // Target can not be seen.
                    }
                    else if (SpellHelper.CheckTown(point, m_Owner.Caster) && m_Owner.CheckSequence())
                    {
                        SpellHelper.Turn(m_Owner.Caster, point);

                        Effects.PlaySound(point, m_Owner.Caster.Map, 0x229);
                        Effects.SendLocationParticles(EffectItem.Create(point, m_Owner.Caster.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5023);

                        InternalItem trap = new InternalItem(point, m_Owner.Caster.Map, m_Owner.Caster);
                        trap.MoveToWorld(point, m_Owner.Caster.Map);
                    }

                    m_Owner.FinishSequence();
                }
            }
        }

        [DispellableField]
        private class InternalItem : Item
        {
            private Timer m_Timer;
            private Mobile m_Caster;

            public override bool BlocksFit { get { return true; } }

            public InternalItem(Point3D loc, Map map, Mobile caster) : base(0x0911) // Use a trap graphic
            {
                Movable = false;
                Visible = true;
                m_Caster = caster;

                MoveToWorld(loc, map);

                m_Timer = new InternalTimer(this, TimeSpan.FromSeconds(60.0));
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

                m_Timer = new InternalTimer(this, TimeSpan.FromSeconds(60.0));
                m_Timer.Start();
            }

            public void Trigger(Mobile from)
            {
                if (from == m_Caster)
                    return;

                Effects.SendLocationEffect(Location, Map, 0x36BD, 20, 10, 0, 0);
                Effects.PlaySound(Location, Map, 0x307);

                from.FixedParticles(0x374A, 10, 15, 5038, EffectLayer.Waist);
                from.PlaySound(0x205);
                from.Damage(Utility.RandomMinMax(15, 30), m_Caster);

                if (from.Alive)
                    from.Paralyze(TimeSpan.FromSeconds(3.0));

                this.Delete();
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
                    m_Item.Delete();
                }
            }
        }
    }
}
