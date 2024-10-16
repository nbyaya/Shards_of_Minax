using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Items;
using Server.Spells;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.RemoveTrapMagic
{
    public class TrapCreation : RemoveTrapSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Trap Creation", "In Vas Corp",
                                                        21001,
                                                        9200
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 15; } }

        public TrapCreation(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
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
                if (this.Scroll != null)
                    Scroll.Consume();

                SpellHelper.Turn(Caster, p);

                SpellHelper.GetSurfaceTop(ref p);

                Point3D loc = new Point3D(p);

                Effects.PlaySound(loc, Caster.Map, 0x208);
                Effects.SendLocationParticles(EffectItem.Create(loc, Caster.Map, EffectItem.DefaultDuration), 0x3709, 10, 30, 5052);

                BasicTrap trap = new BasicTrap(Caster, loc, Caster.Map);
                trap.MoveToWorld(loc, Caster.Map);
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private TrapCreation m_Owner;

            public InternalTarget(TrapCreation owner) : base(10, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is IPoint3D p)
                    m_Owner.Target(p);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }

    public class BasicTrap : Item
    {
        private Timer m_Timer;
        private Mobile m_Caster;

        [Constructable]
        public BasicTrap(Mobile caster, Point3D location, Map map) : base(0x1B72)
        {
            Movable = false;
            Visible = true;
            m_Caster = caster;
            MoveToWorld(location, map);

            Effects.PlaySound(location, map, 0x208);
            m_Timer = Timer.DelayCall(TimeSpan.FromSeconds(5.0), TimeSpan.FromSeconds(1.0), OnTrapTrigger);
        }

        public BasicTrap(Serial serial) : base(serial)
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

        private void OnTrapTrigger()
        {
            if (Deleted)
                return;

            List<Mobile> targets = new List<Mobile>();

            foreach (Mobile m in GetMobilesInRange(1))
            {
                if (m != m_Caster && m.Alive && m.AccessLevel == AccessLevel.Player)
                    targets.Add(m);
            }

            if (targets.Count > 0)
            {
                foreach (Mobile target in targets)
                {
                    target.SendLocalizedMessage(1060516); // A trap has been sprung!
                    Effects.SendLocationEffect(target.Location, target.Map, 0x36BD, 20, 10);
                    Effects.PlaySound(target.Location, target.Map, 0x307);
                    AOS.Damage(target, m_Caster, Utility.RandomMinMax(10, 30), 0, 100, 0, 0, 0);
                }

                Delete();
            }
        }

        public override void OnAfterDelete()
        {
            base.OnAfterDelete();

            if (m_Timer != null)
                m_Timer.Stop();
        }
    }
}
