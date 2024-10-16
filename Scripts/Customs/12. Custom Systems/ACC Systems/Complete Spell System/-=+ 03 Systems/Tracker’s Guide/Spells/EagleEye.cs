using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.TrackingMagic
{
    public class EagleEye : TrackingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Eagle Eye", "Bacus Tracus",
            21004, // Icon ID for the spell
            9300,  // Sound ID for the spell
            false,  // Allow targeting
            Reagent.Bloodmoss
        );

        private static Dictionary<Mobile, int> _detectionRanges = new Dictionary<Mobile, int>();

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 20; } }

        public EagleEye(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (!Caster.CanBeginAction(typeof(EagleEye)))
            {
                Caster.SendMessage("You are already using Eagle Eye.");
                return;
            }

            if (CheckSequence())
            {
                Caster.BeginAction(typeof(EagleEye));
                Caster.SendMessage("Your vision sharpens, allowing you to detect hidden or distant targets.");

                // Apply visual and sound effects
                Effects.SendLocationParticles(EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5008);
                Effects.PlaySound(Caster.Location, Caster.Map, 0x1F2);
                Caster.FixedParticles(0x376A, 10, 15, 5013, EffectLayer.Waist);

                // Buff detection range for a duration
                double skill = Caster.Skills[SkillName.Tracking].Value;
                int rangeBonus = (int)(skill / 10); // Range increase based on Tracking skill
                Caster.SendMessage("Your detection range is increased by {0} tiles.", rangeBonus);

                // Set the detection range
                SetDetectionRange(Caster, GetDetectionRange(Caster) + rangeBonus);

                // Schedule end of the effect
                Timer.DelayCall(TimeSpan.FromSeconds(30.0), EndEagleEyeEffect, Caster, rangeBonus);
            }

            FinishSequence();
        }

        private static void EndEagleEyeEffect(Mobile caster, int rangeBonus)
        {
            if (caster == null || caster.Deleted || !caster.CanBeginAction(typeof(EagleEye)))
                return;

            caster.EndAction(typeof(EagleEye));

            // Revert the detection range
            SetDetectionRange(caster, GetDetectionRange(caster) - rangeBonus);
            caster.SendMessage("The effect of Eagle Eye fades, and your vision returns to normal.");
        }

        // Methods to get and set detection range
        private static int GetDetectionRange(Mobile m)
        {
            if (_detectionRanges.TryGetValue(m, out int range))
            {
                return range;
            }

            // Default range if not set
            return 0;
        }

        private static void SetDetectionRange(Mobile m, int range)
        {
            _detectionRanges[m] = range;
        }
    }
}
