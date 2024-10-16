using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.NinjitsuMagic
{
    public class ShadowWalk : NinjitsuSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Shadow Walk", "Ninja Shadow Walk",
            21004, // Spell Icon
            9300  // Spell Sound
        );

        public override SpellCircle Circle => SpellCircle.Third;

        public override double CastDelay => 1.5;
        public override double RequiredSkill => 60.0;
        public override int RequiredMana => 20;

        private static readonly TimeSpan EffectDuration = TimeSpan.FromSeconds(10.0);

        public ShadowWalk(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (!Caster.CanBeginAction(typeof(ShadowWalk)))
            {
                Caster.SendMessage("You cannot use Shadow Walk again so soon.");
                return;
            }

            if (CheckSequence())
            {
                // Consume the scroll, if applicable
                if (Scroll != null)
                    Scroll.Consume();

                // Play the casting animation and sound
                Caster.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist);
                Caster.PlaySound(0x482);

                // Apply the Shadow Walk effect
                Caster.Hidden = true; // Makes the caster invisible
                Caster.CantWalk = false; // Allows the caster to walk through obstacles
                
                Timer.DelayCall(EffectDuration, EndEffect);

                Caster.SendMessage("You move through the shadows, becoming nearly invisible and phasing through obstacles.");

                // Set a cooldown period for re-use
                Caster.BeginAction(typeof(ShadowWalk));
                Timer.DelayCall(TimeSpan.FromSeconds(30.0), () => Caster.EndAction(typeof(ShadowWalk)));
            }

            FinishSequence();
        }

        private void EndEffect()
        {
            Caster.Hidden = false; // Restore visibility
            Caster.CantWalk = true; // Disable walking through obstacles

            // Play an end effect to signify the effect has worn off
            Caster.FixedParticles(0x375A, 10, 15, 5024, EffectLayer.Waist);
            Caster.PlaySound(0x47D);

            Caster.SendMessage("The shadows release you, and you return to normal.");
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.5);
        }
    }
}
