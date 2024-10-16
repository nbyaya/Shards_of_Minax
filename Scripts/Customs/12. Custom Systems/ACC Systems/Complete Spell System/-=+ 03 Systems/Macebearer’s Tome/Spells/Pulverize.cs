using System;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.MacingMagic
{
    public class Pulverize : MacingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Pulverize", "Vas Rel Pulverize",
            21004, 9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Eighth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 70.0; } }
        public override int RequiredMana { get { return 35; } }

        public Pulverize(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private Pulverize m_Owner;

            public InternalTarget(Pulverize owner) : base(1, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

			protected override void OnTarget(Mobile from, object targeted)
			{
				if (targeted is Mobile)
				{
					Mobile target = (Mobile)targeted;

					if (m_Owner.CheckHSequence(target))
					{
						SpellHelper.Turn(from, target);

						// Play visual effects and sound
						Effects.SendLocationEffect(target.Location, target.Map, 0x36B0, 30, 10, 1153, 0); // Shockwave effect
						Effects.PlaySound(target.Location, target.Map, 0x1F2); // Deep sound of a massive blow

						// Calculate damage and apply it
						double damage = from.Skills[SkillName.Macing].Value / 2;
						AOS.Damage(target, from, (int)damage, 100, 0, 0, 0, 0);

						// Chance to break the weapon or shield
						if (Utility.RandomDouble() < 0.2) // 20% chance
						{
							Item item = target.FindItemOnLayer(Layer.TwoHanded);
							if (item == null)
							{
								item = target.FindItemOnLayer(Layer.OneHanded);
							}

							if (item is BaseWeapon)
							{
								item.Delete(); // Remove the weapon from the player's inventory
								target.SendMessage("Your weapon is shattered by the force of the blow!");
							}
						}
					}

					m_Owner.FinishSequence();
				}
			}


            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(3.0);
        }
    }
}
