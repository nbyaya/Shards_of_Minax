using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.ACC.CSS.Systems.DetectHiddenMagic
{
    public class UnseenHunter : DetectHiddenSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Unseen Hunter", "Invisio Praeda",
            21004, // Effect ID for visual effect when casting
            9300   // Sound ID for casting
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }

        public UnseenHunter(Mobile caster, Item scroll) : base(caster, scroll, m_Info) { }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private UnseenHunter m_Owner;

            public InternalTarget(UnseenHunter owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile target)
                {
                    if (!from.CanSee(target) || target.Hidden)
                    {
                        from.SendLocalizedMessage(500237); // Target can not be seen.
                    }
                    else if (SpellHelper.CheckTown(target.Location, from) && m_Owner.CheckSequence())
                    {
                        SpellHelper.Turn(from, target);

                        // Apply the visual effect and sound when the ability is cast
                        Effects.SendLocationEffect(target.Location, target.Map, 0x376A, 10, 1, 1153, 0); // Flashy effect on target
                        target.PlaySound(0x209); // Impact sound

                        // Check if target is hidden or recently revealed
                        if (target.Hidden || target.Combatant != null)
                        {
                            double damage = Utility.RandomMinMax(15, 25); // Additional damage if conditions met
                            AOS.Damage(target, from, (int)damage, 0, 100, 0, 0, 0);

                            // Mana refund for the caster
                            from.Mana += 5;
                            from.SendMessage("You feel a surge of energy as you strike!");
                        }
                        else
                        {
                            // Standard attack damage
                            from.SendMessage("Your attack was not enhanced.");
                        }

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
            return TimeSpan.FromSeconds(2.5);
        }
    }
}
