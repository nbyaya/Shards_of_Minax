using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Items;
using Server.Spells;

namespace Server.ACC.CSS.Systems.LockpickingMagic
{
    public class LockInsight : LockpickingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Lock Insight", "Rev Luxo",
                                                        21001, // Gump icon
                                                        9301,  // Casting effect
                                                        false
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 20.0; } }
        public override int RequiredMana { get { return 10; } }

        public LockInsight(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(Container target)
        {
            if (!Caster.CanSee(target))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
            }
            else if (SpellHelper.CheckTown(target.Location, Caster) && CheckSequence())
            {
                SpellHelper.Turn(Caster, target);

                // Visual and sound effects for revealing contents
                Effects.SendLocationEffect(target.Location, target.Map, 0x3709, 30, 10, 1153, 0);
                Effects.PlaySound(target.Location, target.Map, 0x1FD);

                Caster.SendMessage("You reveal the contents of the locked container...");

                // Reveal the contents to the caster
                foreach (Item item in target.Items)
                {
                    // Check if the item can be blessed
                    if (item is BaseClothing || item is BaseWeapon)
                    {
                        // Use BlessedFor to mark the item as blessed for the caster
                        item.BlessedFor = Caster;
                    }
                    item.PublicOverheadMessage(Network.MessageType.Regular, 0x3B2, false, item.Name);
                }

                // Simulate the attempt to unlock
                if (target is LockableContainer && ((LockableContainer)target).Locked)
                {
                    Effects.SendLocationEffect(target.Location, target.Map, 0x376A, 30, 10, 1153, 0);
                    Effects.PlaySound(target.Location, target.Map, 0x241);

                    LockableContainer lockable = (LockableContainer)target;
                    if (Utility.RandomDouble() < (Caster.Skills[SkillName.Lockpicking].Value / 100.0))
                    {
                        lockable.Locked = false;
                        Caster.SendMessage("You successfully unlock the container!");
                    }
                    else
                    {
                        Caster.SendMessage("You failed to unlock the container.");
                    }
                }
                else
                {
                    Caster.SendMessage("The container is not locked.");
                }
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private LockInsight m_Owner;

            public InternalTarget(LockInsight owner) : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Container container)
                    m_Owner.Target(container);
                else
                    from.SendMessage("That is not a valid target.");
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
