using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class DrakeMangoFruit : BaseFruit
    {
        public override string FruitName => "Drake Mango";
        public override int FruitHue => 512;
        public override int FruitGraphic => 0x0C73; // Example fruit graphic
        public override Type SeedType => typeof(DrakeMangoFruitSeed);

        [Constructable]
        public DrakeMangoFruit() : base()
        {
        }

        [Constructable]
        public DrakeMangoFruit(int amount) : base(amount)
        {
        }

        public DrakeMangoFruit(Serial serial) : base(serial)
        {
        }

        // Override the Serialize method to handle custom serialization
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer); // Serialize the base class first
            writer.Write((int)0); // Version number
            // Additional fields can be serialized here if needed
        }

        // Override the Deserialize method to handle custom deserialization
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader); // Deserialize the base class first
            int version = reader.ReadInt(); // Read the version number
            // Additional deserialization logic can be added here if needed
        }
    }

    public class DrakeMangoFruitplant : BaseFruitPlant
    {
        public override string PlantName => "a Drake Mango plant";
        public override int PlantHue => 512;
        public override int SeedGraphic => 0x0C45; // Seeds graphic
        public override int HarvestableGraphic => 0x0D2C; // Harvestable plant graphic
        public override Type FruitType => typeof(DrakeMangoFruit);

        [Constructable]
        public DrakeMangoFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
        {
        }

        public DrakeMangoFruitplant(Serial serial) : base(serial)
        {
        }

        // Override the Serialize method to handle custom serialization
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer); // Serialize the base class first
            writer.Write((int)0); // Version number
            // Additional fields can be serialized here if needed
        }

        // Override the Deserialize method to handle custom deserialization
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader); // Deserialize the base class first
            int version = reader.ReadInt(); // Read the version number
            // Additional deserialization logic can be added here if needed
        }
    }

    public class DrakeMangoFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a Drake Mango seed";
        public override int SeedHue => 512;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(DrakeMangoFruitplant);

        [Constructable]
        public DrakeMangoFruitSeed() : base()
        {
        }

        public DrakeMangoFruitSeed(Serial serial) : base(serial)
        {
        }

        // Override the Serialize method to handle custom serialization
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer); // Serialize the base class first
            writer.Write((int)0); // Version number
            // Additional fields can be serialized here if needed
        }

        // Override the Deserialize method to handle custom deserialization
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader); // Deserialize the base class first
            int version = reader.ReadInt(); // Read the version number
            // Additional deserialization logic can be added here if needed
        }
    }
}
