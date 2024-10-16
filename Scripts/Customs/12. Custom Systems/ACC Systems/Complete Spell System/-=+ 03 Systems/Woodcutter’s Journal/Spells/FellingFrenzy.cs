using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections;

namespace Server.ACC.CSS.Systems.LumberjackingMagic
{
    public class FellingFrenzy : LumberjackingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Felling Frenzy", "Rage of the Woods",
            21013, // Icon
            9313,  // Action ID
            true,  // Allow Reagents
            Reagent.SulfurousAsh
        );

        public override SpellCircle Circle { get { return SpellCircle.Second; } }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 30; } }

        private static TimeSpan EffectDuration = TimeSpan.FromSeconds(10.0);
        private static double AttackSpeedBonus = 0.3; // 30% faster
        private static double DamageBonus = 0.25; // 25% more damage

        public FellingFrenzy(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (!Caster.CanBeginAction(typeof(FellingFrenzy)))
            {
                Caster.SendMessage("You are already under the effect of Felling Frenzy!");
                return;
            }

            if (CheckSequence())
            {
                // Apply the buff
                Caster.BeginAction(typeof(FellingFrenzy));
                Caster.SendMessage("You enter a frenzied state, your attacks with axes are faster and more powerful!");

                // Visual and sound effects
                Caster.FixedParticles(0x3728, 1, 30, 9950, 37, 0, EffectLayer.Waist); // Visual effect
                Caster.PlaySound(0x1F7); // Sound effect

                // Apply temporary bonuses
                ApplyBonuses(Caster);

                Timer.DelayCall(EffectDuration, () => EndEffect(Caster));
                FinishSequence();
            }
        }

        private void ApplyBonuses(Mobile caster)
        {
            caster.Hits += 10; // Small health boost

            // Temporarily increase attack speed and damage by adjusting properties or using a timer
            Timer bonusTimer = new BonusTimer(caster, DamageBonus, AttackSpeedBonus);
            bonusTimer.Start();
        }

        private void EndEffect(Mobile caster)
        {
            caster.EndAction(typeof(FellingFrenzy));
            caster.SendMessage("The frenzy fades, and you feel your strength return to normal.");

            // Remove bonuses
            caster.Hits -= 10; // Remove health boost
        }

        private class BonusTimer : Timer
        {
            private Mobile m_Caster;
            private double m_DamageBonus;
            private double m_AttackSpeedBonus;

            public BonusTimer(Mobile caster, double damageBonus, double attackSpeedBonus) 
                : base(TimeSpan.Zero, TimeSpan.FromSeconds(1.0)) // Adjust the interval if needed
            {
                m_Caster = caster;
                m_DamageBonus = damageBonus;
                m_AttackSpeedBonus = attackSpeedBonus;
            }

            protected override void OnTick()
            {
                if (m_Caster == null || !m_Caster.Alive)
                {
                    Stop();
                    return;
                }

                // Check if the caster is using an axe and has the required lumberjacking skill
                if (m_Caster.Weapon is BaseAxe && m_Caster.Skills[SkillName.Lumberjacking].Base >= 50.0)
                {
                    // Apply damage bonus logic when the player hits an enemy
                    // This part will depend on your custom logic
                }

                // Optionally adjust attack speed directly or implement speed bonus here
            }
        }
    }
}
