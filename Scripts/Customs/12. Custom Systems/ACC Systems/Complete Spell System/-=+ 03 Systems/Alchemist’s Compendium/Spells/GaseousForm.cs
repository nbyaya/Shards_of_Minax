using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;
using Server.Targeting;

namespace Server.ACC.CSS.Systems.AlchemyMagic
{
    public class GaseousForm : AlchemySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Gaseous Form", "In An Corp",
            // SpellCircle.Sixth,
            21005,
            9301
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 35; } }

        public GaseousForm(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.SendMessage("You feel your body becoming light as air.");
                Caster.PlaySound(0x64A); // Play a transformation sound
                Caster.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Head); // Visual effect of turning into gas

                Caster.Hidden = true; // Make the caster hidden to simulate becoming gaseous
                Caster.CantWalk = true; // Prevent normal movement

                Timer.DelayCall(TimeSpan.FromSeconds(10), new TimerStateCallback(Revert), Caster); // Revert after 10 seconds
            }

            FinishSequence();
        }

        private void Revert(object state)
        {
            Mobile caster = (Mobile)state;

            if (caster != null && !caster.Deleted)
            {
                caster.Hidden = false; // Reveal the caster
                caster.CantWalk = false; // Allow normal movement

                caster.SendMessage("You feel your body becoming solid again.");
                caster.PlaySound(0x64C); // Play a reverting sound
                caster.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Head); // Visual effect of returning to normal form
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0);
        }
    }
}
