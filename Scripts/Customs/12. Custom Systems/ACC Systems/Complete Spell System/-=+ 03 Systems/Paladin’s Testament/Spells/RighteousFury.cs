using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.ChivalryMagic
{
    public class RighteousFury : ChivalrySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Righteous Fury", "Divinitus Furor",
            // SpellCircle.Sixth, // Uncomment and adjust if using a spell circle
            21016, 9300,
            false
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; } // Adjust based on your system
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 75.0; } }
        public override int RequiredMana { get { return 35; } }

        public RighteousFury(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.PlaySound(0x208); // Play a sound effect
                Caster.FixedParticles(0x375A, 10, 15, 5017, EffectLayer.Waist); // Visual effect

                Caster.SendMessage("You are filled with righteous fury!");

                // Apply buffs: increase damage and simulate critical hit chance
                ApplyDamageIncreaseBuff(Caster, 25, TimeSpan.FromSeconds(10));

                // Grant immunity to crowd control effects (using a buff or custom logic)
                Caster.SendMessage("You are immune to crowd control effects for a short duration.");

                // Timer for buff duration (10 seconds)
                Timer.DelayCall(TimeSpan.FromSeconds(10), () =>
                {
                    Caster.SendMessage("Your righteous fury fades.");
                    RemoveDamageIncreaseBuff(Caster);

                    // Revert Crowd Control Immunity (if applicable)
                    Caster.SendMessage("You are no longer immune to crowd control effects.");
                });
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.5);
        }

        private void ApplyDamageIncreaseBuff(Mobile target, int damageIncrease, TimeSpan duration)
        {
            // You will need to create a custom damage buff class or logic to apply the damage increase
            target.SendMessage($"Damage increased by {damageIncrease}% for {duration.TotalSeconds} seconds.");
            // Implement the logic to actually increase damage here
        }

        private void RemoveDamageIncreaseBuff(Mobile target)
        {
            // Implement the logic to revert the damage increase here
            target.SendMessage("Damage increase effect has been removed.");
        }
    }
}
