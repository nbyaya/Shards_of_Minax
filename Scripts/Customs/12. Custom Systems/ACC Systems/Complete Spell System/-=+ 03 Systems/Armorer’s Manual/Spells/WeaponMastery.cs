using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.ArmsLoreMagic
{
    public class WeaponMastery : ArmsLoreSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Weapon Mastery", "Increased Tactics!",
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 25; } }

        public WeaponMastery(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if ( CheckSequence() )
            {
                // Apply the Tactics skill boost
                ApplyTacticsBoost(Caster);

                // Create visual and sound effects
                PlayEffects(Caster);

                FinishSequence();
            }
        }

        private void ApplyTacticsBoost(Mobile caster)
        {
            // Duration of the skill boost
            TimeSpan duration = TimeSpan.FromSeconds(30);

            // Boost value
            double boost = 20.0;

            // Apply the boost
            caster.Skills[SkillName.Tactics].Base += boost;

            // Schedule a timer to revert the skill boost after duration
            Timer.DelayCall(duration, () =>
            {
                if (caster != null)
                {
                    caster.Skills[SkillName.Tactics].Base -= boost;
                }
            });
        }

        private void PlayEffects(Mobile caster)
        {
            // Visual effect: Sparkles around the caster
            Effects.SendLocationEffect(caster.Location, caster.Map, 0x36D4, 30, 10, 0x0, 0x7F);

            // Sound effect: Victory fanfare
            caster.PlaySound(0x1F5);

            // Create a flashy light effect
            for (int i = 0; i < 10; i++)
            {
                int xOffset = Utility.Random(5) - 2;
                int yOffset = Utility.Random(5) - 2;
                Point3D location = new Point3D(caster.X + xOffset, caster.Y + yOffset, caster.Z);
                Effects.SendLocationEffect(location, caster.Map, 0x1F6, 20, 10, 0x0, 0x7F);
            }
        }
    }
}
