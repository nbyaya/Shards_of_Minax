using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.FishingMagic
{
    public class QuickRelease : FishingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Quick Release", "Release Fish",
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.5; } }
        public override double RequiredSkill { get { return 20.0; } }
        public override int RequiredMana { get { return 15; } }

        public QuickRelease(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.SendMessage("You quickly release the fish back into the water, preserving your bait.");
                Caster.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist); // Visual effect
                Caster.PlaySound(0x026); // Sound effect
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.0);
        }
    }
}
