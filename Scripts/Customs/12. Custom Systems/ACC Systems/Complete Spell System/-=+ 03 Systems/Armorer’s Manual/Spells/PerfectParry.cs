using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.ACC.CSS.Systems.ArmsLoreMagic
{
    public class PerfectParry : ArmsLoreSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Perfect Parry", "Aegis Defensio",
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }

        public PerfectParry(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Apply the Perfect Parry effect
                Caster.SendMessage("You feel a surge of defensive power!");

                // Increase Parry skill temporarily
                Caster.Skills[SkillName.Parry].Base += 20.0; // Increase Parry skill by 20 points
                Timer.DelayCall(TimeSpan.FromSeconds(10.0), () => RestoreParrySkill(Caster, 20.0));

                // Play sound and show visual effects
                Effects.PlaySound(Caster.Location, Caster.Map, 0x1ED); // Play defensive sound
                Caster.FixedParticles(0x376A, 10, 15, 5037, EffectLayer.Waist); // Show parry visual effect
            }

            FinishSequence();
        }

        private void RestoreParrySkill(Mobile caster, double amount)
        {
            if (caster != null && !caster.Deleted)
            {
                caster.Skills[SkillName.Parry].Base -= amount;
                caster.SendMessage("Your enhanced parry skill fades away.");
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0);
        }
    }
}
