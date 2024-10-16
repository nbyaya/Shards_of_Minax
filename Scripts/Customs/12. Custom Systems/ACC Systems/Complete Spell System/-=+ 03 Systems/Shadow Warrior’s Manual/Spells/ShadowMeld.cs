using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Misc;
using Server.Items;

namespace Server.ACC.CSS.Systems.NinjitsuMagic
{
    public class ShadowMeld : NinjitsuSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Shadow Meld", "Umbra Consocia",
            21004,
            9300,
            false,
            Reagent.Nightshade
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }

        public ShadowMeld(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Play initial visual and sound effects
                Caster.PlaySound(0x1FD); // Sound of melding with shadows
                Caster.FixedParticles(0x373A, 10, 15, 5036, EffectLayer.Waist); // Dark mist effect

                // Apply shadow meld effect
                Caster.Hidden = true; // Makes the caster untargetable
                Caster.SendMessage("You meld into the shadows, becoming untargetable and regaining some health.");

                // Schedule the end of the shadow meld effect
                Timer.DelayCall(TimeSpan.FromSeconds(5.0), () => EndShadowMeld(Caster));
                
                // Heal the caster for a portion of their health
                int healAmount = (int)(Caster.HitsMax * 0.2); // Heals 20% of maximum health
                Caster.Hits += healAmount;

                Caster.SendMessage($"You regain {healAmount} health while in the shadows.");
            }

            FinishSequence();
        }

        private void EndShadowMeld(Mobile caster)
        {
            if (caster != null && !caster.Deleted && caster.Hidden)
            {
                caster.Hidden = false; // Make the caster visible again
                caster.SendMessage("You emerge from the shadows.");
                
                // Play ending visual and sound effects
                caster.PlaySound(0x208); // Sound of emerging from shadows
                caster.FixedParticles(0x374A, 10, 15, 5036, EffectLayer.Waist); // Dark mist dissipates effect
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0);
        }
    }
}
