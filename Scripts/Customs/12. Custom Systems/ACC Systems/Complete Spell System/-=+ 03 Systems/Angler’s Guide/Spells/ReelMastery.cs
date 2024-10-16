using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.FishingMagic
{
    public class ReelMastery : FishingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Reel Mastery", "Reel Faster!",
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 20.0; } }
        public override int RequiredMana { get { return 15; } }

        public ReelMastery(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.SendMessage("You feel a surge of energy as you master the art of reeling!");
                Caster.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist); // Visual effect
                Caster.PlaySound(0x1F5); // Sound effect

                // Apply the effect of reducing the time it takes to catch fish
                Caster.SendMessage("Your reeling speed has improved!");
                // Here you would implement the logic to reduce the fishing time
                // Since this is a placeholder, we'll just send a message
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(5.0);
        }
    }
}
