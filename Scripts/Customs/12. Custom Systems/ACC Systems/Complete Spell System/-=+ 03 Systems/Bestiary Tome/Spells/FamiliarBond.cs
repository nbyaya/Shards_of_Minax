using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.ACC.CSS.Systems.AnimalLoreMagic
{
    public class FamiliarBond : AnimalLoreSpell
    {
        private static readonly SpellInfo m_Info = new SpellInfo(
            "Familiar Bond", "In Vas Mana",
            // SpellCircle.Second, // Uncomment if using a circle-based magic system
            21005,
            9400
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay => 0.1;
        public override double RequiredSkill => 30.0; // Example skill requirement
        public override int RequiredMana => 15;

        public FamiliarBond(Mobile caster, Item scroll) : base(caster, scroll, m_Info) { }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Define the range of effect (5 tiles)
                const int range = 5;

                // Get all mobiles (creatures) within range
                List<Mobile> followers = new List<Mobile>();

                foreach (Mobile m in Caster.GetMobilesInRange(range))
                {
                    if (m is BaseCreature creature && creature.Controlled && creature.ControlMaster == Caster)
                    {
                        followers.Add(m);
                    }
                }

                // Apply mana regeneration buff to each follower
                foreach (Mobile follower in followers)
                {
                    ApplyManaRegeneration(follower);
                }

                // Visual and sound effects
                Effects.PlaySound(Caster.Location, Caster.Map, 0x208); // Example sound effect
                Caster.FixedParticles(0x375A, 10, 15, 5010, EffectLayer.Waist); // Example visual effect

                Caster.SendMessage("Your bond with your followers strengthens their mana regeneration!");
            }

            FinishSequence();
        }

		private void ApplyManaRegeneration(Mobile target)
		{
			// Example of a temporary mana regeneration boost effect
			BuffInfo.AddBuff(target, new BuffInfo(BuffIcon.Bless, 1075643, 1075644, TimeSpan.FromSeconds(30.0), target));
			target.SendMessage("You feel a surge of mana flowing through you!");

			// Add visual effect on the target
			target.FixedParticles(0x373A, 1, 15, 9909, EffectLayer.Waist);
			Effects.SendTargetParticles(target, 0x373A, 10, 20, 0, 10, 30, EffectLayer.Waist, 0);
		}


        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(5.0); // Adjust the cast delay as needed
        }
    }
}
