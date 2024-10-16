using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Items;
using Server.Spells;

namespace Server.ACC.CSS.Systems.EvalIntMagic
{
    public class MagicalWard : EvalIntSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Magical Ward", "Pactum Arcanis",
            21005, // Spell effect graphic ID
            9411    // Spell sound ID
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 25; } }

        public MagicalWard(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Target the location for the ward
                Caster.Target = new InternalTarget(this);
            }
            FinishSequence();
        }

		private class InternalTarget : Target
		{
			private MagicalWard m_Owner;

			public InternalTarget(MagicalWard owner) : base(12, true, TargetFlags.None)
			{
				m_Owner = owner;
			}

			protected override void OnTarget(Mobile from, object o)
			{
				// Check if the targeted object is of type IPoint3D
				if (o is IPoint3D point)
				{
					// Convert to Point3D
					Point3D targetPoint = new Point3D(point);

					// Call the Target method with the valid point
					m_Owner.Target(targetPoint);
				}
				else
				{
					// Handle invalid target
					from.SendMessage("Invalid target. Please select a valid location.");
				}
			}

			protected override void OnTargetFinish(Mobile from)
			{
				m_Owner.FinishSequence();
			}
		}


        public void Target(Point3D point)
        {
            if (!Caster.CanSee(point))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
                return;
            }

            if (SpellHelper.CheckTown(point, Caster) && CheckSequence())
            {
                SpellHelper.Turn(Caster, point);

                // Create the ward at the targeted location
                CreateWard(point);

                // Play the spell sound
                Effects.PlaySound(point, Caster.Map, 0x20F);

                // Create a visual effect
                CreateVisualEffect(point);
            }
        }

        private void CreateWard(Point3D point)
        {
            // Create a new item that represents the ward
            InternalWard ward = new InternalWard(point, Caster.Map, Caster);
            ward.MoveToWorld(point, Caster.Map);
        }

        private void CreateVisualEffect(Point3D point)
        {
            for (int i = 0; i < 5; i++)
            {
                Effects.SendLocationEffect(point, Caster.Map, 0x3709, 20, 10);
            }
        }

        private class InternalWard : Item
        {
            private Mobile m_Caster;

            public override bool BlocksFit { get { return true; } }

            public InternalWard(Point3D loc, Map map, Mobile caster) : base(0x1F5C) // Ward item ID
            {
                Visible = false;
                Movable = false;
                MoveToWorld(loc, map);
                m_Caster = caster;

                // Make the ward visible to the caster
                if (caster.InLOS(this))
                    Visible = true;
                else
                    Delete();

                // Set a timer to remove the ward after some time
                Timer timer = new InternalWardTimer(this);
                timer.Start();
            }

            public InternalWard(Serial serial) : base(serial)
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

            private class InternalWardTimer : Timer
            {
                private InternalWard m_Ward;

                public InternalWardTimer(InternalWard ward) : base(TimeSpan.FromSeconds(30.0)) // Ward duration
                {
                    m_Ward = ward;
                }

                protected override void OnTick()
                {
                    if (m_Ward != null && !m_Ward.Deleted)
                    {
                        m_Ward.Delete();
                    }
                }
            }
        }
    }
}
