using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Items;

namespace Server.ACC.CSS.Systems.HealingMagic
{
    public class DivineRestoration : HealingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Divine Restoration", "Divinus Restitutio",
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }

        public DivineRestoration(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private DivineRestoration m_Owner;

            public InternalTarget(DivineRestoration owner) : base(12, false, TargetFlags.Beneficial)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile target)
                {
                    if (from.CanBeBeneficial(target, true))
                    {
                        from.DoBeneficial(target);
                        m_Owner.ApplyEffect(target);
                    }
                }
                else
                {
                    from.SendLocalizedMessage(500237); // Target cannot be seen.
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public void ApplyEffect(Mobile target)
        {
            if (!CheckSequence())
                return;

            // Apply the healing and remove debuffs
            target.Heal(Utility.RandomMinMax(20, 40)); // Heals for a random amount between 20 and 40
            RemoveDebuffs(target);

            // Visual and sound effects
            target.FixedParticles(0x376A, 10, 15, 5030, EffectLayer.Head);
            target.PlaySound(0x1F2);

            FinishSequence();
        }

        private void RemoveDebuffs(Mobile target)
        {
            // Example method to clear common negative states
            // Adjust this according to what "debuffs" mean in your context

            if (target.Poison != null)
            {
                target.CurePoison(target); // Cure any poison
            }

            if (target.Paralyzed)
            {
                target.Paralyzed = false; // Remove paralysis
            }

            if (target.Blessed)
            {
                target.Blessed = false; // Remove any blessed state, assuming it's a negative in this context
            }

            // Add additional debuff checks and removals as needed
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0);
        }
    }
}
