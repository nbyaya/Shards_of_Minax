using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Misc;
using Server.Items;

namespace Server.ACC.CSS.Systems.HealingMagic
{
    public class CleansingSurge : HealingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Cleansing Surge", "Ex Puro",
            21004,
            9300,
            false,
            Reagent.Garlic,
            Reagent.Ginseng,
            Reagent.SpidersSilk
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 20; } }

        public CleansingSurge(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private CleansingSurge m_Owner;

            public InternalTarget(CleansingSurge owner) : base(12, false, TargetFlags.Beneficial)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile target && target.Alive)
                {
                    m_Owner.Target(target);
                }
                else
                {
                    from.SendLocalizedMessage(500237); // Target cannot be seen or is invalid.
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public void Target(Mobile target)
        {
            if (!Caster.CanSee(target))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
            }
            else if (CheckBSequence(target))
            {
                // Play sound and visual effects
                target.PlaySound(0x1F2);
                target.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist);

                // Heal the target
                int healAmount = Utility.RandomMinMax(20, 30) + (int)(Caster.Skills[SkillName.Healing].Value * 0.1);
                target.Hits += healAmount;

                // Remove poison or disease
                target.Poison = null; // Removes poison
                // Optional: Implement disease removal logic if ServUO supports diseases.

                Caster.SendMessage($"You cleanse {target.Name}, healing them for {healAmount} hit points!");

                // Optional: Apply a buff or temporary shield effect
                // Buff or shield effect can be added here if desired.
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.5);
        }
    }
}
