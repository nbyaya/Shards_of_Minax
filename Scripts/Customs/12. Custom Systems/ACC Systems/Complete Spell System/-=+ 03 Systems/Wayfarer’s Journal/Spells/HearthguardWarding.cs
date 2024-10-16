using System;
using System.Collections;
using Server.Mobiles;
using Server.Items;
using Server.Spells;
using Server.Network;
using Server.Targeting;

namespace Server.ACC.CSS.Systems.CampingMagic
{
    public class HearthguardWarding : CampingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Hearthguard Warding", "Ward Us!",
            //SpellCircle.Fourth,
            21005,
            9301
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 40; } }

        public HearthguardWarding(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
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
			else if (CheckSequence())
			{
				// No need to cast IPoint3D to Point3D; use the IPoint3D interface directly
				IPoint3D location = p;

				SpellHelper.Turn(Caster, location);
				SpellHelper.GetSurfaceTop(ref location);

				// Visual and sound effects for casting
				Effects.SendLocationParticles(
					EffectItem.Create(new Point3D(location), Caster.Map, EffectItem.DefaultDuration),
					0x3728, 10, 15, 5042
				);

				Effects.PlaySound(new Point3D(location), Caster.Map, 0x1FB);

				InternalItem wardStone = new InternalItem(Caster, new Point3D(location), Caster.Map);
				wardStone.MoveToWorld(new Point3D(location), Caster.Map);

				FinishSequence();
			}
		}


        private class InternalTarget : Target
        {
            private HearthguardWarding m_Owner;

            public InternalTarget(HearthguardWarding owner) : base(10, true, TargetFlags.None)
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

        private class InternalItem : Item
        {
            private Timer m_Timer;
            private Mobile m_Caster;
            private const int Range = 5; // The range of the ward's effect

            public InternalItem(Mobile caster, Point3D loc, Map map) : base(0x1F14) // Stone block graphic
            {
                Movable = false;
                MoveToWorld(loc, map);

                m_Caster = caster;
                Visible = true;
                Name = "Hearthguard Ward Stone";

                // Start a timer to handle the ward effect
                m_Timer = new InternalTimer(this, TimeSpan.FromMinutes(2.0)); // 2 minutes duration
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

                // Restart timer after deserialization
                m_Timer = new InternalTimer(this, TimeSpan.FromMinutes(2.0));
                m_Timer.Start();
            }

            public override void OnAfterDelete()
            {
                base.OnAfterDelete();
                m_Timer?.Stop();
            }

            private class InternalTimer : Timer
            {
                private InternalItem m_Item;

                public InternalTimer(InternalItem item, TimeSpan duration) : base(duration)
                {
                    m_Item = item;
                    Priority = TimerPriority.OneSecond;
                }

                protected override void OnTick()
                {
                    // Emit sound and visual effect indicating the ward's expiration
                    Effects.PlaySound(m_Item.Location, m_Item.Map, 0x1FB);
                    Effects.SendLocationParticles(
                        EffectItem.Create(m_Item.Location, m_Item.Map, EffectItem.DefaultDuration),
                        0x3728, 10, 15, 5042
                    );

                    m_Item.Delete();
                }
            }

            public override void OnLocationChange(Point3D oldLocation)
            {
                base.OnLocationChange(oldLocation);

                // Deterring effect on hostile creatures
                ArrayList list = new ArrayList();

                foreach (Mobile m in GetMobilesInRange(Range))
                {
                    if (m is BaseCreature && ((BaseCreature)m).ControlMaster == null)
                    {
                        BaseCreature creature = (BaseCreature)m;
                        if (creature.Combatant == m_Caster || creature.IsEnemy(m_Caster))
                        {
                            // Apply deterring effect
                            creature.Freeze(TimeSpan.FromSeconds(2.0));
                            creature.Combatant = null;
                            creature.PublicOverheadMessage(MessageType.Regular, 0x22, false, "*feels repelled*");
                        }
                    }
                }
            }
        }
    }
}
