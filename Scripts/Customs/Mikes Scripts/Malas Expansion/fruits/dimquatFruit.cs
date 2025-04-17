using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class dimquatFruit : BaseFruit
    {
        public override string FruitName => "dimquat fruit";
        public override int FruitHue => 1911;
        public override int FruitGraphic => 0x0C5C; // Example fruit graphic
        public override Type SeedType => typeof(dimquatFruitSeed);

        [Constructable]
        public dimquatFruit() : base()
        {
        }

        [Constructable]
        public dimquatFruit(int amount) : base(amount)
        {
        }

        public dimquatFruit(Serial serial) : base(serial)
        {
        }

        // Serialization method
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Versioning; if you have multiple versions, increment this number
        }

        // Deserialization method
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt(); // Read version data
        }
    }

    public class dimquatFruitplant : BaseFruitPlant
    {
        public override string PlantName => "a dimquat fruit plant";
        public override int PlantHue => 1911;
        public override int SeedGraphic => 0x0C45; // Seeds graphic
        public override int HarvestableGraphic => 0x0C9A; // Harvestable plant graphic
        public override Type FruitType => typeof(dimquatFruit);

        [Constructable]
        public dimquatFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
        {
        }

        public dimquatFruitplant(Serial serial) : base(serial)
        {
        }

        // Serialization method
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Versioning; if you have multiple versions, increment this number
        }

        // Deserialization method
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt(); // Read version data
        }
    }

    public class dimquatFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a dimquat fruit seed";
        public override int SeedHue => 1911;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(dimquatFruitplant);

        [Constructable]
        public dimquatFruitSeed() : base()
        {
        }

        public dimquatFruitSeed(Serial serial) : base(serial)
        {
        }

        // Serialization method
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Versioning; if you have multiple versions, increment this number
        }

        // Deserialization method
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt(); // Read version data
        }
    }
}
