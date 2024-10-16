using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class MurderRemovalDeed : Item
    {
        [Constructable]
        public MurderRemovalDeed() : base(0x14F0)  // Using the graphic of a deed
        {
            Name = "a murder removal deed";
        }

        public MurderRemovalDeed(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from == null || !from.Player)
                return;

            PlayerMobile player = from as PlayerMobile;

            if (player == null)
                return;

            if (!IsChildOf(from.Backpack))
            {
                from.SendMessage("The deed must be in your backpack to use.");
                return;
            }

            if (player.Kills > 0)
            {
                player.Kills -= 1; // Remove one murder count
                from.SendMessage("You feel a weight lifted off your conscience.");
                this.Delete(); // Remove the deed after use
            }
            else
            {
                from.SendMessage("Your conscience is clear, and you have no murders to repent.");
            }
        }
    }
}
