using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections;

namespace Server.ACC.CSS.Systems.RemoveTrapMagic
{
    public class ExplosiveTrap : RemoveTrapSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Explosive Trap", "Kal Vas Flam Trap",
            21001,
            9200
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override double CastDelay { get { return 3.0; } }
        public override double RequiredSkill { get { return 70.0; } }
        public override int RequiredMana { get { return 30; } }

        public ExplosiveTrap(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
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

                Effects.PlaySound(p, Caster.Map, 0x307); // Explosion sound
                Effects.SendLocationEffect(new Point3D(p), Caster.Map, 0x36BD, 30, 10, 0, 0); // Explosion visual effect

                new ExplosiveTrapItem(Caster, new Point3D(p), Caster.Map);

                FinishSequence();
            }
        }

        private class InternalTarget : Target
        {
            private ExplosiveTrap m_Owner;

            public InternalTarget(ExplosiveTrap owner) : base(10, true, TargetFlags.None)
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

        private class ExplosiveTrapItem : Item
        {
            private Timer m_Timer;
            private Mobile m_Caster;

            public ExplosiveTrapItem(Mobile caster, Point3D loc, Map map) : base(0x36BD)
            {
                Movable = false;
                Visible = false;
                m_Caster = caster;

                MoveToWorld(loc, map);
                m_Timer = new InternalTimer(this, TimeSpan.FromSeconds(5.0));
                m_Timer.Start();
            }

            public ExplosiveTrapItem(Serial serial) : base(serial)
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
				if (m.Alive && !(m is BaseCreature)) // Changed from 'm is not BaseCreature'
				{
					Detonate();
				}

				return base.OnMoveOver(m);
			}


            public void Detonate()
            {
                if (Deleted)
                    return;

                Effects.PlaySound(Location, Map, 0x207); // Explosion sound
                Effects.SendLocationEffect(Location, Map, 0x36BD, 30, 10, 0, 0); // Explosion visual effect

                ArrayList targets = new ArrayList();

                foreach (Mobile m in GetMobilesInRange(2))
                {
                    if (m != m_Caster && SpellHelper.ValidIndirectTarget(m_Caster, m) && m_Caster.CanBeHarmful(m, false))
                        targets.Add(m);
                }

                foreach (Mobile m in targets)
                {
                    m_Caster.DoHarmful(m);
                    AOS.Damage(m, m_Caster, Utility.RandomMinMax(30, 50), 100, 0, 0, 0, 0);
                }

                Delete();
            }

            private class InternalTimer : Timer
            {
                private ExplosiveTrapItem m_Item;

                public InternalTimer(ExplosiveTrapItem item, TimeSpan duration) : base(duration)
                {
                    m_Item = item;
                }

                protected override void OnTick()
                {
                    m_Item.Detonate();
                }
            }
        }
    }
}
