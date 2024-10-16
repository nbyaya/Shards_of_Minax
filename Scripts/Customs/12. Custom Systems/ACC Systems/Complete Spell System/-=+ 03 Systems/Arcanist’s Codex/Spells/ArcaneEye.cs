using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.EvalIntMagic
{
    public class ArcaneEye : EvalIntSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Arcane Eye", "Rel Por Xen",
            21005,
            9414,
            false // Adjust reagent if needed
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 15; } }

        public ArcaneEye(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

		public void Target(Point3D p) // Changed to Point3D
		{
			if (!Caster.CanSee(p))
			{
				Caster.SendLocalizedMessage(500237); // Target cannot be seen.
			}
			else if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
			{
				SpellHelper.Turn(Caster, p);
				
				// Introduce an intermediate variable for the cast
				IPoint3D ipoint = p;
				SpellHelper.GetSurfaceTop(ref ipoint);

				Point3D newPoint = new Point3D(ipoint);

				Effects.SendLocationParticles(EffectItem.Create(newPoint, Caster.Map, EffectItem.DefaultDuration), 0x3728, 10, 15, 5052);
				Effects.PlaySound(newPoint, Caster.Map, 0x29);

				InternalItem arcaneEye = new InternalItem(newPoint, Caster.Map, Caster);
				arcaneEye.MoveToWorld(newPoint, Caster.Map);
			}

			FinishSequence();
		}


        private class InternalItem : Item
        {
            private Timer m_Timer;
            private Mobile m_Caster;

            public InternalItem(Point3D loc, Map map, Mobile caster) : base(0x1F1C)
            {
                Movable = false;
                Hue = 1153; // Arcane blue hue
                m_Caster = caster;

                MoveToWorld(loc, map);
                Effects.PlaySound(loc, map, 0x29); // Arcane sound effect

                m_Timer = new InternalTimer(this, caster);
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

            private class InternalTimer : Timer
            {
                private InternalItem m_Item;
                private Mobile m_Caster;
                private DateTime m_End;

                public InternalTimer(InternalItem item, Mobile caster) : base(TimeSpan.FromSeconds(0.5), TimeSpan.FromSeconds(1.0))
                {
                    m_Item = item;
                    m_Caster = caster;
                    m_End = DateTime.Now + TimeSpan.FromSeconds(30.0); // Eye lasts for 30 seconds
                    Priority = TimerPriority.OneSecond;
                }

                protected override void OnTick()
                {
                    if (DateTime.Now >= m_End)
                    {
                        m_Item.Delete();
                        Stop();
                    }
                    else
                    {
                        RevealHiddenEntities(m_Item.Location, m_Caster);
                    }
                }

                private void RevealHiddenEntities(Point3D location, Mobile caster)
                {
                    IPooledEnumerable eable = caster.Map.GetMobilesInRange(location, 8); // Range of 8 tiles

                    foreach (Mobile m in eable)
                    {
                        if (m.Hidden && m != caster)
                        {
                            m.RevealingAction();
                            m.SendMessage("You have been revealed by an arcane eye!");
                        }
                    }

                    eable.Free();
                }
            }
        }

        private class InternalTarget : Target
        {
            private ArcaneEye m_Owner;

            public InternalTarget(ArcaneEye owner) : base(12, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Point3D p) // Changed to Point3D
                    m_Owner.Target(p);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
