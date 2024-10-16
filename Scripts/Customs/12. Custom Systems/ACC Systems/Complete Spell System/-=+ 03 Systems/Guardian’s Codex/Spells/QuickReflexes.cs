using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Items;


namespace Server.ACC.CSS.Systems.ParryMagic
{
    public class QuickReflexes : ParrySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Quick Reflexes", "Tempus Agilis",
            21005,
            9301,
            false,
            Reagent.BlackPearl
        );

        public override SpellCircle Circle { get { return SpellCircle.Fourth; } }
        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 15; } }

        public QuickReflexes(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist); // Flashy particle effect
                Caster.PlaySound(0x1F8); // Play sound effect
                
                int dexBonus = (int)(Caster.Dex * 0.35);
                Caster.SendMessage("Your reflexes quicken as your dexterity increases!");

                // Apply Dexterity boost
                Caster.Dex += dexBonus;

                // Duration of the effect
                Timer.DelayCall(TimeSpan.FromSeconds(30.0), () =>
                {
                    // Remove Dexterity boost after duration
                    if (Caster != null && !Caster.Deleted)
                    {
                        Caster.Dex -= dexBonus;
                        Caster.SendMessage("The effect of Quick Reflexes wears off.");
                    }
                });
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(3.0);
        }
    }
}
