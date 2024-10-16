using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;


namespace Server.ACC.CSS.Systems.HidingMagic
{
    public class GhostWalk : HidingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Ghost Walk", "In Corporealis",
            21007,
            9206
        );

        public override SpellCircle Circle => SpellCircle.Fourth;

        public override double CastDelay => 1.5; // Slight delay to cast the spell
        public override double RequiredSkill => 50.0; // Required skill level to cast the spell
        public override int RequiredMana => 30; // Mana cost for the spell

        public GhostWalk(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Apply the Ghost Walk effect
                Caster.SendMessage("You feel yourself becoming more agile...");
                Caster.FixedParticles(0x373A, 10, 15, 5010, EffectLayer.Waist); // New visual effect around the caster
                Caster.PlaySound(0x482); // Sound effect for the spell

                // Temporarily increase the caster's speed

                Timer.DelayCall(TimeSpan.FromSeconds(5.0), () => EndEffect(Caster)); // Timer to end effect after 5 seconds
            }

            FinishSequence();
        }

        private void EndEffect(Mobile caster)
        {
            if (caster == null || caster.Deleted) return;

            // Revert caster's speed to normal

            caster.SendMessage("You feel yourself slowing down to normal.");
            caster.FixedParticles(0x373A, 10, 15, 5010, EffectLayer.Waist); // Same visual effect when ending
            caster.PlaySound(0x482); // Same sound effect when ending
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(3.0); // Delay between consecutive casts
        }
    }
}
