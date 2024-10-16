using System;
using Server.Items;

namespace Server.Items
{
    public class ZeonRune : Item
    {
        [Constructable]
        public ZeonRune() : this(1)
        {
        }

        [Constructable]
        public ZeonRune(int amount) : base(0x1423)  //0xE34 is the itemID for a scroll. You can change this to another itemID if desired.
        {
            Stackable = true;
            Amount = amount;
            Name = "Zeon Rune";
            Hue = 2459;  // Color of the scroll, change as required.
        }

        public ZeonRune(Serial serial) : base(serial)
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
