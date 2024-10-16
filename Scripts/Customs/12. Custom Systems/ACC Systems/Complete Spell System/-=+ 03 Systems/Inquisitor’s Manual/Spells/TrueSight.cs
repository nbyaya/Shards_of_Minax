using System;
using System.Collections;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.DetectHiddenMagic
{
    public class TrueSight : DetectHiddenSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "True Sight", "Veritas Videre",
            21004,
            9300,
            false
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 70.0; } }
        public override int RequiredMana { get { return 40; } }

        public TrueSight(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Mobile caster = Caster;

                // Effects for caster
                caster.FixedParticles(0x375A, 10, 30, 5013, 1153, 2, EffectLayer.Head);
                caster.PlaySound(0x1E2); // Sound effect for casting

                // Apply True Sight effect to the caster and allies in range
                int range = 5; // Range in tiles
                TimeSpan duration = TimeSpan.FromSeconds(30); // Duration of True Sight

                List<Mobile> targets = new List<Mobile>();

                // Find all mobiles in range
                foreach (Mobile m in caster.GetMobilesInRange(range))
                {
                    if (m != caster && m.Player && m.Alive && m.AccessLevel == AccessLevel.Player)
                    {
                        targets.Add(m);
                    }
                }

                // Apply True Sight effect to caster and nearby allies
                targets.Add(caster); // Include the caster

                foreach (Mobile target in targets)
                {
                    target.SendMessage("You gain the ability to see through invisibility and stealth!");
                    target.Hidden = false; // Reveal any hidden players instantly

                    // Grant the effect of True Sight
                    target.BeginAction(typeof(TrueSight));
                    Timer.DelayCall(duration, () => RemoveTrueSight(target));

                    // Visual and sound effects for targets
                    target.FixedParticles(0x376A, 9, 32, 5005, 1153, 0, EffectLayer.Head);
                    target.PlaySound(0x1F3); // Different sound effect for allies
                }
            }

            FinishSequence();
        }

        private void RemoveTrueSight(Mobile target)
        {
            if (target == null || target.Deleted)
                return;

            target.EndAction(typeof(TrueSight));
            target.SendMessage("The effect of True Sight has worn off.");
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0); // Casting delay
        }
    }
}
