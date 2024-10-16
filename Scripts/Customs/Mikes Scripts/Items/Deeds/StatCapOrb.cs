using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class StatCapOrb : Item
    {
        [Constructable]
        public StatCapOrb() : base(0x1869) // Use the appropriate item ID for your orb
        {
            Name = "Stat Cap Orb";
            Hue = 1153; // Adjust color as needed
            Weight = 1.0;
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Raises Stat Cap by 1");
        }

        public StatCapOrb(Serial serial) : base(serial)
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

            player.StatCap += 1; // Increase the player's stat cap by one
            from.SendMessage("Your stat cap has been increased.");
            this.Consume(); // Consume the orb
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
