using System;
using Server;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
    public class WizardKey : Item
    {
        [Constructable]
        public WizardKey() : base(0x100E)
        {
            Name = "Wizard Key";
            LootType = LootType.Blessed;
        }

        public WizardKey(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int) 0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }

        public override void OnDoubleClick(Mobile from)
        {
            from.SendMessage("Select a container or door to lock.");
            from.Target = new LockTarget();
        }

        private class LockTarget : Target
        {
            public LockTarget() : base(10, false, TargetFlags.None)
            {
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is BaseDoor)
                {
                    BaseDoor door = (BaseDoor)targeted;
                    door.Locked = true;
                    from.SendMessage("You magically lock the door.");
                }
                else if (targeted is LockableContainer)
                {
                    LockableContainer container = (LockableContainer)targeted;
                    container.Locked = true;
                    from.SendMessage("You magically lock the container.");
                }
                else
                {
                    from.SendMessage("That is not a lockable object.");
                }
            }
        }
    }
}
