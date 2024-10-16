using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.MusicianshipMagic
{
    public class SoothingSerenade : MusicianshipSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Soothing Serenade", "Calma Melodía",
            21004,
            9300
        );

        public override SpellCircle Circle { get { return SpellCircle.First; } }
        public override double CastDelay { get { return 2.5; } }
        public override double RequiredSkill { get { return 20.0; } }
        public override int RequiredMana { get { return 50; } }

        public SoothingSerenade(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            // Define target area and effects
            Caster.Target = new InternalTarget(this);
        }

		public void Target(IPoint3D p)
		{
			Point3D point = new Point3D(p); // Convert IPoint3D to Point3D

			if (!Caster.CanSee(point))
			{
				Caster.SendLocalizedMessage(500237); // Target can not be seen.
			}
			else if (SpellHelper.CheckTown(point, Caster) && CheckSequence())
			{
				SpellHelper.Turn(Caster, point);

				// Convert point to IPoint3D for GetSurfaceTop, then back to Point3D
				IPoint3D surfaceTop = point;
				SpellHelper.GetSurfaceTop(ref surfaceTop); 

				// After GetSurfaceTop, if surfaceTop is still needed as a Point3D, cast it back
				point = new Point3D(surfaceTop);

				// Visual and sound effect
				Effects.PlaySound(point, Caster.Map, 0x2D6); // A soothing sound effect
				Effects.SendLocationParticles(EffectItem.Create(point, Caster.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 1153, 0, 5022, 0); // A calming blue wave effect

				// Heal and remove debuffs
				List<Mobile> targets = new List<Mobile>();

				foreach (Mobile m in Caster.GetMobilesInRange(5))
				{
					if (m != null && m.Alive && m.Player && m.InLOS(Caster) && m.Karma >= 0 && m != Caster)
						targets.Add(m);
				}

				foreach (Mobile m in targets)
				{
					// Heal the target
					m.Hits += Utility.RandomMinMax(10, 20); // Heals between 10 to 20 HP
					m.SendLocalizedMessage(1008111); // You feel refreshed!

					// Remove minor debuffs
					RemoveMinorDebuffs(m);

					// Additional visual effect on each target
					m.FixedParticles(0x376A, 9, 32, 5005, EffectLayer.Waist); // More calming effects
				}
			}

			FinishSequence();
		}


        private void RemoveMinorDebuffs(Mobile m)
        {
            // Example: Remove common debuffs like poison, fatigue, etc.
            m.Poison = null;

            // Implement any additional debuff removal logic here if needed
        }

        private class InternalTarget : Target
        {
            private SoothingSerenade m_Owner;

            public InternalTarget(SoothingSerenade owner) : base(12, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is IPoint3D)
                    m_Owner.Target((IPoint3D)targeted);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
