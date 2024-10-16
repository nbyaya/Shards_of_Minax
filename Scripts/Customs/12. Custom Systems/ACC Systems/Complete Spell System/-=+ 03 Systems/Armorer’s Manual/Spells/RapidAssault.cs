using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.ArmsLoreMagic
{
    public class RapidAssault : ArmsLoreSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Rapid Assault", "Vas Ylem Lor",  // Name and incantation
            21004, 9300,                     // Icon and sound
            false,                           // Line of sight
            Reagent.BlackPearl,              // Reagents
            Reagent.SpidersSilk
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; } // Adjust circle level as needed
        }

        public override double CastDelay { get { return 0.1; } }  // Short cast time
        public override double RequiredSkill { get { return 50.0; } }  // Skill level requirement
        public override int RequiredMana { get { return 25; } }  // Mana cost

        public RapidAssault(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private RapidAssault m_Owner;

            public InternalTarget(RapidAssault owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile target && m_Owner.Caster.CanBeHarmful(target))
                {
                    m_Owner.PerformRapidHits(target);
                }
                else
                {
                    from.SendMessage("You cannot target that.");
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public void PerformRapidHits(Mobile target)
        {
            if (!Caster.CanSee(target))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
                return;
            }

            if (CheckSequence())
            {
                Caster.DoHarmful(target);

                // Rapid assault: Three fast hits
                for (int i = 0; i < 3; i++)
                {
                    int damage = Utility.RandomMinMax(5, 10);  // Damage range per hit
                    Caster.MovingEffect(target, 0xF62, 10, 1, false, false, 0, 0); // Visual effect for hit
                    Caster.PlaySound(0x208); // Play hit sound

                    Timer.DelayCall(TimeSpan.FromMilliseconds(i * 200), () => // Delay between hits
                    {
                        if (target.Alive && Caster.CanBeHarmful(target))
                        {
                            Caster.DoHarmful(target);
                            AOS.Damage(target, Caster, damage, 100, 0, 0, 0, 0); // Apply damage
                            target.FixedEffect(0x376A, 10, 16, 1153, 3); // Hit visual effect
                        }
                    });
                }

                // Additional effect: Chance to stun
                if (Utility.RandomDouble() < 0.2)  // 20% chance
                {
                    target.Freeze(TimeSpan.FromSeconds(2));  // Stun for 2 seconds
                    target.SendMessage("You are stunned by the rapid assault!");
                }
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.0);
        }
    }
}
