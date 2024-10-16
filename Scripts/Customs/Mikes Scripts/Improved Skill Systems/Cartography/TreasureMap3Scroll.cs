using System;
using Server;
using Server.Items;
using Server.Network;

namespace Server.Items
{
    public class TreasureMap3Scroll : Item
    {
        [Constructable]
        public TreasureMap3Scroll() : base(0x1F4C) // Item ID for a scroll
        {
            Name = "Treasure Map Scroll";
            Hue = 1152; // Optional: Sets the color of the scroll
            LootType = LootType.Blessed; // Ensures the item is blessed
        }

        public TreasureMap3Scroll(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack)) // Ensure the item is in the player's backpack
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                return;
            }

            // Create a level 1 treasure map and place it in the player's backpack
            TreasureMap treasureMap = new TreasureMap(3, from.Map);
            if (!from.Backpack.TryDropItem(from, treasureMap, false))
            {
                treasureMap.Delete();
                from.SendMessage("You do not have enough room in your backpack for the treasure map.");
            }
            else
            {
                from.SendMessage("A level 3 treasure map has been placed in your backpack.");
                this.Delete(); // Delete the scroll after use
            }
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
    }
}
