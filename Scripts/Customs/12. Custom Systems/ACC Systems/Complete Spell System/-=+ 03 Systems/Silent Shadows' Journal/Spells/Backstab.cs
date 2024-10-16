using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Items;
using Server.Spells;

namespace Server.ACC.CSS.Systems.StealthMagic
{
    public class Backstab : StealthSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Backstab", "Clandestinus Lethalis",
                                                        // SpellCircle.Third,
                                                        21009,
                                                        9208,
                                                        false
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.5; } }
        public override double RequiredSkill { get { return 60.0; } } // Skill level required to use the ability
        public override int RequiredMana { get { return 30; } } // Mana cost for the ability

        public Backstab(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(Mobile target)
        {
            if (!Caster.CanSee(target))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
            }
            else if (CheckSequence())
            {
                // Calculate the direction and determine if behind the target
                bool isBehind = Caster.InRange(target, 1) && IsBehind(target);

                if (isBehind)
                {
                    // Play visual and sound effects
                    Effects.SendLocationParticles(EffectItem.Create(target.Location, target.Map, EffectItem.DefaultDuration), 0x3728, 10, 30, 5052);
                    Effects.PlaySound(target.Location, target.Map, 0x238);

                    // Deal extra damage
                    int baseDamage = (int)(Caster.Skills[SkillName.Stealth].Value / 2); // Base damage depends on Stealth skill
                    int bonusDamage = (int)(baseDamage * 0.5); // +50% bonus damage
                    int totalDamage = baseDamage + bonusDamage;

                    target.Damage(totalDamage, Caster); // Inflict damage

                    // Show text above target
                    target.PublicOverheadMessage(MessageType.Regular, 0x22, false, "*Backstab!*");
                }
                else
                {
                    Caster.SendMessage("You need to be behind the target to backstab!");
                }
            }

            FinishSequence();
        }

        private bool IsBehind(Mobile target)
        {
            // Check if the caster is behind the target
            int diffX = Caster.Location.X - target.Location.X;
            int diffY = Caster.Location.Y - target.Location.Y;

            return (Math.Abs(diffX) <= 1 && Math.Abs(diffY) <= 1);
        }

        private class InternalTarget : Target
        {
            private Backstab m_Owner;

            public InternalTarget(Backstab owner) : base(1, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile)
                {
                    m_Owner.Target((Mobile)targeted);
                }
                else
                {
                    from.SendMessage("You can only target a living being with this ability.");
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
