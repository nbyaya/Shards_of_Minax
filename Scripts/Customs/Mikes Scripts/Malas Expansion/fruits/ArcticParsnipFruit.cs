using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class ArcticParsnipFruit : BaseFruit
    {
        public override string FruitName => "Arctic Parsnip";
        public override int FruitHue => 1345;
        public override int FruitGraphic => 0x172B; // Example fruit graphic
        public override Type SeedType => typeof(ArcticParsnipFruitSeed);

        [Constructable]
        public ArcticParsnipFruit() : base()
        {
        }

        [Constructable]
        public ArcticParsnipFruit(int amount) : base(amount)
        {
        }

        public ArcticParsnipFruit(Serial serial) : base(serial)
        {
        }

        // This ensures the serialization works correctly for this class
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer); // Call the base class Serialize method
            writer.Write((int)0); // Version number (useful for future changes in serialization)
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader); // Call the base class Deserialize method
            int version = reader.ReadInt(); // Read the version
        }
    }
    
    public class ArcticParsnipFruitplant : BaseFruitPlant
    {
        public override string PlantName => "a Arctic Parsnip plant";
        public override int PlantHue => 1345;
        public override int SeedGraphic => 0x0C45; // Seeds graphic
        public override int HarvestableGraphic => 0x0C8D; // Harvestable plant graphic
        public override Type FruitType => typeof(ArcticParsnipFruit);

        [Constructable]
        public ArcticParsnipFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
        {
        }

        public ArcticParsnipFruitplant(Serial serial) : base(serial)
        {
        }

        // This ensures the serialization works correctly for this class
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer); // Call the base class Serialize method
            writer.Write((int)0); // Version number (useful for future changes in serialization)
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader); // Call the base class Deserialize method
            int version = reader.ReadInt(); // Read the version
        }
    }


    public class ArcticParsnipFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a Arctic Parsnip seed";
        public override int SeedHue => 1345;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(ArcticParsnipFruitplant);

        [Constructable]
        public ArcticParsnipFruitSeed() : base()
        {
        }

        public ArcticParsnipFruitSeed(Serial serial) : base(serial)
        {
        }

        // This ensures the serialization works correctly for this class
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer); // Call the base class Serialize method
            writer.Write((int)0); // Version number (useful for future changes in serialization)
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader); // Call the base class Deserialize method
            int version = reader.ReadInt(); // Read the version
        }
    }
}
