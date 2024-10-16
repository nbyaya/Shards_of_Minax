using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Items;
using Server.Spells;

namespace Server.ACC.CSS.Systems.StealingMagic
{
    public class Pickpocket : StealingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Pickpocket", "Swift Hands",
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override int RequiredMana { get { return 10; } }
        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 20.0; } }

        public Pickpocket(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new PickpocketTarget(this);
        }

        private class PickpocketTarget : Target
        {
            private Pickpocket m_Ability;

            public PickpocketTarget(Pickpocket ability) : base(1, false, TargetFlags.None)
            {
                m_Ability = ability;
            }

			protected override void OnTarget(Mobile from, object targeted)
			{
				if (targeted is Mobile target && target != from)
				{
					if (!target.Player)
					{
						from.SendMessage("You can only pickpocket players.");
						return;
					}

					double successChance = from.Skills[SkillName.Stealing].Value / 100.0;
					successChance += (from.Skills[SkillName.Stealing].Value - target.Skills[SkillName.Stealth].Value) / 200.0; // Replaced Detection with Stealth

					if (successChance > Utility.RandomDouble())
					{
						Item stolenItem = StealRandomItem(target);

						if (stolenItem != null)
						{
							from.AddToBackpack(stolenItem);
							from.SendMessage("You successfully steal an item!");
							Effects.PlaySound(from.Location, from.Map, 0x2E6); // Sound effect for success
							from.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist); // Visual effect for success
						}
						else
						{
							from.SendMessage("The target has nothing to steal.");
						}
					}
					else
					{
						from.SendMessage("You failed to steal anything.");
						target.SendMessage("Someone tried to pickpocket you!");
						Effects.PlaySound(target.Location, target.Map, 0x5C); // Sound effect for failure
						target.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist); // Visual effect for detection
					}
				}
				else
				{
					from.SendMessage("That is not a valid target.");
				}

				m_Ability.FinishSequence();
			}


            private Item StealRandomItem(Mobile target)
            {
                if (target.Backpack != null && target.Backpack.Items.Count > 0)
                {
                    int index = Utility.Random(target.Backpack.Items.Count);
                    Item item = target.Backpack.Items[index];

                    if (item != null && item.Movable)
                    {
                        item.MoveToWorld(target.Location, target.Map);
                        return item;
                    }
                }

                return null;
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Ability.FinishSequence();
            }
        }
    }
}
