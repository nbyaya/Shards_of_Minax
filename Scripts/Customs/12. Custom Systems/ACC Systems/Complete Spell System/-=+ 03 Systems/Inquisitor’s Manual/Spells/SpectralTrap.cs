using System;
using Server.Targeting;
using Server.Network;
using Server.Spells;
using Server.Mobiles;
using Server.Items;

namespace Server.ACC.CSS.Systems.DetectHiddenMagic
{
    public class SpectralTrap : DetectHiddenSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Spectral Trap", "Spectre Crux",
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 30; } }

        public SpectralTrap(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private SpectralTrap m_Owner;

            public InternalTarget(SpectralTrap owner) : base(12, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is IPoint3D)
                {
                    IPoint3D point = (IPoint3D)o;
                    Point3D p = new Point3D(point.X, point.Y, point.Z);
                    m_Owner.Target(p);
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

		public void Target(Point3D p)
		{
			if (!Caster.CanSee(p))
			{
				Caster.SendLocalizedMessage(500237); // Target can not be seen.
			}
			else if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
			{
				SpellHelper.Turn(Caster, p);
				
				// Create a temporary IPoint3D wrapper
				IPoint3D tempPoint = new Point3D(p.X, p.Y, p.Z);
				SpellHelper.GetSurfaceTop(ref tempPoint);

				// Use the updated point
				Point3D newPoint = new Point3D(tempPoint.X, tempPoint.Y, tempPoint.Z);

				Effects.PlaySound(newPoint, Caster.Map, 0x653); // Spectral trap sound effect
				Effects.SendLocationParticles(EffectItem.Create(newPoint, Caster.Map, EffectItem.DefaultDuration), 0x376A, 10, 20, 5023); // Ghostly effect

				InternalItem trap = new InternalItem(newPoint, Caster.Map, Caster);
				trap.MoveToWorld(newPoint, Caster.Map);
			}

			FinishSequence();
		}


        private class InternalItem : Item
        {
            private Timer m_Timer;
            private Mobile m_Caster;

            public InternalItem(Point3D loc, Map map, Mobile caster) : base(0x1B72) // Trap ID
            {
                Movable = false;
                Visible = false;
                MoveToWorld(loc, map);
                m_Caster = caster;

                if (caster.InLOS(this))
                    Visible = true;
                else
                    Delete();

                if (Deleted)
                    return;

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

            public override void OnAfterDelete()
            {
                base.OnAfterDelete();
                if (m_Timer != null)
                    m_Timer.Stop();
            }

            private class InternalTimer : Timer
            {
                private InternalItem m_Item;
                private Mobile m_Caster;

                public InternalTimer(InternalItem item, Mobile caster) : base(TimeSpan.FromSeconds(0.5), TimeSpan.FromSeconds(0.5))
                {
                    m_Item = item;
                    m_Caster = caster;
                    Priority = TimerPriority.FiftyMS;
                }

                protected override void OnTick()
                {
                    foreach (Mobile m in m_Item.GetMobilesInRange(1))
                    {
                        if (m != null && m.Hidden && m.Alive && m.Player && m != m_Caster)
                        {
                            m.RevealingAction();
                            m.SendMessage(38, "You have triggered a Spectral Trap!");

                            m.FixedParticles(0x374A, 10, 15, 5028, EffectLayer.Head); // Immobilization effect
                            m.PlaySound(0x204);

                            m.Freeze(TimeSpan.FromSeconds(3)); // Freeze the target for 3 seconds

                            foreach (NetState ns in m_Caster.GetClientsInRange(15))
                            {
                                ns.Mobile.SendMessage(68, $"{m.Name} has been revealed by the Spectral Trap!");
                            }

                            m_Item.Delete();
                            break;
                        }
                    }
                }
            }
        }
    }
}
