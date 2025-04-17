using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class AutumnCherryFruit : BaseFruit
    {
        public override string FruitName => "Autumn Cherry";
        public override int FruitHue => 2238;
        public override int FruitGraphic => 0x0C6A; // Example fruit graphic
        public override Type SeedType => typeof(AutumnCherryFruitSeed);

        [Constructable]
        public AutumnCherryFruit() : base()
        {
        }

        [Constructable]
        public AutumnCherryFruit(int amount) : base(amount)
        {
        }

        public AutumnCherryFruit(Serial serial) : base(serial)
        {
        }

        // Serialization for the AutumnCherryFruit class
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer); // Call the base class's Serialize method
            writer.Write((int)0); // Versioning - start with version 0
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader); // Call the base class's Deserialize method
            int version = reader.ReadInt(); // Read the version number (which we can use later for changes)
        }
    }

    public class AutumnCherryFruitplant : BaseFruitPlant
    {
        public override string PlantName => "a Autumn Cherry plant";
        public override int PlantHue => 2238;
        public override int SeedGraphic => 0x0C45; // Seeds graphic
        public override int HarvestableGraphic => 0x0D2A; // Harvestable plant graphic
        public override Type FruitType => typeof(AutumnCherryFruit);

        [Constructable]
        public AutumnCherryFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
        {
        }

        public AutumnCherryFruitplant(Serial serial) : base(serial)
        {
        }

        // Serialization for the AutumnCherryFruitplant class
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer); // Call the base class's Serialize method
            writer.Write((int)0); // Versioning - start with version 0
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader); // Call the base class's Deserialize method
            int version = reader.ReadInt(); // Read the version number (which we can use later for changes)
        }
    }

    public class AutumnCherryFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a Autumn Cherry seed";
        public override int SeedHue => 2238;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(AutumnCherryFruitplant);

        [Constructable]
        public AutumnCherryFruitSeed() : base()
        {
        }

        public AutumnCherryFruitSeed(Serial serial) : base(serial)
        {
        }

        // Serialization for the AutumnCherryFruitSeed class
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer); // Call the base class's Serialize method
            writer.Write((int)0); // Versioning - start with version 0
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader); // Call the base class's Deserialize method
            int version = reader.ReadInt(); // Read the version number (which we can use later for changes)
        }
    }
}
