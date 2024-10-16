using System;
using System.Collections;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Items;

namespace Server.ACC.CSS.Systems.VeterinaryMagic
{
    public class AggressiveCharge : VeterinarySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Aggressive Charge", "Velox Hostis",
            //SpellCircle.Third,
            21005,
            9301
        );

        public override SpellCircle Circle { get { return SpellCircle.Third; } }
        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }

        public AggressiveCharge(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private AggressiveCharge m_Owner;

            public InternalTarget(AggressiveCharge owner) : base(10, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile target && target != m_Owner.Caster)
                {
                    if (SpellHelper.CheckTown(target, from) && m_Owner.CheckSequence())
                    {
                        m_Owner.Caster.Direction = m_Owner.Caster.GetDirectionTo(target);
                        m_Owner.Caster.Animate(31, 7, 1, true, false, 0); // Animation for charge
                        Effects.PlaySound(m_Owner.Caster.Location, m_Owner.Caster.Map, 0x1F5); // Sound effect for charge

                        Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
                        {
                            if (m_Owner.Caster.InRange(target, 2))
                            {
                                target.SendMessage("You have been stunned by a powerful charge!");
                                target.Freeze(TimeSpan.FromSeconds(2.0)); // Stun effect
                                AOS.Damage(target, m_Owner.Caster, Utility.RandomMinMax(10, 20), 100, 0, 0, 0, 0); // Impact damage
                                Effects.SendMovingEffect(m_Owner.Caster, target, 0x36D4, 7, 0, false, false, 0, 0); // Visual effect for impact
                                Effects.PlaySound(target.Location, target.Map, 0x140); // Sound effect for impact
                            }
                            else
                            {
                                from.SendMessage("The target was too far away to charge.");
                            }
                        });
                    }
                }
                else
                {
                    from.SendMessage("You must target a valid enemy to charge.");
                }

                m_Owner.FinishSequence();
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
