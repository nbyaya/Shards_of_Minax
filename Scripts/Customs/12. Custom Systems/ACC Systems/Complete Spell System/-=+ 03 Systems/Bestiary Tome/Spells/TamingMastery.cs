using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Items;
using Server.Misc;

namespace Server.ACC.CSS.Systems.AnimalLoreMagic
{
    public class TamingMastery : AnimalLoreSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Taming Mastery", "Tame Animals",
            21005, // Effect ID for animation
            9400 // Sound ID for audio effect
        );

        public override SpellCircle Circle => SpellCircle.Third; // Adjust this if needed

        public override double CastDelay => 0.1;
        public override double RequiredSkill => 50.0; // Example: Adjust if a different skill level is required
        public override int RequiredMana => 20; // Mana cost as specified in the description

        public TamingMastery(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Apply temporary Animal Taming skill increase
                int skillBonus = 30;
                TimeSpan duration = TimeSpan.FromSeconds(30); // Example duration of 30 seconds

                Caster.SendMessage("You feel a surge of taming mastery flowing through you!");

                Effects.PlaySound(Caster.Location, Caster.Map, 9400); // Sound effect
                Caster.FixedParticles(21005, 10, 30, 5052, EffectLayer.Waist); // Visual effect

                // Assuming BuffIcon has a valid value instead of AdvancedTaming
                BuffInfo.AddBuff(Caster, new BuffInfo(
                    BuffIcon.Bless, // Replace with a valid icon
                    1075639,
                    1075640,
                    duration,
                    Caster,
                    false // Replace with an appropriate boolean if needed
                ));

                Caster.Skills[SkillName.AnimalTaming].Base += skillBonus;

                Timer.DelayCall(duration, () =>
                {
                    if (Caster != null && !Caster.Deleted)
                    {
                        Caster.SendMessage("Your taming mastery fades.");
                        Caster.Skills[SkillName.AnimalTaming].Base -= skillBonus;
                    }
                });
            }

            FinishSequence();
        }

        private string ToHtmlString(int hue, string text)
        {
            return String.Format("<BASEFONT COLOR=#{0:X6}>{1}</BASEFONT>", hue, text);
        }
    }
}
