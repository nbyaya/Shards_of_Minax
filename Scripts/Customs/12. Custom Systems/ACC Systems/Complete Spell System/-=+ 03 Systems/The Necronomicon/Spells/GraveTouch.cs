using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections;

namespace Server.ACC.CSS.Systems.NecromancyMagic
{
    public class GraveTouch : NecromancySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Grave Touch", "Anima Mortis",
            21004, // Animation
            9300,  // Sound
            false
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; } // Assuming First Circle, adjust as needed
        }

        public override double CastDelay { get { return 0.5; } } // Faster cast delay for a melee attack
        public override double RequiredSkill { get { return 30.0; } } // Adjust skill requirement
        public override int RequiredMana { get { return 20; } }

        public GraveTouch(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private GraveTouch m_Owner;

            public InternalTarget(GraveTouch owner) : base(1, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile target && from.CanBeHarmful(target, false))
                {
                    m_Owner.Target(target);
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

		public void Target(Mobile target)
		{
			if (!Caster.CanSee(target))
			{
				Caster.SendLocalizedMessage(500237); // Target cannot be seen
			}
			else
			{
				// Removed CheckHSequence call
				Caster.DoHarmful(target);

				// Visual and sound effects
				Effects.SendMovingEffect(Caster, target, 0xF51, 10, 0, false, false, 0, 0);
				target.FixedParticles(0x374A, 1, 15, 9909, 1153, 3, EffectLayer.Head);
				target.PlaySound(0x1FB); // Sound for fear effect

				// Inflict damage and paralyze
				int damage = Utility.RandomMinMax(10, 20); // Adjust damage range
				AOS.Damage(target, Caster, damage, 100, 0, 0, 0, 0);

				target.Paralyze(TimeSpan.FromSeconds(3.0)); // Paralyze for 3 seconds

				// Additional visual effect to show fear
				target.FixedParticles(0x37B9, 10, 30, 5052, EffectLayer.LeftFoot);
				target.SendMessage("You are paralyzed with fear!");

				// Mana cost
				Caster.Mana -= RequiredMana;
			}

			FinishSequence();
		}


        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.0);
        }

        // Removed the override keyword, not overriding a method
        public void OnHit(Mobile attacker, Mobile defender)
        {
            if (Utility.RandomDouble() < 0.25) // 25% chance to trigger the fear effect on melee hit
            {
                defender.Freeze(TimeSpan.FromSeconds(3.0)); // Freeze for 3 seconds
                defender.FixedParticles(0x3779, 1, 15, 9502, EffectLayer.Waist);
                defender.PlaySound(0x204);
                defender.SendMessage("You are frozen in fear!");
            }
        }
    }
}
