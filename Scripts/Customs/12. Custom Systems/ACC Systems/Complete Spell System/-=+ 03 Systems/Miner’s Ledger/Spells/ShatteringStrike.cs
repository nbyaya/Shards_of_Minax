using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;


namespace Server.ACC.CSS.Systems.MiningMagic
{
    public class ShatteringStrike : MiningSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Shattering Strike", "Uus Ylem",
            21013,
            9300,
            false,
            Reagent.MandrakeRoot,
            Reagent.BlackPearl
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 70.0; } }
        public override int RequiredMana { get { return 25; } }

        public ShatteringStrike(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private ShatteringStrike m_Owner;

            public InternalTarget(ShatteringStrike owner) : base(10, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile target)
                {
                    if (from.CanBeHarmful(target) && m_Owner.CheckSequence())
                    {
                        from.DoHarmful(target);
                        m_Owner.CastShatteringStrike(target);
                    }
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        private void CastShatteringStrike(Mobile target)
        {
            // Play visuals and sound effects
            Caster.PlaySound(0x1F5); // Sound of a powerful strike
            Caster.FixedParticles(0x376A, 1, 29, 9950, EffectLayer.Waist); // Sparkling effect around caster

            target.PlaySound(0x2C3); // Sound of rock shattering
            target.FixedParticles(0x36BD, 1, 25, 9937, EffectLayer.Waist); // Rock shatter effect on target

            // Apply damage
            double damage = Utility.RandomMinMax(30, 50); // Heavy damage range
            SpellHelper.Damage(this, target, damage, 100, 0, 0, 0, 0); // 100% physical damage

            // Additional effects - Armor Debuff
            TimeSpan debuffDuration = TimeSpan.FromSeconds(10.0);
            int armorDebuffAmount = -20; // Reduces target's physical resist by 20%
            ResistanceMod mod = new ResistanceMod(ResistanceType.Physical, armorDebuffAmount);

            target.AddResistanceMod(mod);
            Timer.DelayCall(debuffDuration, () =>
            {
                target.RemoveResistanceMod(mod);
            });
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.5);
        }
    }
}
