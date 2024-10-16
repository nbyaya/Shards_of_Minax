using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using Server;

namespace Server.ACC.CSS.Systems.BlacksmithMagic
{
    public class ShieldBash : BlacksmithSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Shield Bash", "Shld Bsu",
                                                        //SpellCircle.Third,
                                                        21016,
                                                        9312
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.5; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 20; } }

        public ShieldBash(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private ShieldBash m_Owner;

            public InternalTarget(ShieldBash owner) : base(1, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile target && from != target)
                {
                    if (!from.CanBeHarmful(target))
                    {
                        from.SendLocalizedMessage(1060508); // You cannot harm that target.
                        return;
                    }

                    if (m_Owner.CheckSequence())
                    {
                        from.DoHarmful(target);

                        // Play sound and visual effect
                        Effects.PlaySound(target.Location, target.Map, 0x213); // Shield bash sound
                        Effects.SendLocationEffect(target.Location, target.Map, 0x3728, 10, 0, 0x481, 0); // Flashy visual effect

                        // Apply damage
                        int damage = Utility.RandomMinMax(5, 10); // Minor damage
                        AOS.Damage(target, from, damage, 100, 0, 0, 0, 0); // Physical damage

                        // Apply stun effect
                        target.Freeze(TimeSpan.FromSeconds(2.0)); // 2 seconds stun

                        from.SendMessage("You bash your enemy with your shield, stunning them!");
                        target.SendMessage("You have been stunned by a shield bash!");
                    }

                    m_Owner.FinishSequence();
                }
                else
                {
                    from.SendLocalizedMessage(500237); // Target can not be seen.
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
