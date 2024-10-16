using System;
using Server.Network;
using Server.Prompts;
using Server.Items;
using Server.Targeting;
using Server.Mobiles;

namespace Server.Items
{
    public class CapacityIncreaseDeed : Item
    {
        [Constructable]
        public CapacityIncreaseDeed()
            : base(0x14F0) // Using the same graphic as a deed for simplicity.
        {
            Name = "a backpack capacity increase deed (+50)";
            Hue = 0x481;
            Weight = 1.0;
        }

        public CapacityIncreaseDeed(Serial serial) : base(serial)
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
            if (from == null || from.Backpack == null)
                return;

            if (!IsChildOf(from.Backpack))
            {
                from.SendMessage("The deed must be in your backpack to use.");
                return;
            }

            PlayerMobile player = from as PlayerMobile;

            if (player != null)
            {
                // This increases the max items a player can have in their backpack by 50.
                // You might want to adjust this according to your server's mechanics.
                player.Backpack.MaxItems += 50;
                from.SendMessage("Your backpack's capacity has been increased by 50 items.");
                this.Delete(); // Consume the deed after use.
            }
            else
            {
                from.SendMessage("An error occurred. Unable to increase backpack capacity.");
            }
        }
    }
}
