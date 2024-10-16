using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.ArcheryMagic
{
    public class ExplosiveArrow : ArcherySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Explosive Arrow", "Explode!",
            21005,
            9301,
            false
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; } // Adjust the spell circle as needed
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 30; } }

        public ExplosiveArrow(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private ExplosiveArrow m_Owner;

            public InternalTarget(ExplosiveArrow owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile target)
                {
                    m_Owner.Target(target);
                }
                else
                {
                    from.SendMessage("You can only target a mobile entity.");
                    m_Owner.FinishSequence();
                }
            }
        }

        public void Target(Mobile target)
        {
            if (!Caster.CanSee(target))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
                return;
            }

            if (CheckSequence())
            {
                // Calculate the damage based on caster's skills
                double damage = Utility.RandomMinMax(20, 40) + Caster.Skills[SkillName.Archery].Value * 0.1;

                // Play explosion effects and sound
                Effects.PlaySound(target.Location, Caster.Map, 0x207); // Explosion sound
                Effects.SendLocationEffect(target.Location, Caster.Map, 0x36BD, 20, 10, 0, 0); // Explosion effect

                // Deal damage to the main target
                AOS.Damage(target, Caster, (int)damage, 100, 0, 0, 0, 0);

                // Deal area-of-effect damage to nearby targets
                foreach (Mobile m in target.GetMobilesInRange(2))
                {
                    if (m != target && Caster.CanBeHarmful(m) && Caster.InLOS(m))
                    {
                        Caster.DoHarmful(m);
                        AOS.Damage(m, Caster, (int)(damage / 2), 100, 0, 0, 0, 0);
                    }
                }
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(5.0); // Adjust as needed for balance
        }
    }
}
