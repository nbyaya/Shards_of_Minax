using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.MacingMagic
{
    public class CrushingImpact : MacingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Crushing Impact", "CRUSH!",
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Eighth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 25; } }

        public CrushingImpact(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private CrushingImpact m_Owner;

            public InternalTarget(CrushingImpact owner) : base(1, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile target && from.CanBeHarmful(target))
                {
                    from.DoHarmful(target);
                    m_Owner.Effect(target);
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public void Effect(Mobile target)
        {
            if (CheckSequence())
            {
                Caster.RevealingAction();

                // Play a sound effect and show visual effects at the target location
                Effects.PlaySound(Caster.Location, Caster.Map, 0x212);
                Effects.SendMovingParticles(Caster, target, 0x36B0, 7, 0, false, true, 0, 0, 0, 0, 0, 0);

                // Calculate additional damage
                double damage = Caster.Skills[SkillName.Macing].Value / 10.0 + Utility.RandomMinMax(10, 15);

                // Deal damage
                SpellHelper.Damage(this, target, damage, 0, 100, 0, 0, 0);

                // Chance to knock down the target
                if (Utility.RandomDouble() < 0.3) // 30% chance
                {
                    target.Animate(21, 6, 1, true, false, 0); // Knockdown animation
                    target.SendMessage("You have been knocked down by the Crushing Impact!");
                    target.Frozen = true; // Temporarily freeze the target

                    Timer.DelayCall(TimeSpan.FromSeconds(2.0), () =>
                    {
                        target.Frozen = false; // Unfreeze after 2 seconds
                        target.SendMessage("You manage to stand up again.");
                    });
                }
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.5);
        }
    }
}
