using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;
using Server.Targeting;

namespace Server.ACC.CSS.Systems.LockpickingMagic
{
    public class QuickHands : LockpickingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Quick Hands", "Vas Yum Rel",
            21001, // Icon
            9301,  // Cast Animation
            false
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 10; } }

        public QuickHands(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Apply skill bonus effect
                Caster.SendMessage("Your hands move with incredible speed and dexterity!");
                Caster.PlaySound(0x1F5); // Play a sound effect for the spell cast
                Caster.FixedParticles(0x373A, 10, 15, 5012, EffectLayer.Waist); // Visual effect at the waist
                
                SkillMod skillMod = new DefaultSkillMod(SkillName.Lockpicking, true, 20); // +20 lockpicking skill
                Caster.AddSkillMod(skillMod);

                Timer.DelayCall(TimeSpan.FromSeconds(30.0), () =>
                {
                    // Remove the skill mod after duration expires
                    Caster.SendMessage("The effect of Quick Hands fades away.");
                    Caster.RemoveSkillMod(skillMod);
                });
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.5);
        }
    }
}
