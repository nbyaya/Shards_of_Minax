using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.WrestlingMagic
{
    public class ShieldBash : WrestlingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Shield Bash", "Bash",
                                                        // SpellCircle.Third,
                                                        21004,
                                                        9300
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 15; } }

        public ShieldBash(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private void Target(Mobile target)
        {
            if (Caster == null || target == null || Caster == target || !Caster.CanSee(target) || !Caster.InRange(target, 1))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen or is out of range.
                return;
            }

            if (CheckSequence())
            {
                SpellHelper.Turn(Caster, target);

                // Play visual and sound effects
                target.FixedParticles(0x3779, 1, 15, 9912, 1153, 0, EffectLayer.Waist); // Shield bash visual effect
                target.PlaySound(0x1F5); // Bash sound effect

                // Deal heavy damage
                int damage = (int)(Caster.Skills[SkillName.Macing].Value / 5); // Adjust the damage calculation as needed
                AOS.Damage(target, Caster, damage, 100, 0, 0, 0, 0); // Physical damage

                // Paralyze the target
                TimeSpan duration = TimeSpan.FromSeconds(5.0);
                target.Paralyze(duration);

                // Inform the target
                target.SendMessage("You have been struck by a powerful shield bash and are paralyzed!");

                // Inform the caster
                Caster.SendMessage("You bash your target with your shield, dealing damage and paralyzing them!");
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private ShieldBash m_Owner;

            public InternalTarget(ShieldBash owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile)
                    m_Owner.Target((Mobile)targeted);
                else
                    from.SendMessage("You can only target mobiles.");
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
