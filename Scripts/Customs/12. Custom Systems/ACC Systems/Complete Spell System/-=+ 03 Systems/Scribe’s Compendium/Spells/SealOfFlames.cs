using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.InscribeMagic
{
    public class SealOfFlames : InscribeSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Seal of Flames", "Rex Flam Grav",
            //SpellCircle.Fourth,
            21004,
            9300,
            false
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 30; } }

        public SealOfFlames(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
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

                Effects.PlaySound(p, Caster.Map, 0x208); // Sound of fire
                Effects.SendLocationEffect(new Point3D(p), Caster.Map, 0x3709, 30, 10, 0, 0); // Fire effect

                InternalItem flameSeal = new InternalItem(new Point3D(p), Caster.Map, Caster);
                flameSeal.MoveToWorld(new Point3D(p), Caster.Map);
            }

            FinishSequence();
        }

        private class InternalItem : Item
        {
            private Timer m_Timer;
            private DateTime m_End;
            private Mobile m_Caster;

            public override bool BlocksFit { get { return true; } }

            public InternalItem(Point3D loc, Map map, Mobile caster) : base(0x3709) // Fire effect item ID
            {
                Movable = false;
                Visible = true;
                m_Caster = caster;
                MoveToWorld(loc, map);

                m_Timer = new InternalTimer(this, TimeSpan.FromSeconds(30.0));
                m_Timer.Start();

                m_End = DateTime.Now + TimeSpan.FromSeconds(30.0); // Duration of the seal
            }

            public InternalItem(Serial serial) : base(serial)
            {
            }

            public override void Serialize(GenericWriter writer)
            {
                base.Serialize(writer);
                writer.Write((int)0); // version
                writer.Write(m_End - DateTime.Now);
            }

            public override void Deserialize(GenericReader reader)
            {
                base.Deserialize(reader);
                int version = reader.ReadInt();
                TimeSpan duration = reader.ReadTimeSpan();

                m_Timer = new InternalTimer(this, duration);
                m_Timer.Start();

                m_End = DateTime.Now + duration;
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

            public override bool OnMoveOver(Mobile m)
            {
                if (m is BaseCreature || m.Player)
                {
                    if (m.Karma <= 0 && m.Alive) // Damages only hostile or negative karma entities
                    {
                        int damage = Utility.RandomMinMax(10, 20); // Random damage between 10 and 20
                        AOS.Damage(m, m_Caster, damage, 0, 100, 0, 0, 0); // Fire damage
                        m.PlaySound(0x208); // Fire sound effect
                        m.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot); // Fire visual effect
                    }
                }

                return base.OnMoveOver(m);
            }
        }

        private class InternalTarget : Target
        {
            private SealOfFlames m_Owner;

            public InternalTarget(SealOfFlames owner) : base(12, true, TargetFlags.None)
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
}
