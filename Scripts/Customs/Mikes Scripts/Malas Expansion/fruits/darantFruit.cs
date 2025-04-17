using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class darantFruit : BaseFruit
    {
        public override string FruitName => "darant fruit";
        public override int FruitHue => 2376;
        public override int FruitGraphic => 0x0C6D; // Example fruit graphic
        public override Type SeedType => typeof(darantFruitSeed);

        [Constructable]
        public darantFruit() : base()
        {
        }

        [Constructable]
        public darantFruit(int amount) : base(amount)
        {
        }

        public darantFruit(Serial serial) : base(serial)
        {
            // Ensure that the base class handles the serialization
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer); // Serialize base class properties first
            writer.Write((int)0); // Versioning for future serialization
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader); // Deserialize base class properties first
            int version = reader.ReadInt(); // Read the version number
        }
    }

    public class darantFruitplant : BaseFruitPlant
    {
        public override string PlantName => "a darant fruit plant";
        public override int PlantHue => 2376;
        public override int SeedGraphic => 0x0C45; // Seeds graphic
        public override int HarvestableGraphic => 0x0DC4; // Harvestable plant graphic
        public override Type FruitType => typeof(darantFruit);

        [Constructable]
        public darantFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
        {
        }

        public darantFruitplant(Serial serial) : base(serial)
        {
            // Ensure that the base class handles the serialization
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer); // Serialize base class properties first
            writer.Write((int)0); // Versioning for future serialization
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader); // Deserialize base class properties first
            int version = reader.ReadInt(); // Read the version number
        }
    }

    public class darantFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a darant fruit seed";
        public override int SeedHue => 2376;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(darantFruitplant);

        [Constructable]
        public darantFruitSeed() : base()
        {
        }

        public darantFruitSeed(Serial serial) : base(serial)
        {
            // Ensure that the base class handles the serialization
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer); // Serialize base class properties first
            writer.Write((int)0); // Versioning for future serialization
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader); // Deserialize base class properties first
            int version = reader.ReadInt(); // Read the version number
        }
    }
}
