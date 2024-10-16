using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.ACC.CSS.Systems.HidingMagic
{
    public class CloakOfTheNight : HidingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Cloak of the Night", "Umbra Tenebrae",
            21005, // Placeholder for the sound effect ID
            9204,  // Placeholder for the visual effect ID
            false,
            Reagent.BlackPearl,  // Example reagent, modify as needed
            Reagent.Nightshade   // Example reagent, modify as needed
        );

        public override SpellCircle Circle { get { return SpellCircle.Third; } }
		public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }

        public CloakOfTheNight(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Mobile caster = Caster;
                caster.SendMessage("You are enveloped by a cloak of shadows.");
                caster.PlaySound(0x5C3); // Placeholder for sound effect
                caster.FixedParticles(0x374A, 10, 15, 5038, EffectLayer.Head); // Visual effect for casting

                int hidingBonus = (int)(caster.Skills[SkillName.Hiding].Base * 0.2); // 20% hiding skill increase
                double duration = 30.0 + caster.Skills[SkillName.Hiding].Base * 0.1; // Base 30 seconds, +1 second per 10 hiding skill points

                caster.Skills[SkillName.Hiding].Base += hidingBonus;

                Timer.DelayCall(TimeSpan.FromSeconds(duration), () =>
                {
                    if (caster != null && !caster.Deleted)
                    {
                        caster.Skills[SkillName.Hiding].Base -= hidingBonus;
                        caster.SendMessage("The cloak of shadows fades away.");
                        caster.PlaySound(0x5C4); // Placeholder for sound effect when the effect ends
                    }
                });
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0); // 2 seconds casting delay
        }
    }
}
