using Server;
using Server.Items;

namespace Server.Items
{
    public class ChargedAcoustesium : Item
    {
        [Constructable]
        public ChargedAcoustesium() : base(0x1841) // Set the graphic ID
        {
            Name = "Charged Acoustesium"; // Name of the item
            Hue = 520;          // Set the hue
            Stackable = false;    // Make the item stackable
            Weight = 0.1;        // Optional: set the weight of the item
        }

        public ChargedAcoustesium(Serial serial) : base(serial)
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
