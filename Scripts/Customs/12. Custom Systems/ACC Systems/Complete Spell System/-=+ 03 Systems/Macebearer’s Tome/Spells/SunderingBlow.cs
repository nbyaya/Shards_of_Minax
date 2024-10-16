using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.MacingMagic
{
    public class SunderingBlow : MacingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Sundering Blow", "Ventus Viator",
            21004,
            9300,
            false,
            Reagent.BlackPearl,
            Reagent.Nightshade
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Eighth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 70.0; } }
        public override int RequiredMana { get { return 30; } }

        public SunderingBlow(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private SunderingBlow m_Owner;

            public InternalTarget(SunderingBlow owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile)
                {
                    Mobile target = (Mobile)targeted;

                    if (from.CanBeHarmful(target) && m_Owner.CheckSequence())
                    {
                        from.DoHarmful(target);

                        // Calculate bonus damage based on target's remaining health percentage
                        double remainingHealthPercentage = (double)target.Hits / target.HitsMax;
                        int bonusDamage = (int)(20 * (1.0 - remainingHealthPercentage)); // Bonus damage scales inversely with health

                        // Apply base and bonus damage
                        int baseDamage = Utility.RandomMinMax(10, 20);
                        int totalDamage = baseDamage + bonusDamage;

                        from.MovingEffect(target, 0x1FDD, 7, 1, false, false); // Adds a visual effect to the attack
                        from.PlaySound(0x208); // Adds a sound effect to the attack
                        target.FixedParticles(0x374A, 10, 15, 5038, EffectLayer.Waist); // Adds particles effect at the target

                        AOS.Damage(target, from, totalDamage, 100, 0, 0, 0, 0); // Physical damage
                    }
                }

                m_Owner.FinishSequence();
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0);
        }
    }
}
