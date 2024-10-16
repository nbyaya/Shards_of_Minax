using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.LumberjackingMagic
{
    public class AxeThrow : LumberjackingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Axe Throw", "Hurl Axem",
            21016, // Icon
            9316,  // Cast sound
            false  // Requires targeting
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 20.0; } }
        public override int RequiredMana { get { return 20; } }

        public AxeThrow(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private void Target(Mobile m)
        {
            if (!Caster.CanSee(m))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
            }
            else if (CheckSequence())
            {
                Caster.Mana -= RequiredMana;

                SpellHelper.Turn(Caster, m);

                // Calculate damage and effects
                int damage = Utility.RandomMinMax(20, 30);
                bool willStun = Utility.RandomDouble() < 0.2; // 20% chance to stun
                bool willDisorient = !willStun && Utility.RandomDouble() < 0.2; // 20% chance to disorient if not stunned

                // Apply damage to target
                AOS.Damage(m, Caster, damage, 0, 100, 0, 0, 0);

                // Play effects
                Effects.SendMovingEffect(Caster, m, 0xF47, 10, 0, false, false, 0, 0); // Axe throwing animation
                Caster.PlaySound(0x51D); // Sound of throwing an axe

                // Apply additional effects
                if (willStun)
                {
                    m.Freeze(TimeSpan.FromSeconds(2.0)); // Stun for 2 seconds
                    m.SendMessage("You have been stunned by a powerful axe throw!");
                }
                else if (willDisorient)
                {
                    m.Paralyzed = true;
                    Timer.DelayCall(TimeSpan.FromSeconds(1.5), () =>
                    {
                        if (m != null && !m.Deleted)
                        {
                            m.Paralyzed = false;
                            m.SendMessage("You feel disoriented after the axe hit.");
                        }
                    });
                }

                // Add flashy effect at the point of impact
                Effects.SendLocationEffect(m.Location, m.Map, 0x3728, 10, 1, 1153, 0); // Impact effect
                m.PlaySound(0x213); // Sound of impact

                FinishSequence();
            }
        }

        private class InternalTarget : Target
        {
            private AxeThrow m_Owner;

            public InternalTarget(AxeThrow owner) : base(10, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                {
                    m_Owner.Target((Mobile)o);
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
