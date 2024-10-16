using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Items;
using Server.Spells;
using System.Collections;

namespace Server.ACC.CSS.Systems.TailoringMagic
{
    public class PortableWorkbench : TailoringSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Portable Workbench", "To me my bench!",
                                                        21012,
                                                        9300,
                                                        false,
                                                        Reagent.BlackPearl,
                                                        Reagent.MandrakeRoot
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 2.5; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 25; } }

        public PortableWorkbench(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
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
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
            {
                SpellHelper.Turn(Caster, p);
                SpellHelper.GetSurfaceTop(ref p);

                Effects.PlaySound(p, Caster.Map, 0x2E6); // Sound of workbench summoning

                Point3D loc = new Point3D(p);

                // Summoning the workbench
                WorkbenchItem workbench = new WorkbenchItem(Caster.Location, Caster.Map);
                workbench.MoveToWorld(loc, Caster.Map);

                Effects.SendLocationParticles(EffectItem.Create(loc, Caster.Map, EffectItem.DefaultDuration), 0x373A, 1, 20, 1153, 4, 9502, 0); // Visual effect for summoning

                FinishSequence();
            }
        }

        private class InternalTarget : Target
        {
            private PortableWorkbench m_Owner;

            public InternalTarget(PortableWorkbench owner) : base(12, true, TargetFlags.None)
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

        private class WorkbenchItem : Item
        {
            private Timer m_Timer;

            public override bool BlocksFit { get { return true; } }

            public WorkbenchItem(Point3D loc, Map map) : base(0x1E5E) // Workbench item ID
            {
                Movable = false;
                MoveToWorld(loc, map);

                // Timer to remove the workbench after 3 minutes
                m_Timer = new InternalTimer(this, TimeSpan.FromMinutes(3.0));
                m_Timer.Start();
            }

            public WorkbenchItem(Serial serial) : base(serial)
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
                private WorkbenchItem m_Item;

                public InternalTimer(WorkbenchItem item, TimeSpan duration) : base(duration)
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
