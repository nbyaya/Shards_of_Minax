using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class criarianFruit : BaseFruit
    {
        public override string FruitName => "criarian fruit";
        public override int FruitHue => 1114;
        public override int FruitGraphic => 0x0C65; // Example fruit graphic
        public override Type SeedType => typeof(criarianFruitSeed);

        [Constructable]
        public criarianFruit() : base()
        {
        }

        [Constructable]
        public criarianFruit(int amount) : base(amount)
        {
        }

        public criarianFruit(Serial serial) : base(serial)  // Ensure proper serialization
        {
        }

        // Override the Serialize and Deserialize methods for correct serialization
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);  // Call base class serialization
            writer.Write(0); // Versioning, for potential future updates
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader); // Call base class deserialization
            int version = reader.ReadInt(); // Read version
        }
    }

    public class criarianFruitplant : BaseFruitPlant
    {
        public override string PlantName => "a criarian fruit plant";
        public override int PlantHue => 1114;
        public override int SeedGraphic => 0x0C45; // Seeds graphic
        public override int HarvestableGraphic => 0x0D28; // Harvestable plant graphic
        public override Type FruitType => typeof(criarianFruit);

        [Constructable]
        public criarianFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
        {
        }

        public criarianFruitplant(Serial serial) : base(serial) // Ensure proper serialization
        {
        }

        // Override the Serialize and Deserialize methods for correct serialization
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);  // Call base class serialization
            writer.Write(0); // Versioning, for potential future updates
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader); // Call base class deserialization
            int version = reader.ReadInt(); // Read version
        }
    }

    public class criarianFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a criarian fruit seed";
        public override int SeedHue => 1114;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(criarianFruitplant);

        [Constructable]
        public criarianFruitSeed() : base()
        {
        }

        public criarianFruitSeed(Serial serial) : base(serial) // Ensure proper serialization
        {
        }

        // Override the Serialize and Deserialize methods for correct serialization
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);  // Call base class serialization
            writer.Write(0); // Versioning, for potential future updates
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader); // Call base class deserialization
            int version = reader.ReadInt(); // Read version
        }
    }
}
