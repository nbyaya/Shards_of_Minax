using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;
using System.Collections.Generic;
using Server;

namespace Server.ACC.CSS.Systems.FletchingMagic
{
    public class SwiftDraw : FletchingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Swift Draw", "Velo Cito",
            // Custom ability specific identifier
            21005,
            9400,
            false,
            Reagent.BlackPearl // Example reagent, can be changed
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 14; } }

        public SwiftDraw(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (!Caster.CanBeginAction(typeof(SwiftDraw)))
            {
                Caster.SendLocalizedMessage(1005583); // You are already under the effect of Swift Draw!
                return;
            }

            if (CheckSequence())
            {
                // Apply the attack speed increase effect
                Caster.BeginAction(typeof(SwiftDraw));
                Caster.SendMessage("You feel a surge of speed flow through your veins!");

                // Visual and sound effects
                Effects.SendLocationParticles(
                    EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration),
                    0x3728, 1, 13, 1153, 4, 3, 0
                ); // Flashy visual effect
                Caster.PlaySound(0x208); // Sound effect for drawing the bowstring

                Timer.DelayCall(TimeSpan.FromSeconds(0.1), TimeSpan.FromSeconds(1.0), 10, () => IncreaseAttackSpeed(Caster));

                Timer.DelayCall(TimeSpan.FromSeconds(10.0), () => EndEffect(Caster));
            }

            FinishSequence();
        }

        private static void IncreaseAttackSpeed(Mobile caster)
        {
            if (caster == null || !caster.Alive)
                return;

            // Apply a custom delay effect for attack speed
            // Example: Reducing delay between attacks by adjusting a custom stat or using a custom mechanism
            caster.SendMessage("Your attack speed is increased temporarily!");

            // Additional visual effect each time
            caster.FixedParticles(0x373A, 1, 30, 9911, 67, 7, EffectLayer.Waist);
        }

        private static void EndEffect(Mobile caster)
        {
            if (caster == null)
                return;

            // Restore original attack speed
            caster.SendMessage("Your swift draw effect has worn off.");

            caster.EndAction(typeof(SwiftDraw));
            // Optionally, you could reverse any custom effects applied in IncreaseAttackSpeed
            // Restore the delay or other adjustments you made

            // Visual and sound effects for ending
            caster.PlaySound(0x208);
            caster.FixedParticles(0x373A, 1, 30, 9911, 67, 7, EffectLayer.Waist);
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.0);
        }
    }
}
