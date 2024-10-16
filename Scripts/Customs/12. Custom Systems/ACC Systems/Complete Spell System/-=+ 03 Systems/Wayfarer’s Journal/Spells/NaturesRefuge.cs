using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using System.Collections;
using Server.Items;
using Server.Misc;

namespace Server.ACC.CSS.Systems.CampingMagic
{
    public class NaturesRefuge : CampingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Nature's Refuge", "An Sanct Terra",
            21005, 9301
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 50; } }

        private Mobile m_Caster;

        public NaturesRefuge(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
            m_Caster = caster;
        }

        public override void OnCast()
        {
            m_Caster.Target = new InternalTarget(this);
        }

        public void Target(IPoint3D p)
        {
            if (!m_Caster.CanSee(p))
            {
                m_Caster.SendLocalizedMessage(500237); // Target cannot be seen.
            }
            else if (SpellHelper.CheckTown(p, m_Caster) && CheckSequence())
            {
                if (this.Scroll != null)
                    Scroll.Consume();

                SpellHelper.Turn(m_Caster, p);
                SpellHelper.GetSurfaceTop(ref p);

                Point3D loc = new Point3D(p.X, p.Y, p.Z);
                Effects.PlaySound(loc, m_Caster.Map, 0x2D6);
                Effects.SendLocationParticles(EffectItem.Create(loc, m_Caster.Map, EffectItem.DefaultDuration), 0x375A, 1, 30, 1153, 4, 3, 0);

                InternalItem refugeCircle = new InternalItem(loc, m_Caster.Map, m_Caster);
                refugeCircle.MoveToWorld(loc, m_Caster.Map);

                FinishSequence();
            }
        }

        private class InternalItem : Item
        {
            private Timer m_Timer;
            private Timer m_Bless;
            private Mobile m_Caster;

            public InternalItem(Point3D loc, Map map, Mobile caster) : base(0x1B72) // Circle of Protection visual
            {
                Movable = false;
                MoveToWorld(loc, map);
                m_Caster = caster;

                if (caster.InLOS(this))
                {
                    Visible = true;
                    Effects.PlaySound(loc, map, 0x1F7); // Sound effect for the protection
                    m_Timer = new InternalTimer(this, TimeSpan.FromSeconds(20.0));
                    m_Timer.Start();

                    m_Bless = new BlessTimer(this, caster);
                    m_Bless.Start();
                }
                else
                {
                    Delete();
                }
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

                public InternalTimer(InternalItem item, TimeSpan duration) : base(duration)
                {
                    m_Item = item;
                }

                protected override void OnTick()
                {
                    m_Item.Delete();
                }
            }

			private class BlessTimer : Timer
			{
				private Item m_RefugeCircle;
				private Mobile m_Caster;

				public BlessTimer(Item circle, Mobile caster) : base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0))
				{
					Priority = TimerPriority.OneSecond;
					m_RefugeCircle = circle;
					m_Caster = caster;
				}

				protected override void OnTick()
				{
					if (m_RefugeCircle.Deleted)
						return;

					ArrayList list = new ArrayList();

					foreach (Mobile m in m_RefugeCircle.GetMobilesInRange(5))
					{
						if (m.Player && m.Alive && !m.Criminal && !IsHarmful(m))
							list.Add(m);
					}

					for (int i = 0; i < list.Count; ++i)
					{
						Mobile m = (Mobile)list[i];
						m.FixedEffect(0x376A, 10, 16, 1153, 0); // Protective visual effect
						m.SendMessage("You feel the protective power of Nature's Refuge.");
						m.VirtualArmorMod += 10; // Increase armor temporarily
					}
				}

				private bool IsHarmful(Mobile target)
				{
					// Define your logic to check if the target is considered harmful or not.
					// For instance, check if the caster and target are enemies, or if
					// the target is currently in combat.
					
					// Placeholder logic: Adjust as needed
					return !m_Caster.CanBeHarmful(target);
				}
			}

        }

        private class InternalTarget : Target
        {
            private NaturesRefuge m_Owner;

            public InternalTarget(NaturesRefuge owner) : base(12, true, TargetFlags.None)
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
