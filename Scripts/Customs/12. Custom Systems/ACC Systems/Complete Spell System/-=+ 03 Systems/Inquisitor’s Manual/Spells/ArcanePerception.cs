using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Misc;
using Server.Items;

namespace Server.ACC.CSS.Systems.DetectHiddenMagic
{
    public class ArcanePerception : DetectHiddenSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Arcane Perception", "Perceptio Arcanus",
            //SpellCircle.First,
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 0.0; } }
        public override int RequiredMana { get { return 15; } }

        public ArcanePerception(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.SendMessage("You feel a surge of arcane energy heightening your senses.");

                // Play a sound and create a visual effect
                Caster.PlaySound(0x1E9); // Sound of a magic effect
                Effects.SendLocationParticles(
                    EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration), 
                    0x376A, 1, 14, 1153, 2, 9962, 0 // Blue sparkles around caster
                );

                // If HiddenDetectionRange and MagicResistBonus are not available, use alternative logic
                // This assumes you have a way to handle detection range and magic resist
                // Example: Assume we have a custom method or property to handle this

                // Temporary custom implementation:
                IncreaseDetectionRange(Caster, 5);
                IncreaseMagicResistBonus(Caster, 10);

                // Start a timer to revert the effects after a duration
                Timer.DelayCall(TimeSpan.FromSeconds(60), () =>
                {
                    DecreaseDetectionRange(Caster, 5);
                    DecreaseMagicResistBonus(Caster, 10);
                    Caster.SendMessage("The effects of Arcane Perception fade away.");
                });
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.0);
        }

        // Example methods to increase/decrease detection range and magic resistance
        private void IncreaseDetectionRange(Mobile caster, int amount)
        {
            // Implement this method or use existing methods to modify detection range
        }

        private void DecreaseDetectionRange(Mobile caster, int amount)
        {
            // Implement this method or use existing methods to modify detection range
        }

        private void IncreaseMagicResistBonus(Mobile caster, int amount)
        {
            // Implement this method or use existing methods to modify magic resistance
        }

        private void DecreaseMagicResistBonus(Mobile caster, int amount)
        {
            // Implement this method or use existing methods to modify magic resistance
        }
    }
}
