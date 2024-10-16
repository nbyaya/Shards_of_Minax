using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Items;

namespace Server.ACC.CSS.Systems.HealingMagic
{
    public class PurifyingLight : HealingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Purifying Light", "Sanctus Lux",
            21004, // Icon ID
            9300, // Sound ID
            false // Reagents
        );

        public override SpellCircle Circle { get { return SpellCircle.Fourth; } }

        public override double CastDelay { get { return 2.5; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 20; } }

        public PurifyingLight(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(Mobile target)
        {
            if (Caster.CanSee(target) && CheckBenevolentTarget(target) && CheckSequence())
            {
                Caster.DoBeneficial(target);
                SpellHelper.Turn(Caster, target);

                double healingAmount = Caster.Skills[SkillName.Healing].Value / 2.0;
                target.Heal((int)healingAmount);

                // Remove negative effects
                target.Paralyzed = false;
                target.CurePoison(Caster);

                // Update buff removal with correct entries
                BuffInfo.RemoveBuff(target, BuffIcon.Clumsy);
                BuffInfo.RemoveBuff(target, BuffIcon.Weaken);
                // BuffInfo.RemoveBuff(target, BuffIcon.Feeblemind); // Removed or replaced

                // Visual effects
                target.FixedParticles(0x376A, 10, 15, 5037, EffectLayer.Waist); // Blue sparkle effect
                target.PlaySound(0x1F2); // Heal sound

                // Area of effect dispel negative effects for allies
                foreach (Mobile m in target.GetMobilesInRange(3))
                {
                    if (m != target && m != Caster && Caster.CanBeBeneficial(m, false, true) && m.Alive && !m.IsDeadBondedPet && m.Karma >= 0)
                    {
                        m.FixedParticles(0x376A, 10, 15, 5037, EffectLayer.Waist);
                        m.Paralyzed = false;
                        m.CurePoison(Caster);
                        BuffInfo.RemoveBuff(m, BuffIcon.Clumsy);
                        BuffInfo.RemoveBuff(m, BuffIcon.Weaken);
                        // BuffInfo.RemoveBuff(m, BuffIcon.Feeblemind); // Removed or replaced
                    }
                }
            }
            else
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
            }

            FinishSequence();
        }

        private bool CheckBenevolentTarget(Mobile target)
        {
            // Replacing 'IsEnemy' with a custom check or logic
            // Example: Assume a target is not an enemy if their Karma is non-negative
            return target.Karma >= 0 && !target.IsDeadBondedPet; // Add any other conditions as needed
        }

        private class InternalTarget : Target
        {
            private PurifyingLight m_Owner;

            public InternalTarget(PurifyingLight owner) : base(12, false, TargetFlags.Beneficial)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile)
                    m_Owner.Target((Mobile)targeted);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
