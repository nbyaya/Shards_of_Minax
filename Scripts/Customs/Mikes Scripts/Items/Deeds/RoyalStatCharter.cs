using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class RoyalStatCharter : Item
    {
        [Constructable]
        public RoyalStatCharter() : base( 5360 ) // Use the appropriate item ID for your charter
        {
            Name = "Royal Stat Charter";
            Hue = 1153; // Adjust color as needed
            Weight = 1.0;
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Raises Stat Cap by 1, up to a maximum of 200");
        }

        public RoyalStatCharter(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            PlayerMobile player = from as PlayerMobile;

            if (player == null)
            {
                from.SendMessage("Only players can use this.");
                return;
            }

            if (!IsChildOf(from.Backpack))
            {
                from.SendMessage("This must be in your backpack to use.");
                return;
            }

            if (player.StatCap < 200)
            {
                player.StatCap += 1; // Increase the player's stat cap by one
                from.SendMessage("Your stat cap has been increased.");
                this.Consume(); // Consume the charter
            }
            else
            {
                from.SendMessage("Your stat cap is already at the maximum of 200.");
            }
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
