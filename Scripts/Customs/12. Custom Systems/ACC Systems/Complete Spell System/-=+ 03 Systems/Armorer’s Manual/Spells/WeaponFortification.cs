using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.ACC.CSS.Systems.ArmsLoreMagic
{
    public class WeaponFortification : ArmsLoreSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Weapon Fortification", "Fortis Arma",
                                                        21004, // Spell icon ID
                                                        9300,  // Effect sound ID
                                                        false
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override double CastDelay { get { return 0.2; } } // 2 seconds delay
        public override double RequiredSkill { get { return 50.0; } } // Requires 50.0 skill in magic
        public override int RequiredMana { get { return 20; } } // Costs 20 mana

        public WeaponFortification(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Ensure the caster is a player
                if (Caster is PlayerMobile player)
                {
                    // Apply visual and sound effects
                    Effects.PlaySound(Caster.Location, Caster.Map, 0x1FA); // Play spell casting sound
                    Caster.FixedParticles(0x373A, 1, 30, 9503, 92, 3, EffectLayer.Waist); // Glowing particles around caster
                    Caster.FixedParticles(0x3779, 1, 15, 9502, 67, 4, EffectLayer.Waist); // Additional particles for a more flashy effect

                    // Calculate the dexterity boost
                    int dexterityBoost = (int)(player.Dex * 0.3); // 30% increase
                    int duration = 30; // Buff duration in seconds

                    // Apply the temporary dexterity boost
                    player.SendMessage(0x35, "Your dexterity has been temporarily increased!");
                    player.Dex += dexterityBoost;

                    // Start a timer to remove the dexterity boost after the duration
                    Timer.DelayCall(TimeSpan.FromSeconds(duration), () =>
                    {
                        if (player != null && !player.Deleted && player.Alive)
                        {
                            player.Dex -= dexterityBoost;
                            player.SendMessage(0x35, "Your dexterity boost has worn off.");
                            Effects.PlaySound(player.Location, player.Map, 0x1F8); // Play effect sound when buff ends
                            player.FixedParticles(0x373A, 1, 15, 9502, 92, 3, EffectLayer.Waist); // Final visual effect
                        }
                    });
                }
            }

            FinishSequence();
        }
    }
}
