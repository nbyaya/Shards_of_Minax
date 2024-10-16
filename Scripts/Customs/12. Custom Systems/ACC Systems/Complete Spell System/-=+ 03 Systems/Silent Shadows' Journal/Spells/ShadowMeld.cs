using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Items;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.StealthMagic
{
    public class ShadowMeld : StealthSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Shadow Meld", "Velox Umbra",
                                                        21001,
                                                        9200,
                                                        false,
                                                        Reagent.Nightshade,
                                                        Reagent.BlackPearl
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.5; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 30; } }

        public ShadowMeld(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (Caster.Hidden || Caster is BaseCreature)
            {
                Caster.SendLocalizedMessage(1063087); // You are already hidden!
            }
            else if (CheckSequence())
            {
                Caster.Hidden = true; // Make the caster invisible
                Caster.PlaySound(0x3C4); // Play a stealth sound effect
                Caster.SendMessage("You meld into the shadows, becoming invisible.");

                // Apply visual effects
                Effects.SendLocationParticles(EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration), 0x376A, 1, 14, 1109, 0, 9962, 0); // Purple smoke effect
                Effects.SendTargetParticles(Caster, 0x376A, 10, 15, 9942, 0, 0, EffectLayer.Waist, 0); // Additional effect around the caster

                Timer.DelayCall(TimeSpan.FromSeconds(10), () => EndEffect(Caster)); // Schedule the end of invisibility
            }

            FinishSequence();
        }

        private void EndEffect(Mobile caster)
        {
            if (caster.Hidden && caster.Alive)
            {
                caster.RevealingAction(); // Reveal the caster
                caster.SendMessage("You fade back into view as the shadow meld dissipates.");
                Effects.SendTargetParticles(caster, 0x3735, 1, 15, 9909, 0, 0, EffectLayer.Waist, 0); // Subtle fade-out effect
            }
        }
    }
}
