using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.HidingMagic
{
    public class FeignDeath : HidingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Feign Death", "Oi Im dead!",
            21015,
            9214
        );

        public override SpellCircle Circle => SpellCircle.Second; // Choose appropriate spell circle for balance

        public override double CastDelay => 0.1;
        public override double RequiredSkill => 30.0; // Required skill level to cast
        public override int RequiredMana => 15;

        private static readonly int feignDuration = 10; // Duration in seconds

        public FeignDeath(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.SendMessage("You collapse to the ground, feigning death!");

                // Play death animation and sound effect
                Caster.Animate(32, 5, 1, true, false, 0); // Collapse animation
                Caster.PlaySound(0x1FB); // Death sound

                // Apply effect: Enemies ignore the caster
                Caster.Hidden = true;
                Caster.Frozen = true; // Prevent movement

                // Timer to reset state after feign duration
                Timer.DelayCall(TimeSpan.FromSeconds(feignDuration), () => EndFeignDeath(Caster));
            }

            FinishSequence();
        }

        private void EndFeignDeath(Mobile caster)
        {
            if (caster == null || caster.Deleted)
                return;

            caster.SendMessage("You get back up, ending your feigned death!");
            caster.Hidden = false; // Become visible again
            caster.Frozen = false; // Allow movement again

            // Play a sound and effect for waking up
            Effects.PlaySound(caster.Location, caster.Map, 0x5C1);
            caster.FixedEffect(0x373A, 10, 15); // Smoke effect on getting up
        }
    }
}
