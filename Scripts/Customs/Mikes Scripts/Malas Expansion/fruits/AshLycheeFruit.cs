using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class AshLycheeFruit : BaseFruit
    {
        public override string FruitName => "Ash Lychee";
        public override int FruitHue => 1996;
        public override int FruitGraphic => 0x09D2; // Example fruit graphic
        public override Type SeedType => typeof(AshLycheeFruitSeed);

        [Constructable]
        public AshLycheeFruit() : base()
        {
        }

        [Constructable]
        public AshLycheeFruit(int amount) : base(amount)
        {
        }

        public AshLycheeFruit(Serial serial) : base(serial)
        {
            // Add custom serialization logic if needed here
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Versioning (e.g., version 0 for now)
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt(); // Read version to handle future changes
        }
    }
    
    public class AshLycheeFruitplant : BaseFruitPlant
    {
        public override string PlantName => "a Ash Lychee plant";
        public override int PlantHue => 1996;
        public override int SeedGraphic => 0x0C45; // Seeds graphic
        public override int HarvestableGraphic => 0x0D25; // Harvestable plant graphic
        public override Type FruitType => typeof(AshLycheeFruit);

        [Constructable]
        public AshLycheeFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
        {
        }

        public AshLycheeFruitplant(Serial serial) : base(serial)
        {
            // Add custom deserialization logic if needed here
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Versioning (e.g., version 0 for now)
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt(); // Read version to handle future changes
        }
    }

    public class AshLycheeFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a Ash Lychee seed";
        public override int SeedHue => 1996;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(AshLycheeFruitplant);

        [Constructable]
        public AshLycheeFruitSeed() : base()
        {
        }

        public AshLycheeFruitSeed(Serial serial) : base(serial)
        {
            // Add custom deserialization logic if needed here
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Versioning (e.g., version 0 for now)
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt(); // Read version to handle future changes
        }
    }  
}
