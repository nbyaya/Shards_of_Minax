using System;
using System.Collections;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.MeditationMagic
{
    public class InnerSanctum : MeditationSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Inner Sanctum", "Sanctum Interius",
            21004, // Icon ID
            9300,  // Casting sound ID
            false, // Allow multiple castings simultaneously
            Reagent.BlackPearl
        );

        public override SpellCircle Circle => SpellCircle.Sixth; // Adjust circle if necessary
        public override double CastDelay => 2.5;
        public override double RequiredSkill => 70.0;
        public override int RequiredMana => 35;

        public InnerSanctum(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
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
                SpellHelper.Turn(Caster, p);
                SpellHelper.GetSurfaceTop(ref p);
                
                // Create the safe zone at the target location
                Point3D loc = new Point3D(p);
                Map map = Caster.Map;
                
                // Play casting effect
                Effects.SendLocationParticles(EffectItem.Create(loc, map, EffectItem.DefaultDuration), 0x373A, 10, 30, 5052);
                Effects.PlaySound(loc, map, 0x299);

                InternalItem sanctum = new InternalItem(Caster.Location, Caster.Map, Caster);
                sanctum.MoveToWorld(loc, map);
            }

            FinishSequence();
        }

        private class InternalItem : Item
        {
            private Timer m_Timer;
            private Mobile m_Caster;

            public override bool BlocksFit => true;

            public InternalItem(Point3D loc, Map map, Mobile caster) : base(0x1ECD) // Adjust ID to desired visual
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

                // Create a timer to remove the safe zone after a set duration
                m_Timer = new InternalTimer(this, TimeSpan.FromSeconds(30.0));
                m_Timer.Start();
                
                // Apply the resistance boost to allies within the zone
                new ResistanceBoostTimer(this, m_Caster).Start();
            }

            public InternalItem(Serial serial) : base(serial) { }

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
        }

		private class ResistanceBoostTimer : Timer
		{
			private Item m_Sanctum;
			private Mobile m_Caster;

			public ResistanceBoostTimer(Item sanctum, Mobile caster) : base(TimeSpan.FromSeconds(0.5), TimeSpan.FromSeconds(1.0))
			{
				Priority = TimerPriority.FiftyMS;
				m_Sanctum = sanctum;
				m_Caster = caster;
			}

			protected override void OnTick()
			{
				if (m_Sanctum.Deleted)
					Stop();

				foreach (Mobile m in m_Sanctum.GetMobilesInRange(5))
				{
					if (m.Player && m.Alive && m != m_Caster && m.InLOS(m_Sanctum))
					{
						// Apply visual and sound effects
						m.FixedParticles(0x373A, 1, 15, 5052, EffectLayer.Waist);
						m.PlaySound(0x202);
						
						// Apply resistance boost using ResistanceMod
						m.AddResistanceMod(new ResistanceMod(ResistanceType.Physical, 10));
						m.AddResistanceMod(new ResistanceMod(ResistanceType.Fire, 10));
						m.AddResistanceMod(new ResistanceMod(ResistanceType.Cold, 10));
						m.AddResistanceMod(new ResistanceMod(ResistanceType.Poison, 10));
						m.AddResistanceMod(new ResistanceMod(ResistanceType.Energy, 10));

						// Apply status effect resistance (e.g., immunity to paralysis)
						m.SendMessage("You feel a protective aura around you.");
					}
				}
			}
		}


        private class InternalTarget : Target
        {
            private InnerSanctum m_Owner;

            public InternalTarget(InnerSanctum owner) : base(12, true, TargetFlags.None)
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
