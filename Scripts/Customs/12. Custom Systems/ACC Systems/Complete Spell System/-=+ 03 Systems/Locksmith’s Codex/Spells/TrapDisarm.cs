using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Items;
using Server.Spells;

namespace Server.ACC.CSS.Systems.LockpickingMagic
{
    public class TrapDisarm : LockpickingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Trap Disarm", "Nullus Trap Disarmare",
            21001,
            9301
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 25.0; } }
        public override int RequiredMana { get { return 10; } }

        public TrapDisarm(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private TrapDisarm m_Owner;

            public InternalTarget(TrapDisarm owner) : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is BaseDoor || targeted is LockableContainer)
                {
                    if (SpellHelper.CheckTown(targeted as IEntity, from) && m_Owner.CheckSequence())
                    {
                        from.PlaySound(0x3B5); // Trap disarm sound effect
                        Effects.SendLocationEffect(((IEntity)targeted).Location, from.Map, 0x376A, 10, 1, 1152, 0); // Visual effect for disarming

                        if (targeted is LockableContainer container)
                        {
                            // Replace with actual trap disarming logic for LockableContainer
                            from.SendMessage("You have successfully disarmed the trap on the chest!");
                        }
                        else if (targeted is BaseDoor door)
                        {
                            // Replace with actual trap disarming logic for BaseDoor
                            from.SendMessage("You have successfully disarmed the trap on the door!");
                        }
                    }
                    else
                    {
                        from.SendMessage("You failed to disarm the trap.");
                    }
                }
                else
                {
                    from.SendMessage("That is not a valid target.");
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
