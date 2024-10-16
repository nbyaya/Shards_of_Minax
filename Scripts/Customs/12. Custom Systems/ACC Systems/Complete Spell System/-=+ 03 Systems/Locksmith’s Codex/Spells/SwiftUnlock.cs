using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Items;
using Server.Spells;

namespace Server.ACC.CSS.Systems.LockpickingMagic
{
    public class SwiftUnlock : LockpickingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Swift Unlock", "Unlockus Instanta",
                                                        21001,
                                                        9301
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 0.0; } }
        public override int RequiredMana { get { return 10; } }

        public SwiftUnlock(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private SwiftUnlock m_Owner;

            public InternalTarget(SwiftUnlock owner) : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is LockableContainer || o is BaseDoor)
                {
                    ILockable lockable = (ILockable)o;

                    if (lockable.Locked)
                    {
                        double unlockChance = 0.9; // High success rate of 90%
                        if (Utility.RandomDouble() <= unlockChance)
                        {
                            lockable.Locked = false;
                            from.SendMessage("You successfully unlock the target!");

                            // Cast to Item to access Location and Map
                            Item item = o as Item;

                            if (item != null)
                            {
                                // Play flashy effects
                                Effects.SendLocationParticles(EffectItem.Create(item.Location, item.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5024);
                                Effects.PlaySound(item.Location, item.Map, 0x5C3);
                            }
                        }
                        else
                        {
                            from.SendMessage("You failed to unlock the target.");
                        }
                    }
                    else
                    {
                        from.SendMessage("The target is already unlocked.");
                    }
                }
                else
                {
                    from.SendLocalizedMessage(500713); // That cannot be unlocked.
                }

                m_Owner.FinishSequence();
            }
        }
    }
}
