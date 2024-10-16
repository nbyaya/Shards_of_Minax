using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.CarpentryMagic
{
    public class WoodenShield : CarpentrySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Wooden Shield", "Oaken Shield",
            21004, // Animation
            9300,  // Sound
            false
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.2; } } // Delay in seconds
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }

        public WoodenShield(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Check if caster has enough mana
                if (Caster.Mana < RequiredMana)
                {
                    Caster.SendMessage("You do not have enough mana to cast this spell.");
                    return;
                }

                // Consume mana
                Caster.Mana -= RequiredMana;

                // Apply shield buff
                Caster.VirtualArmorMod += 10; // Boost defense by 10 points
                Caster.SendMessage("You feel the protection of a wooden shield!");

                // Visual effects
                Caster.FixedParticles(0x373A, 10, 15, 5036, EffectLayer.Waist); // Green sparkles around the caster
                Caster.PlaySound(0x1F0); // Play wood creaking sound

                // Timer to remove shield after duration
                Timer.DelayCall(TimeSpan.FromSeconds(10.0), () => RemoveShield(Caster));
            }

            FinishSequence();
        }

        private void RemoveShield(Mobile caster)
        {
            if (caster != null && caster.VirtualArmorMod > 0)
            {
                caster.VirtualArmorMod -= 10; // Remove the defense boost
                caster.SendMessage("The wooden shield fades away.");
                caster.FixedParticles(0x3735, 1, 30, 9502, EffectLayer.Waist); // Smoke dissipating effect
                caster.PlaySound(0x1F8); // Play sound of shield breaking
            }
        }
    }
}
