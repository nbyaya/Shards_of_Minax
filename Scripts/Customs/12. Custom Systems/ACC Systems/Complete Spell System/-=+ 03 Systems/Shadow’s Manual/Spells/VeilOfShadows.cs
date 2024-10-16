using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.HidingMagic
{
    public class VeilOfShadows : HidingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Veil of Shadows", "Umbra Caligo",
            //SpellCircle.Third,
            21002,
            9201
        );

        public override SpellCircle Circle { get { return SpellCircle.Third; } }
        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 40.0; } }
        public override int RequiredMana { get { return 20; } }

        public VeilOfShadows(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
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

                // Visual and sound effects
                Effects.PlaySound(p, Caster.Map, 0x280); // Dark magic sound
                Effects.SendLocationParticles(EffectItem.Create(new Point3D(p), Caster.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 1109, 0, 5022, 0); // Dark smoke effect

                // Create the area of darkness
                new DarknessArea(Caster, new Point3D(p), Caster.Map, TimeSpan.FromSeconds(10.0));

                FinishSequence();
            }
        }

        private class InternalTarget : Target
        {
            private VeilOfShadows m_Owner;

            public InternalTarget(VeilOfShadows owner) : base(12, true, TargetFlags.None)
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
    }

    public class DarknessArea : Item
    {
        private Timer m_Timer;
        private Mobile m_Caster;

        public DarknessArea(Mobile caster, Point3D loc, Map map, TimeSpan duration) : base(0x1)
        {
            Movable = false;
            Visible = false;
            m_Caster = caster;

            MoveToWorld(loc, map);

            m_Timer = new InternalTimer(this, duration);
            m_Timer.Start();

            Effects.SendLocationParticles(EffectItem.Create(loc, map, EffectItem.DefaultDuration), 0x376A, 9, 32, 1109, 0, 5022, 0); // Dark smoke effect
        }

        public override void OnDelete()
        {
            base.OnDelete();

            if (m_Timer != null)
                m_Timer.Stop();
        }

        private class InternalTimer : Timer
        {
            private DarknessArea m_Item;

            public InternalTimer(DarknessArea item, TimeSpan duration) : base(duration)
            {
                m_Item = item;
                Priority = TimerPriority.FiftyMS;
            }

            protected override void OnTick()
            {
                m_Item.Delete();
            }
        }

        public DarknessArea(Serial serial) : base(serial)
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

        public override bool OnMoveOver(Mobile m)
        {
            if (m != m_Caster && m is BaseCreature && ((BaseCreature)m).ControlMaster != m_Caster)
            {
                // Apply darkness debuff here, such as lowering hit chance or adding a status effect
                m.SendMessage("You are enveloped in darkness and find it harder to see!");

                // Example of applying a visual or status effect (can be expanded as needed)
                m.FixedParticles(0x374A, 10, 15, 5038, EffectLayer.Head);
            }

            return base.OnMoveOver(m);
        }
    }
}
