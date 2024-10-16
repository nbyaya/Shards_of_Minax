using System;
using Server.Items;

namespace Server.Items
{
    public class MaxxiaScroll : Item
    {
        [Constructable]
        public MaxxiaScroll() : this(1)
        {
        }

        [Constructable]
        public MaxxiaScroll(int amount) : base(0xE34)  //0xE34 is the itemID for a scroll. You can change this to another itemID if desired.
        {
            Stackable = false;
            Amount = amount;
            Name = "Maxxia Scroll";
            Hue = 1150;  // Color of the scroll, change as required.
        }

        public MaxxiaScroll(Serial serial) : base(serial)
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
    }
}
