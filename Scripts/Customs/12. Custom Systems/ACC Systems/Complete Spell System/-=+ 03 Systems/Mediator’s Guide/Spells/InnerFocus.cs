using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;
using Server.Targeting;

namespace Server.ACC.CSS.Systems.MeditationMagic
{
    public class InnerFocus : MeditationSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Inner Focus", "In Vol Ner",
            21004,
            9300,
            false,
            Reagent.BlackPearl
        );

        public override SpellCircle Circle => SpellCircle.Fourth; // Example Circle, adjust if necessary
        public override double CastDelay => 1.5;
        public override double RequiredSkill => 50.0;
        public override int RequiredMana => 15;

        public InnerFocus(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Apply effects
                Caster.PlaySound(0x1F5); // Play sound effect
                Caster.FixedParticles(0x375A, 10, 15, 5018, EffectLayer.Waist); // Visual effect

                // Buff duration based on skill level
                double duration = 10.0 + (Caster.Skills[CastSkill].Value * 0.1);
                BuffInfo.AddBuff(Caster, new BuffInfo(BuffIcon.Bless, 1075810, 1075811, TimeSpan.FromSeconds(duration), Caster)); // Display a buff icon

                // Increase accuracy and critical hit chance
                Caster.SendMessage("You feel a surge of focus and energy!");

                Timer buffTimer = new InnerFocusTimer(Caster, duration);
                buffTimer.Start();
            }

            FinishSequence();
        }

        private class InnerFocusTimer : Timer
        {
            private Mobile m_Caster;
            private double m_OriginalAccuracyBonus;
            private double m_OriginalCritChanceBonus;

            public InnerFocusTimer(Mobile caster, double duration) : base(TimeSpan.FromSeconds(duration))
            {
                m_Caster = caster;
                Priority = TimerPriority.TwoFiftyMS;

                m_OriginalAccuracyBonus = m_Caster.GetStatMod("AccuracyBonus")?.Offset ?? 0;
                m_OriginalCritChanceBonus = m_Caster.GetStatMod("CritChanceBonus")?.Offset ?? 0;

                m_Caster.AddStatMod(new StatMod(StatType.Dex, "AccuracyBonus", (int)(m_OriginalAccuracyBonus + 15), TimeSpan.FromSeconds(duration))); // Increase accuracy
                m_Caster.AddStatMod(new StatMod(StatType.Int, "CritChanceBonus", (int)(m_OriginalCritChanceBonus + 10), TimeSpan.FromSeconds(duration))); // Increase critical hit chance
            }

            protected override void OnTick()
            {
                m_Caster.SendMessage("Your inner focus fades away.");
                m_Caster.RemoveStatMod("AccuracyBonus");
                m_Caster.RemoveStatMod("CritChanceBonus");

                Stop();
            }
        }
    }
}
