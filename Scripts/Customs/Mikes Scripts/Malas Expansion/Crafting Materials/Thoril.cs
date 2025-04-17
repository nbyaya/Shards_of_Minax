using Server;
using Server.Items;

namespace Server.Items
{
    public class Thoril : Item
    {
        [Constructable]
        public Thoril() : base(0x1841) // Set the graphic ID
        {
            Name = "Thoril"; // Name of the item
            Hue = 2335;          // Set the hue
            Stackable = false;    // Make the item stackable
            Weight = 0.1;        // Optional: set the weight of the item
        }

        public Thoril(Serial serial) : base(serial)
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
