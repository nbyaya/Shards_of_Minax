using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class DessertBroccoliFruit : BaseFruit
    {
        public override string FruitName => "Dessert Broccoli";
        public override int FruitHue => 1463;
        public override int FruitGraphic => 0x172A; // Example fruit graphic
        public override Type SeedType => typeof(DessertBroccoliFruitSeed);

        [Constructable]
        public DessertBroccoliFruit() : base()
        {
        }

        [Constructable]
        public DessertBroccoliFruit(int amount) : base(amount)
        {
        }

        // Deserialize constructor for the fruit class
        public DessertBroccoliFruit(Serial serial) : base(serial)
        {
        }

        // Override the method to deserialize data specific to this class
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer); // Ensure base class serialization
            writer.Write(0); // Version number (you can increment if you make changes to the serialization structure)
        }

        // Override the method to deserialize data specific to this class
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader); // Ensure base class deserialization
            int version = reader.ReadInt(); // Read the version number
        }
    }
    
    public class DessertBroccoliFruitplant : BaseFruitPlant
    {
        public override string PlantName => "a Dessert Broccoli plant";
        public override int PlantHue => 1463;
        public override int SeedGraphic => 0x0C45; // Seeds graphic
        public override int HarvestableGraphic => 0x0C8A; // Harvestable plant graphic
        public override Type FruitType => typeof(DessertBroccoliFruit);

        [Constructable]
        public DessertBroccoliFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
        {
        }

        // Deserialize constructor for the plant class
        public DessertBroccoliFruitplant(Serial serial) : base(serial)
        {
        }

        // Override the method to serialize data for the plant class
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer); // Ensure base class serialization
            writer.Write(0); // Version number for the plant class
        }

        // Override the method to deserialize data for the plant class
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader); // Ensure base class deserialization
            int version = reader.ReadInt(); // Read the version number for the plant class
        }
    }

    public class DessertBroccoliFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a Dessert Broccoli seed";
        public override int SeedHue => 1463;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(DessertBroccoliFruitplant);

        [Constructable]
        public DessertBroccoliFruitSeed() : base()
        {
        }

        // Deserialize constructor for the seed class
        public DessertBroccoliFruitSeed(Serial serial) : base(serial)
        {
        }

        // Override the method to serialize data for the seed class
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer); // Ensure base class serialization
            writer.Write(0); // Version number for the seed class
        }

        // Override the method to deserialize data for the seed class
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader); // Ensure base class deserialization
            int version = reader.ReadInt(); // Read the version number for the seed class
        }
    }
}
