using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.CartographyMagic
{
	public class PathOfLeastResistance : CartographySpell
	{
		private static SpellInfo m_Info = new SpellInfo(
			"Path of Least Resistance", "Pthio Fin Xen",
			// SpellCircle.Sixth,
			21004,
			9300,
			false
		);

		public override SpellCircle Circle
		{
			get { return SpellCircle.Sixth; }
		}

		public override double CastDelay { get { return 0.2; } }
		public override double RequiredSkill { get { return 70.0; } }
		public override int RequiredMana { get { return 50; } }

		public PathOfLeastResistance(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget(this);
		}

		public void Target(IPoint3D p)
		{
			if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
			{
				if (this.Scroll != null)
					Scroll.Consume();

				SpellHelper.Turn(Caster, p);

				SpellHelper.GetSurfaceTop(ref p);

				// Play a flashy visual effect
				Effects.SendLocationEffect(new Point3D(Caster.X, Caster.Y, Caster.Z), Caster.Map, 0x36B0, 20, 10, 1153, 0);
				Effects.SendLocationEffect(new Point3D(p.X, p.Y, p.Z), Caster.Map, 0x36B0, 20, 10, 1153, 0);

				// Play a teleport sound
				Caster.PlaySound(0x1FC);

				// Teleport the caster to the selected location
				Caster.MoveToWorld(new Point3D(p), Caster.Map);

				// Another visual effect at the destination
				Effects.SendLocationEffect(new Point3D(p.X, p.Y, p.Z), Caster.Map, 0x3728, 10, 10, 1153, 0);

				Caster.SendMessage("You teleport to the chosen location using the Path of Least Resistance.");
			}

			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private PathOfLeastResistance m_Owner;

			public InternalTarget(PathOfLeastResistance owner) : base(12, true, TargetFlags.None)
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

		public override TimeSpan GetCastDelay()
		{
			return TimeSpan.FromSeconds(2); // 1.5 minutes
		}

		// Directly manage cooldown and duration if not possible to override
		public TimeSpan GetCooldown()
		{
			return TimeSpan.FromMinutes(15); // 15 minutes cooldown
		}

		public TimeSpan GetDuration()
		{
			return TimeSpan.FromMinutes(10); // 10 minutes duration
		}
	}

}
