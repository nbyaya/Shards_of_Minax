using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.SwordsMagic
{
    public class DecisiveStrike : SwordsSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Decisive Strike", "Dux Fok",
                                                        21005,
                                                        9404
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 35; } }

        public DecisiveStrike(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(Mobile target)
        {
            if (target == null || !Caster.CanSee(target) || !Caster.InRange(target, 1) || !Caster.CanBeHarmful(target))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen or is out of range.
            }
            else if (CheckSequence())
            {
                Caster.DoHarmful(target);

                // Play visual and sound effects
                Effects.SendMovingEffect(Caster, target, 0xF52, 10, 1, false, false, 1160, 0); // Visual effect of a fast strike
                Effects.PlaySound(target.Location, target.Map, 0x20E); // Sound of a critical hit

                // Calculate damage with critical chance
                double critChance = Caster.Skills[SkillName.Swords].Value / 100.0;
                bool isCritical = Utility.RandomDouble() <= critChance;

                int damage = Utility.RandomMinMax(20, 30);
                if (isCritical)
                {
                    damage *= 2;
                    target.SendLocalizedMessage(1060165); // You have been critically hit!
                    Effects.SendLocationEffect(target.Location, target.Map, 0x37B9, 20, 10, 1166, 0); // Explosion effect on target
                }

                // Apply damage to the target
                AOS.Damage(target, Caster, damage, 100, 0, 0, 0, 0);

                // Chance to apply a bleed effect
                if (Utility.RandomDouble() < 0.2) // 20% chance to apply bleed
                {
                    target.ApplyPoison(Caster, Poison.Lesser);
                    target.SendLocalizedMessage(1060092); // You are bleeding!
                }

                // Chance to stun the target
                if (Utility.RandomDouble() < 0.15) // 15% chance to stun
                {
                    target.Paralyze(TimeSpan.FromSeconds(2.0));
                    target.SendLocalizedMessage(1070835); // You are frozen and cannot move!
                }

                // End casting sequence
                FinishSequence();
            }
        }

        private class InternalTarget : Target
        {
            private DecisiveStrike m_Owner;

            public InternalTarget(DecisiveStrike owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile target)
                    m_Owner.Target(target);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
