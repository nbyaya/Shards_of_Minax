using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using System.Collections.Generic;


namespace Server.ACC.CSS.Systems.NinjitsuMagic
{
    public class EscapeArtist : NinjitsuSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Escape Artist", "Vos Ort Grav",
            //SpellCircle.First, // If using spell circles, uncomment this line
            21004, // Animation or effect ID for casting
            9300  // Sound effect ID for casting
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; } // Adjust circle level if necessary
        }

        public override double CastDelay { get { return 0.5; } } // Quick cast
        public override double RequiredSkill { get { return 20.0; } } // Lower skill requirement
        public override int RequiredMana { get { return 20; } } // Mana cost

        public EscapeArtist(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (!Caster.CanBeginAction(typeof(EscapeArtist)))
            {
                Caster.SendMessage("You cannot use this ability right now.");
                return;
            }

            if (CheckSequence())
            {
                // Remove crowd control effects
                Caster.Paralyzed = false;
                Caster.Frozen = false;

                // Remove custom buffs (you need to adapt this if using custom buff classes)
                // Example for a custom implementation: remove any custom buff effect
                // Caster.RemoveCustomBuff(); 

                // Apply speed burst
                Caster.SendMessage("You feel a burst of speed course through your veins!");
                Caster.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist); // Visual effect for speed burst
                Caster.PlaySound(0x1F7); // Sound effect for speed burst
                Caster.BeginAction(typeof(EscapeArtist));

                // Implementing haste effect. This example assumes that you have a `Timer` based approach.
                Timer.DelayCall(TimeSpan.FromSeconds(1.0), () =>
                {
                    // Implement speed burst here, example if you have a custom haste effect:
                    // Caster.AddHaste(TimeSpan.FromSeconds(5)); 

                    // If no haste effect exists, you can just increase movement speed directly
                    Caster.SendMessage("Your speed is increased!");
                    // Caster.MoveSpeed = 1.0; // Adjust as needed

                    Timer.DelayCall(TimeSpan.FromSeconds(5.0), () => 
                    {
                        // Reset speed after 5 seconds
                        // Caster.MoveSpeed = 1.0; // Adjust back to normal speed
                        Caster.SendMessage("Your speed returns to normal.");
                    });
                });

                // Cooldown for using the skill again
                Timer.DelayCall(TimeSpan.FromSeconds(30), () =>
                {
                    Caster.EndAction(typeof(EscapeArtist));
                    Caster.SendMessage("You can use Escape Artist again.");
                });
            }

            FinishSequence();
        }
    }
}
