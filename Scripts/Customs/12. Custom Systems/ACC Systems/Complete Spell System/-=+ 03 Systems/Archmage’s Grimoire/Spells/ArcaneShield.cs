using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;
using Server.Targeting;

namespace Server.ACC.CSS.Systems.MageryMagic
{
    public class ArcaneShield : MagerySpell
    {
        private static readonly SpellInfo m_Info = new SpellInfo(
            "Arcane Shield", "In Vas An Flam",
            //SpellCircle.Fourth,
            21004,
            9300,
            false,
            Reagent.BlackPearl,
            Reagent.MandrakeRoot
        );

        public override SpellCircle Circle => SpellCircle.Fourth;

        public override double CastDelay => 1.5;
        public override double RequiredSkill => 50.0;
        public override int RequiredMana => 20;

        private const int SkillBoostDuration = 30; // Duration in seconds
        private const double SkillBoostAmount = 20.0; // Amount of skill boost

        public ArcaneShield(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.FixedParticles(0x375A, 10, 15, 5013, EffectLayer.Waist); // Visual effect
                Caster.PlaySound(0x1F7); // Sound effect

                Timer.DelayCall(TimeSpan.FromSeconds(1.0), () =>
                {
                    Caster.SendMessage("You feel protected by the Arcane Shield!"); // Message to the caster
                    IncreaseMagicResistanceSkill(Caster, SkillBoostAmount);

                    Timer.DelayCall(TimeSpan.FromSeconds(SkillBoostDuration), () =>
                    {
                        DecreaseMagicResistanceSkill(Caster, SkillBoostAmount);
                        Caster.SendMessage("The Arcane Shield effect has worn off."); // Message to the caster
                    });
                });
            }

            FinishSequence();
        }

        private void IncreaseMagicResistanceSkill(Mobile caster, double amount)
        {
            if (caster == null || caster.Deleted)
                return;

            caster.Skills[SkillName.MagicResist].Base += amount;
            caster.SendMessage("Your magic resistance skill temporarily increases.");
        }

        private void DecreaseMagicResistanceSkill(Mobile caster, double amount)
        {
            if (caster == null || caster.Deleted)
                return;

            caster.Skills[SkillName.MagicResist].Base -= amount;
            caster.SendMessage("Your magic resistance skill returns to normal.");
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0);
        }
    }
}
