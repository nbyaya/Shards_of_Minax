using Server;
using Server.Items;

namespace Server.Items
{
    public class TransparentAurarus : Item
    {
        [Constructable]
        public TransparentAurarus() : base(0x1841) // Set the graphic ID
        {
            Name = "TransparentAurarus"; // Name of the item
            Hue = 442;          // Set the hue
            Stackable = false;    // Make the item stackable
            Weight = 0.1;        // Optional: set the weight of the item
        }

        public TransparentAurarus(Serial serial) : base(serial)
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
    }
}
