using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.ArmsLoreMagic
{
    public class VeteransInsight : ArmsLoreSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Veteran's Insight", "Cognitio Armis",
            //SpellCircle.Third,
            21004,
            9300,
            false,
            Reagent.BlackPearl,
            Reagent.SpidersSilk
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 30; } }

        private static TimeSpan BuffDuration = TimeSpan.FromSeconds(30.0); // Duration of the skill boost
        private static int SkillBoostAmount = 20; // Amount of Arms Lore skill boost

        public VeteransInsight(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.SendMessage("You feel a surge of insight into combat tactics!");

                // Visual and sound effects
                Caster.PlaySound(0x64F); // Sound effect
                Caster.FixedParticles(0x375A, 10, 15, 5010, EffectLayer.Waist); // Visual effect

                // Apply the skill boost
                Caster.Skills[SkillName.ArmsLore].Base += SkillBoostAmount;
                Caster.SendMessage("Your Arms Lore skill has been temporarily boosted!");

                Timer.DelayCall(BuffDuration, () => RemoveSkillBoost(Caster)); // Set timer to remove skill boost
            }

            FinishSequence();
        }

        private void RemoveSkillBoost(Mobile caster)
        {
            if (caster == null || caster.Deleted)
                return;

            caster.Skills[SkillName.ArmsLore].Base -= SkillBoostAmount;
            caster.SendMessage("Your Arms Lore skill boost has worn off.");
            
            // Visual and sound effects for ending the boost
            caster.PlaySound(0x658); // Sound effect
            caster.FixedParticles(0x373A, 10, 15, 5010, EffectLayer.Head); // Visual effect
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.5);
        }
    }
}
