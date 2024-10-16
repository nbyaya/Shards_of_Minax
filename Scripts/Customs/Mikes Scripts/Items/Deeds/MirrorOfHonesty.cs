using System;
using Server;
using Server.Network;
using Server.Items;
using Server.Multis;

namespace Server.Items
{
    public class MirrorOfHonesty : Item
    {
        [Constructable]
        public MirrorOfHonesty() : base(0x2252) // You can change the item ID (0x2252 is a handheld mirror)
        {
            Name = "Mirror of Honesty";
            Hue = 1150; // Gives it a unique color, adjust as you see fit
            Weight = 1.0;
        }

        public MirrorOfHonesty(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                return;
            }

            // Assuming a radius of 5 tiles, adjust as needed
            IPooledEnumerable eable = from.GetItemsInRange(5);
            foreach (Item item in eable)
            {
                if (item is BaseDoor && ((BaseDoor)item).Locked)
                {
                    // Reveals secret doors
                    ((BaseDoor)item).Locked = false;
                    from.SendMessage("You've revealed a secret door!");
                }
                else if (item is TrapableContainer && ((TrapableContainer)item).TrapType != TrapType.None)
                {
                    // Reveals traps on containers
                    ((TrapableContainer)item).TrapType = TrapType.None;
                    from.SendMessage("You've neutralized a trap!");
                }
            }
            eable.Free();
        }
    }
}
