using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Misc;

namespace Server.ACC.CSS.Systems.NinjitsuMagic
{
    public class SilentBlade : NinjitsuSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Silent Blade", "Death...",
            21004, // Icon
            9300   // Sound
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.5; } }
        public override double RequiredSkill { get { return 70.0; } }
        public override int RequiredMana { get { return 20; } }

        public SilentBlade(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private SilentBlade m_Owner;

            public InternalTarget(SilentBlade owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                {
                    Mobile target = (Mobile)o;

                    if (!from.CanSee(target))
                    {
                        from.SendLocalizedMessage(500237); // Target cannot be seen.
                    }
                    else if (from.InLOS(target) && m_Owner.CheckSequence())
                    {
                        SpellHelper.Turn(from, target);

                        // Calculate damage
                        double damage = from.Skills[SkillName.Ninjitsu].Value / 1.5;
                        bool isUnaware = !target.CanSee(from); // Target is unaware if it cannot see the caster

                        if (isUnaware)
                        {
                            damage *= 1.5; // Increase damage by 50% if target is unaware
                            target.SendMessage("You are struck by a powerful silent attack!");
                            from.SendMessage("Your Silent Blade strikes with increased force!");
                        }
                        else
                        {
                            target.SendMessage("You sense an attack but it's too late to avoid it!");
                        }

                        // Apply damage
                        AOS.Damage(target, from, (int)damage, 100, 0, 0, 0, 0);

                        // Visual and sound effects
                        Effects.SendTargetEffect(target, 0x379F, 10, 30, 1153, 0);
                        Effects.PlaySound(target.Location, target.Map, 0x1FB);

                        m_Owner.FinishSequence();
                    }
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(0.5);
        }
    }
}
