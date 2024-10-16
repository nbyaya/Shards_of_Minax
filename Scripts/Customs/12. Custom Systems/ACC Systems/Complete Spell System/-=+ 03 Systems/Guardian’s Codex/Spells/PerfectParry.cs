using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.SkillHandlers;

namespace Server.ACC.CSS.Systems.ParryMagic
{
    public class PerfectParry : ParrySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Perfect Parry", "In Paratus Deflectus",
            21005,
            9301
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 40.0; } }
        public override int RequiredMana { get { return 15; } }

        public PerfectParry(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private PerfectParry m_Owner;

            public InternalTarget(PerfectParry owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object target)
            {
                if (target is Mobile)
                {
                    Mobile defender = (Mobile)target;

                    if (from.CanBeHarmful(defender) && m_Owner.CheckSequence())
                    {
                        from.DoHarmful(defender);

                        // Deal medium damage
                        int damage = Utility.RandomMinMax(15, 25);
                        SpellHelper.Damage(TimeSpan.Zero, defender, from, damage);

                        // Visual and sound effects for the attack
                        defender.FixedParticles(0x3779, 10, 30, 5052, EffectLayer.Waist);
                        defender.PlaySound(0x1F1);

                        // Increase Parry skill temporarily
                        // Ensure you have the correct BuffIcon here
                        BuffInfo.AddBuff(from, new BuffInfo(BuffIcon.Paralyze, 1075658, TimeSpan.FromSeconds(10), from)); // Changed to use an existing BuffIcon
                        from.SendLocalizedMessage(1075659); // Your parrying skill has increased!

                        from.SendMessage("You feel your parry skill increase!");
                        from.FixedParticles(0x373A, 10, 15, 5038, EffectLayer.Waist);
                        from.PlaySound(0x1F2);

                        // Temporary Parry skill boost
                        Timer.DelayCall(TimeSpan.FromSeconds(10), () => RemoveParryBonus(from));

                        m_Owner.FinishSequence();
                    }
                }
                else
                {
                    from.SendMessage("You must target a valid mobile.");
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        private static void RemoveParryBonus(Mobile from)
        {
            if (from == null || from.Deleted)
                return;

            // Corrected to use the appropriate BuffIcon or BuffInfo reference
            BuffInfo.RemoveBuff(from, BuffIcon.Paralyze); // Ensure this matches the BuffIcon used in AddBuff
            from.SendLocalizedMessage(1075660); // Your temporary parrying skill increase has worn off.
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.0);
        }
    }
}
