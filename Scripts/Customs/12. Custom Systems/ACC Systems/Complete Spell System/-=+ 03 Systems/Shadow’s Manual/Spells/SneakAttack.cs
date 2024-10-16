using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Items;
using System.Collections;

namespace Server.ACC.CSS.Systems.HidingMagic
{
    public class SneakAttack : HidingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Sneak Attack", "Vis Umbras",
            21008,
            9207
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 25.0; } }
        public override int RequiredMana { get { return 25; } }

        public SneakAttack(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            // Remove Caster.Invisibility and rely on Caster.Hidden
            if (!Caster.Hidden)
            {
                Caster.SendLocalizedMessage(1060177); // You must be hidden to use this skill.
                return;
            }

            Caster.Target = new InternalTarget(this);
        }

        public void Target(Mobile target)
        {
            if (target == null || !Caster.CanSee(target))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
            }
            else if (Caster.Hidden && CheckSequence()) // Removed Caster.Invisibility check
            {
                Caster.RevealingAction();

                // Determine bonus damage
                double damageMultiplier = 1.5; // Base extra damage multiplier
                if (Caster.Hidden)
                    damageMultiplier = 2.0; // Increase multiplier if the caster is hidden

                int damage = (int)(Caster.Skills[SkillName.Stealth].Value * damageMultiplier);
                AOS.Damage(target, Caster, damage, 100, 0, 0, 0, 0);

                // Flashy visual and sound effects
                target.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.Head);
                target.PlaySound(0x3E3);

                Caster.SendMessage("You perform a sneak attack, dealing extra damage!");

                FinishSequence();
            }
        }

        private class InternalTarget : Target
        {
            private SneakAttack m_Owner;

            public InternalTarget(SneakAttack owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                    m_Owner.Target((Mobile)o);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
