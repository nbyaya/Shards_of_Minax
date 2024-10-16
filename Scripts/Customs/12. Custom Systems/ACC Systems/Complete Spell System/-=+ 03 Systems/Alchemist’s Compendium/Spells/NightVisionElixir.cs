using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Items;
using Server.Misc;
using Server;

namespace Server.ACC.CSS.Systems.AlchemyMagic
{
    public class NightVisionElixir : AlchemySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Night Vision Elixir", "In Vas Xen Bal",
                                                        21005,
                                                        9301,
                                                        false
                                                       );

        public override SpellCircle Circle { get { return SpellCircle.First; } }
        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 0.0; } } // No skill requirement
        public override int RequiredMana { get { return 20; } }

        public NightVisionElixir(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Mobile caster = Caster;

                if (caster == null || caster.Deleted)
                {
                    return;
                }

                // Apply night vision effect
                caster.SendMessage("You feel your senses sharpen as you drink the elixir.");
                caster.LightLevel = 25; // Maximum light level

                Effects.PlaySound(caster.Location, caster.Map, 0x1E3); // Play a sound effect
                caster.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist); // Visual effect around the caster

                Timer.DelayCall(TimeSpan.FromSeconds(30), () => RemoveNightVision(caster)); // Remove effect after 30 seconds
            }

            FinishSequence();
        }

        private void RemoveNightVision(Mobile caster)
        {
            if (caster == null || caster.Deleted)
            {
                return;
            }

            caster.LightLevel = 0; // Restore original light level
            caster.SendMessage("Your night vision fades away.");
        }
    }
}
