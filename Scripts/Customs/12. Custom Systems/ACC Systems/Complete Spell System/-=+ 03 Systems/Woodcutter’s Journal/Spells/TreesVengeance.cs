using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections;

namespace Server.ACC.CSS.Systems.LumberjackingMagic
{
    public class TreesVengeance : LumberjackingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Tree’s Vengeance", "Ves Re Ylem",
                                                        21012,
                                                        9312,
                                                        false,
                                                        Reagent.Bloodmoss,
                                                        Reagent.MandrakeRoot
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 25; } }

        public TreesVengeance(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
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

                Effects.PlaySound(p, Caster.Map, 0x2F); // Sound of branches creaking

                // Summon animated branches at the target location
                Point3D loc = new Point3D(p);
                int branchCount = 3 + (int)(Caster.Skills[CastSkill].Value / 20.0); // More branches with higher skill

                for (int i = 0; i < branchCount; i++)
                {
                    AnimatedBranch branch = new AnimatedBranch(Caster, loc, Caster.Map);
                    branch.MoveToWorld(new Point3D(loc.X + Utility.Random(-2, 5), loc.Y + Utility.Random(-2, 5), loc.Z), Caster.Map);
                }

                Caster.FixedParticles(0x3728, 10, 15, 5038, EffectLayer.Waist); // Vines and roots effect
                Caster.PlaySound(0x1F5); // Sound of nature awakening
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private TreesVengeance m_Owner;

            public InternalTarget(TreesVengeance owner) : base(12, true, TargetFlags.Harmful)
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

	public class AnimatedBranch : Mobile
	{
		private Mobile m_Caster;
		private Timer m_Timer;
		private Timer m_RemovalTimer; // Timer to remove the branch

		public AnimatedBranch(Mobile caster, Point3D location, Map map) : base()
		{
			m_Caster = caster;
			Body = 47; // Appearance of a branch or a small tree
			Hue = 0x497; // Dark green or brown color
			Name = "Animated Branch";
			MoveToWorld(location, map);

			Effects.SendLocationParticles(EffectItem.Create(location, map, EffectItem.DefaultDuration), 0x373A, 10, 15, 5044); // Leaves falling effect
			PlaySound(0x165); // Sound of leaves rustling

			// Start the attack timer
			m_Timer = new BranchAttackTimer(this, caster);
			m_Timer.Start();

			// Start the removal timer (e.g., 30 seconds)
			m_RemovalTimer = new RemovalTimer(this, TimeSpan.FromSeconds(5));
			m_RemovalTimer.Start();
		}

		public AnimatedBranch(Serial serial) : base(serial)
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

		private class BranchAttackTimer : Timer
		{
			private Mobile m_Branch;
			private Mobile m_Caster;

			public BranchAttackTimer(Mobile branch, Mobile caster) : base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.5))
			{
				m_Branch = branch;
				m_Caster = caster;
				Priority = TimerPriority.TwoFiftyMS;
			}

			protected override void OnTick()
			{
				if (m_Branch == null || m_Branch.Deleted)
				{
					Stop();
					return;
				}

				ArrayList targets = new ArrayList();

				foreach (Mobile m in m_Branch.GetMobilesInRange(3))
				{
					if (m != m_Caster && m.Player && m.Alive && m.AccessLevel == AccessLevel.Player && !m.Blessed)
					{
						targets.Add(m);
					}
				}

				if (targets.Count > 0)
				{
					Mobile target = (Mobile)targets[Utility.Random(targets.Count)];

					if (target != null && !target.Deleted)
					{
						m_Branch.Animate(11, 5, 1, true, false, 0); // Animation of the branch attacking
						m_Branch.PlaySound(0x1B6); // Sound of a branch striking

						int damage = Utility.RandomMinMax(10, 20);
						target.Damage(damage, m_Caster);
						target.FixedParticles(0x374A, 10, 15, 5030, EffectLayer.Waist); // Particles for the hit effect

						if (Utility.RandomDouble() < 0.3) // 30% chance to slow the target
						{
							target.SendMessage("You are entangled by the branches!");
							target.Paralyze(TimeSpan.FromSeconds(2.0));
						}
					}
				}
			}
		}

		private class RemovalTimer : Timer
		{
			private Mobile m_Branch;

			public RemovalTimer(Mobile branch, TimeSpan delay) : base(delay)
			{
				m_Branch = branch;
				Priority = TimerPriority.OneSecond;
			}

			protected override void OnTick()
			{
				if (m_Branch != null && !m_Branch.Deleted)
				{
					m_Branch.Delete(); // Remove the branch from the world
				}
			}
		}
	}

}
