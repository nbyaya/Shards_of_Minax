using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.InscribeMagic
{
    public class RuneOfConfusion : InscribeSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Rune of Confusion", "In Sar Ylem",
            21004,
            9300
        );

        public override SpellCircle Circle => SpellCircle.Sixth;

        public override double CastDelay => 1.5;
        public override double RequiredSkill => 75.0;
        public override int RequiredMana => 20;

        public RuneOfConfusion(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

		private void Target(IPoint3D p)
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

				Point3D point = new Point3D(p); // Convert IPoint3D to Point3D

				Effects.PlaySound(point, Caster.Map, 0x1FE); // Play a sound for rune placement
				Effects.SendLocationParticles(EffectItem.Create(point, Caster.Map, EffectItem.DefaultDuration), 0x376A, 10, 15, 5018); // Rune visual effect

				InternalItem rune = new InternalItem(point, Caster.Map, Caster);
				rune.MoveToWorld(point, Caster.Map);
			}

			FinishSequence();
		}


        private class InternalItem : Item
        {
            private Timer m_Timer;
            private Mobile m_Caster;

            public InternalItem(Point3D loc, Map map, Mobile caster) : base(0x1F14)
            {
                Movable = false;
                MoveToWorld(loc, map);
                m_Caster = caster;
                Visible = true;

                m_Timer = new InternalTimer(this, TimeSpan.FromSeconds(10.0));
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

                public InternalTimer(InternalItem item, TimeSpan duration) : base(duration)
                {
                    m_Item = item;
                    Priority = TimerPriority.OneSecond;
                }

                protected override void OnTick()
                {
                    if (m_Item.Deleted)
                        return;

                    m_Item.Delete();

                    // Get all enemies in range
                    List<Mobile> targets = new List<Mobile>();

                    foreach (Mobile m in m_Item.GetMobilesInRange(5))
                    {
                        if (m is BaseCreature && ((BaseCreature)m).Controlled == false && m.Combatant != null)
                        {
                            targets.Add(m);
                        }
                    }

                    // Apply confusion effect
                    foreach (Mobile target in targets)
                    {
                        target.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist); // Confusion visual effect
                        target.PlaySound(0x145); // Confusion sound effect

                        target.Combatant = targets[Utility.Random(targets.Count)];
                        target.SendMessage("You are confused and disoriented!"); // Message to player
                    }
                }
            }
        }

        private class InternalTarget : Target
        {
            private RuneOfConfusion m_Owner;

            public InternalTarget(RuneOfConfusion owner) : base(12, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is IPoint3D)
                    m_Owner.Target(new Point3D((IPoint3D)o)); // Convert IPoint3D to Point3D
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
