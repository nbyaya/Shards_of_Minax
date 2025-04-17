using Server;
using Server.Items;

namespace Server.Items
{
    public class Astrocyne : Item
    {
        [Constructable]
        public Astrocyne() : base(0x0F07) // Set the graphic ID
        {
            Name = "Astrocyne"; // Name of the item
            Hue = 1911;          // Set the hue
            Stackable = true;    // Make the item stackable
            Weight = 0.1;        // Optional: set the weight of the item
        }

        public Astrocyne(Serial serial) : base(serial)
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
