using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.ParryMagic
{
    public class GuardedAssault : ParrySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Guarded Assault", null,
            21005, // Effect ID for casting
            9301,  // Sound ID
            false
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override int RequiredMana { get { return 25; } }

        public GuardedAssault(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private GuardedAssault m_Owner;

            public InternalTarget(GuardedAssault owner) : base(10, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile target && from.CanBeHarmful(target))
                {
                    from.RevealingAction();
                    from.PlaySound(0x517); // Play attack sound

                    if (from.CheckSkill(SkillName.Parry, 0.0, 100.0))
                    {
                        double parrySkill = from.Skills[SkillName.Parry].Value;
                        double bonusDamage = 10.0 + (parrySkill * 0.2); // Calculate bonus damage based on parry skill

                        from.DoHarmful(target);
                        double finalDamage = Utility.RandomMinMax(15, 25) + bonusDamage; // Base damage with bonus

                        AOS.Damage(target, from, (int)finalDamage, 100, 0, 0, 0, 0); // Apply damage

                        Effects.SendTargetEffect(target, 0x37B9, 10, 30); // Visual effect on target
                        target.FixedParticles(0x374A, 1, 30, 9502, 68, 3, EffectLayer.Waist); // Secondary visual effect
                    }
                    else
                    {
                        from.SendMessage("You fail to properly execute the Guarded Assault.");
                    }
                }
                else
                {
                    from.SendLocalizedMessage(500237); // Target can not be seen.
                }

                m_Owner.FinishSequence();
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public override bool CheckCast()
        {
            if (Caster.Mana >= RequiredMana)
                return base.CheckCast();

            Caster.SendMessage("You do not have enough mana to perform Guarded Assault.");
            return false;
        }

        public override void OnCasterHurt()
        {
            // Optional: Logic to interrupt casting if desired
            base.OnCasterHurt();
        }
    }
}
