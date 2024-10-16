using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.TrackingMagic
{
    public class QuickReflexes : TrackingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Quick Reflexes", "Velox Motus",
            21004,
            9300,
            false,
            Reagent.Bloodmoss,
            Reagent.SpidersSilk
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 20.0; } }
        public override int RequiredMana { get { return 25; } }

        public QuickReflexes(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.SendMessage("You feel a surge of energy flowing through your body!");
                Caster.FixedParticles(0x373A, 10, 30, 5036, EffectLayer.Waist); // Speed boost visual effect
                Caster.PlaySound(0x1E7); // Sound effect for activation

                // Buff the player with increased speed and agility
                Caster.SendMessage("Your reflexes sharpen and your movements quicken!");

                // Increase Dexterity
                Caster.Dex += 10;

                // To simulate speed increase, you might need to use skills or movement adjustment
                // Assuming you have a way to change movement speed
                Timer.DelayCall(TimeSpan.FromSeconds(10), () =>
                {
                    // Restore player's original dexterity after the effect duration
                    Caster.Dex -= 10;
                    Caster.SendMessage("The effect of Quick Reflexes fades away.");
                    Caster.FixedParticles(0x373A, 10, 30, 5036, EffectLayer.Waist); // Visual effect for fading
                    Caster.PlaySound(0x1F8); // Sound effect for fade
                });
            }

            FinishSequence();
        }
    }
}
