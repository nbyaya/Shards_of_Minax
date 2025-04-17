using Server;
using Server.Items;

namespace Server.Items
{
    public class Cryovitrin : Item
    {
        [Constructable]
        public Cryovitrin() : base(0x0F07) // Set the graphic ID
        {
            Name = "Cryovitrin"; // Name of the item
            Hue = 2963;          // Set the hue
            Stackable = true;    // Make the item stackable
            Weight = 0.1;        // Optional: set the weight of the item
        }

        public Cryovitrin(Serial serial) : base(serial)
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
