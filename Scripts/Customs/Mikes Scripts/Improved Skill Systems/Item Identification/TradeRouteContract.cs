using System;
using Server.Gumps;
using Server.Items;
using Server.Network;
using Server.Targeting;
using Server.Custom;

namespace Server.Items
{
    public class TradeRouteContract : Item
    {
        [Constructable]
        public TradeRouteContract() : base(0x14F0) // Example item ID, change as needed
        {
            Name = "Trade Route Contract";
            Hue = 1153; // Example hue, change as needed
        }

        public TradeRouteContract(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                return;
            }

            from.SendGump(new TradeRouteGump(from));
            Delete(); // Delete the item after use
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
    }
}

