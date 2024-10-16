using System;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Spells;
using Server.Targeting;

namespace Server.ACC.CSS.Systems.SwordsMagic
{
    public class Riposte : SwordsSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Riposte", "Reposte!",
            21004, // Animation ID
            9403   // Animation Sound
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override int RequiredMana { get { return 15; } }

        public Riposte(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.Target = new InternalTarget(this);
            }
            else
            {
                FinishSequence();
            }
        }

        public void Target(Mobile target)
        {
            if (!Caster.CanSee(target))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
            }
            else if (Caster == target)
            {
                Caster.SendLocalizedMessage(500948); // You can't target yourself.
            }
            else if (!Caster.InRange(target, 1))
            {
                Caster.SendLocalizedMessage(500946); // That is too far away.
            }
            else if (CheckSequence())
            {
                SpellHelper.Turn(Caster, target);
                Caster.FixedParticles(0x3779, 10, 15, 5032, EffectLayer.Waist); // Visual Effect
                Caster.PlaySound(0x1F7); // Sound Effect

                double damage = Caster.Skills[SkillName.Swords].Value / 2.0; // Damage based on skill level
                double stunDuration = 1.5; // Stun duration in seconds

                AOS.Damage(target, Caster, (int)damage, 100, 0, 0, 0, 0); // Physical damage

                // Apply stun effect
                target.Freeze(TimeSpan.FromSeconds(stunDuration));
                target.SendMessage("You are stunned by the riposte!");

                FinishSequence();
            }
        }

        private class InternalTarget : Target
        {
            private Riposte m_Owner;

            public InternalTarget(Riposte owner) : base(1, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile)
                {
                    m_Owner.Target((Mobile)targeted);
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
