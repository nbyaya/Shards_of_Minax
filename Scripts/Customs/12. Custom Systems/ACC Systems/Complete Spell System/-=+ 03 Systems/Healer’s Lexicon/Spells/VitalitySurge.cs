using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections;

namespace Server.ACC.CSS.Systems.HealingMagic
{
    public class VitalitySurge : HealingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Vitality Surge", "Vita Reclusio",
            // SpellCircle.Sixth,
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 75.0; } }
        public override int RequiredMana { get { return 20; } }

        public VitalitySurge(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private VitalitySurge m_Owner;

            public InternalTarget(VitalitySurge owner) : base(10, false, TargetFlags.Beneficial)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile target)
                {
                    if (target.Alive && target.CanBeBeneficial(from, true))
                    {
                        from.DoBeneficial(target);
                        m_Owner.ApplyEffect(target);
                    }
                    else
                    {
                        from.SendMessage("You cannot target that.");
                    }
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public void ApplyEffect(Mobile target)
        {
            if (CheckSequence())
            {
                int boostAmount = 50; // Amount to boost health
                int durationSeconds = 30; // Duration of the effect in seconds

                Effects.SendTargetParticles(target, 0x376A, 1, 29, 1153, 3, 9502, EffectLayer.Waist, 0); // Visual effect
                target.PlaySound(0x1F2); // Sound effect

                int originalHits = target.Hits;
                target.Hits += boostAmount; // Heal the target by the boost amount

                Timer.DelayCall(TimeSpan.FromSeconds(durationSeconds), () =>
                {
                    target.Hits = originalHits; // Reset health after duration
                    target.SendMessage("The surge of vitality fades away.");
                    Effects.SendTargetParticles(target, 0x373A, 1, 15, 1153, 4, 9502, EffectLayer.Waist, 0); // End effect visual
                    target.PlaySound(0x1F8); // End effect sound
                });
            }

            FinishSequence();
        }
    }
}
