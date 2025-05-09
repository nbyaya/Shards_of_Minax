using System;
using Server;
using Server.Engines.Craft;

namespace Server.Items
{
    public class TransmutationCauldron : BaseTool
    {
        public override CraftSystem CraftSystem { get { return DefTransmuting.CraftSystem; } }

        [Constructable]
        public TransmutationCauldron() : base(0xA5B4) // You can replace 0x9B2 with the appropriate item ID for a backpack
        {
            Name = "Transmutation Cauldron";
            Weight = 5.0; // Adjust the weight as needed
            ShowUsesRemaining = true;
        }

        [Constructable]
        public TransmutationCauldron(int uses) : base(uses, 0xA5B4)
        {
            Name = "Transmutation Cauldron";
            Weight = 5.0; // Adjust the weight as needed
        }

        public TransmutationCauldron(Serial serial) : base(serial)
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
