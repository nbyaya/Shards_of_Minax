using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.CartographyMagic
{
    public class ShroudedCartography : CartographySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Shrouded Cartography", "In Vis Elix",
                                                        21004,
                                                        9300,
                                                        false
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; } // Adjust as needed
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } } // Adjust skill requirement as needed
        public override int RequiredMana { get { return 30; } }

        public ShroudedCartography(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

		public override void OnCast()
		{
			if (CheckSequence())
			{
				// Play sound and visual effects
				Caster.PlaySound(0x65A); // Play a mystical sound effect
				Caster.FixedParticles(0x375A, 10, 30, 5037, EffectLayer.Head); // Play a visual effect

				// Hide the caster
				Caster.Hidden = true;

				// Set a timer to unhide after 10 minutes
				Timer.DelayCall(TimeSpan.FromMinutes(10), UnhideCaster);

				// Cooldown logic (45 minutes)
				// Convert current time and cooldown period to ticks and set NextSpellTime

			}

			FinishSequence();
		}


        private void UnhideCaster()
        {
            if (Caster != null && Caster.Hidden)
            {
                Caster.Hidden = false;
                Caster.SendMessage("You are no longer hidden.");
                Caster.PlaySound(0x247); // Play a sound to indicate the end of hiding
                Caster.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Head); // Visual effect when becoming visible
            }
        }

    }
}
