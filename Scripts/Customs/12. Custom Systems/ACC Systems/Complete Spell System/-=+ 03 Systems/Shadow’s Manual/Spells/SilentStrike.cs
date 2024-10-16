using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.HidingMagic
{
    public class SilentStrike : HidingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Silent Strike", "In Manus Tacitus",
            // SpellCircle.Third,
            21013,
            9212
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 25; } }

        public SilentStrike(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(Mobile target)
        {
            if (!Caster.CanBeHarmful(target) || target == Caster)
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
                return;
            }

            if (CheckSequence())
            {
                // Apply invisibility effect to the caster for a short duration
                Caster.Hidden = true;
                Timer.DelayCall(TimeSpan.FromSeconds(5.0), () => Caster.RevealingAction());

                // Play stealthy sound and visual effect
                Caster.PlaySound(0x1FA); // Ghostly sound
                Caster.FixedParticles(0x376A, 1, 17, 9913, 1108, 3, EffectLayer.Waist);

                // Deal extra damage to the target
                int damage = (int)(Caster.Skills[SkillName.Swords].Value * 0.5);
                Caster.DoHarmful(target);
                AOS.Damage(target, Caster, damage, 100, 0, 0, 0, 0);

                // Make the caster silent, preventing detection
                target.SendMessage("You are unable to detect the attacker's position!");
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private SilentStrike m_Owner;

            public InternalTarget(SilentStrike owner) : base(1, false, TargetFlags.Harmful)
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
