using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.RemoveTrapMagic
{
    public class Camouflage : RemoveTrapSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Camouflage", "Invisible Trap",
            21001,
            9200
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 15; } }

        private object m_TargetedObject;

        public Camouflage(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
            m_TargetedObject = null; // Clear previous target
        }

		private void Target(IPoint3D p)
		{
			Point3D point = new Point3D(p); // Convert to Point3D
			IPoint3D ipoint = point; // Create IPoint3D reference
			if (!Caster.CanSee(point))
			{
				Caster.SendLocalizedMessage(500237); // Target cannot be seen.
			}
			else if (SpellHelper.CheckTown(point, Caster) && CheckSequence())
			{
				SpellHelper.Turn(Caster, point);
				SpellHelper.GetSurfaceTop(ref ipoint); // Fix the point to the surface

				// Convert back to Point3D if needed
				point = new Point3D(ipoint);

				// Play sound and visual effects
				Effects.PlaySound(Caster.Location, Caster.Map, 0x1FD); // Stealth activation sound
				Effects.SendLocationParticles(EffectItem.Create(point, Caster.Map, EffectItem.DefaultDuration), 0x376A, 10, 15, 5044); // Stealth shimmer effect

				// Hide the caster only
				if (Caster is PlayerMobile)
				{
					Caster.Hidden = true;
					Caster.SendMessage("You have become camouflaged!");
				}
				else if (m_TargetedObject is BaseTrap trap)
				{
					// Add additional handling for BaseTrap if needed
					Caster.SendMessage("You have camouflaged the trap!");
				}

				// Start a timer for the camouflage duration
				Timer.DelayCall(TimeSpan.FromSeconds(10), () => EndCamouflage(Caster));
			}

			FinishSequence();
		}


        private void EndCamouflage(Mobile caster)
        {
            if (caster != null && !caster.Deleted && caster.Hidden)
            {
                caster.Hidden = false;
                caster.SendMessage("You are no longer camouflaged.");
            }
        }

        private class InternalTarget : Target
        {
            private Camouflage m_Owner;

            public InternalTarget(Camouflage owner) : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is IPoint3D p)
                {
                    m_Owner.m_TargetedObject = targeted;
                    m_Owner.Target(p);
                }
                else
                {
                    from.SendMessage("You cannot camouflage that.");
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
