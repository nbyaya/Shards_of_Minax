using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.FencingMagic
{
    public class QuickReflexes : FencingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Quick Reflexes", "Fortis Agilis",
            21005, 9301,
            false,
            Reagent.Ginseng,
            Reagent.SpidersSilk
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 20.0; } }
        public override int RequiredMana { get { return 20; } }

        public QuickReflexes(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Mobile caster = Caster;
                double duration = 10.0 + (caster.Skills[SkillName.Anatomy].Value * 0.2); // Duration scales with Anatomy skill
                int dexBonus = (int)(caster.RawDex * 0.35); // Increase dex by 35%

                caster.SendMessage("You feel your reflexes quicken!");
                caster.PlaySound(0x1F2);
                caster.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist); // Flashy particle effect around the waist

                BuffInfo.AddBuff(caster, new BuffInfo(BuffIcon.Agility, 1075846, 1075847, TimeSpan.FromSeconds(duration), caster)); // Display buff icon

                caster.RawDex += dexBonus; // Apply dexterity increase

                Timer.DelayCall(TimeSpan.FromSeconds(duration), () =>
                {
                    caster.RawDex -= dexBonus; // Remove dexterity increase after duration
                    caster.SendMessage("Your reflexes return to normal.");
                    caster.PlaySound(0x1F8); // Play sound to indicate buff expiration
                });
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.0);
        }
    }
}
