using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.ACC.CSS.Systems.AnimalLoreMagic
{
    public class BeastsInsight : AnimalLoreSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Beast's Insight", "Animus Cognitio",
            21005, 9400
        );

        public override SpellCircle Circle { get { return SpellCircle.First; } }
        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 0.0; } }
        public override int RequiredMana { get { return 10; } }

        public BeastsInsight(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.PlaySound(0x20E); // Sound effect for casting the spell
                Caster.FixedParticles(0x373A, 1, 30, 9964, 1153, 2, EffectLayer.Head); // Visual effect on the caster

                Timer.DelayCall(TimeSpan.FromSeconds(0.5), () => ApplyTamingBonus(Caster));
            }

            FinishSequence();
        }

        private void ApplyTamingBonus(Mobile caster)
        {
            if (caster == null || caster.Deleted)
                return;

            Skill taming = caster.Skills[SkillName.AnimalTaming];
            double originalValue = taming.Base;

            taming.Base += 20; // Temporary bonus to Animal Taming skill

            // Display a message to the player
            caster.SendMessage("You feel a deep connection with the beasts around you.");

            Timer.DelayCall(TimeSpan.FromSeconds(30.0), () => RemoveTamingBonus(caster, originalValue));
        }

        private void RemoveTamingBonus(Mobile caster, double originalValue)
        {
            if (caster == null || caster.Deleted)
                return;

            caster.Skills[SkillName.AnimalTaming].Base = originalValue; // Restore the original skill value

            // Display a message to the player
            caster.SendMessage("The connection fades, and your skill returns to normal.");
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.5);
        }
    }
}
