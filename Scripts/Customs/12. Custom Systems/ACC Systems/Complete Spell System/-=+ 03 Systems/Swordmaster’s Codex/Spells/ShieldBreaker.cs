using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using Server.Engines;

namespace Server.ACC.CSS.Systems.SwordsMagic
{
    public class ShieldBreaker : SwordsSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Shield Breaker", "Ex Ter Sae",
                                                        21010,
                                                        9409
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 25; } }

        public ShieldBreaker(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private ShieldBreaker m_Owner;

            public InternalTarget(ShieldBreaker owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile target && target.FindItemOnLayer(Layer.TwoHanded) is BaseShield shield)
                {
                    m_Owner.Target(target, shield);
                }
                else
                {
                    from.SendMessage("You must target a shield-bearing opponent.");
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

		public void Target(Mobile target, BaseShield shield)
		{
			if (!Caster.CanSee(target))
			{
				Caster.SendLocalizedMessage(500237); // Target cannot be seen.
			}
			else if (CheckHSequence(target))
			{
				SpellHelper.Turn(Caster, target);

				// Updated line with the correct number of parameters
				Effects.SendTargetParticles(target, 0x36B0, 1, 15, 1153, 2, 9918, EffectLayer.Waist, 0); // visual effect on the target's shield
				target.PlaySound(0x22C); // sound effect of breaking shield

				double damage = Utility.RandomMinMax(10, 30) + (Caster.Skills[CastSkill].Value / 2.0);

				if (shield.HitPoints > 0)
				{
					shield.HitPoints -= (int)damage;
					target.SendMessage("Your shield is being damaged!");

					if (shield.HitPoints <= 0)
					{
						target.SendMessage("Your shield has been destroyed!");
						shield.Delete();
					}
				}
				else
				{
					target.SendMessage("Your shield has already been destroyed!");
				}
			}

			FinishSequence();
		}


        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0);
        }
    }
}
