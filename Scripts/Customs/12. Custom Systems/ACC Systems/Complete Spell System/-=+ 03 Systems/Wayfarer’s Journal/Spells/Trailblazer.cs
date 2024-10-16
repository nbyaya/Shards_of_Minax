using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;
using Server.Targeting; // Required for in-range targeting

namespace Server.ACC.CSS.Systems.CampingMagic
{
    public class Trailblazer : CampingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Trailblazer", "Terra Velox",
            21005, // Icon ID
            9301,  // Action ID
            false,
            Reagent.MandrakeRoot,
            Reagent.Bloodmoss
        );

        public override SpellCircle Circle => SpellCircle.Fifth;

        public override double CastDelay => 0.2;
        public override double RequiredSkill => 60.0;
        public override int RequiredMana => 30;

        public Trailblazer(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            // Check if the spell sequence is valid
            if (!CheckSequence())
                return;

            // Get all mobiles within 3 tiles of the caster
            IPooledEnumerable eable = Caster.GetMobilesInRange(3);

            foreach (Mobile mobile in eable)
            {
                // Check if the mobile is a PlayerMobile and if they're alive
                if (mobile is PlayerMobile player && player.Alive)
                {
                    player.SendMessage("You feel lighter on your feet as you traverse the terrain!");
                    player.PlaySound(0x1FB); // Play a magical sound effect
                    player.FixedParticles(0x373A, 1, 15, 9502, EffectLayer.Waist); // Display a particle effect

                    // Apply movement speed boost
                    BuffInfo.AddBuff(player, new BuffInfo(BuffIcon.Agility, 1075849, 1075858, TimeSpan.FromMinutes(2), player)); // Duration: 2 minutes

                    // Apply a temporary speed increase or other effects
                    player.SendMessage("Your speed has been increased temporarily.");
                    
                    // Use a timer to remove the effect after 2 minutes
                    Timer.DelayCall(TimeSpan.FromMinutes(2), () =>
                    {
                        player.SendMessage("The trailblazing effect has worn off.");
                    });
                }
            }

            // Clean up the enumerator
            eable.Free();

            // Complete the spell sequence
            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(10); // Cooldown: 10 seconds
        }
    }
}
