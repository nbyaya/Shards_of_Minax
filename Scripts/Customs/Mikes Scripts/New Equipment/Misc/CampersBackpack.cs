using System;
using Server;
using Server.Engines.Craft;

namespace Server.Items
{
    public class CampersBackpack : BaseTool
    {
        public override CraftSystem CraftSystem { get { return DefCamping.CraftSystem; } }

        [Constructable]
        public CampersBackpack() : base(0x9B2) // You can replace 0x9B2 with the appropriate item ID for a backpack
        {
            Name = "Campers Backpack";
            Weight = 5.0; // Adjust the weight as needed
            ShowUsesRemaining = true;
        }

        [Constructable]
        public CampersBackpack(int uses) : base(uses, 0x9B2)
        {
            Name = "Campers Backpack";
            Weight = 5.0; // Adjust the weight as needed
        }

        public CampersBackpack(Serial serial) : base(serial)
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
