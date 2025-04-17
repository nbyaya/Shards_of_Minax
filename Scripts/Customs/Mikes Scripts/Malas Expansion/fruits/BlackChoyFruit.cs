using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class BlackChoyFruit : BaseFruit
    {
        public override string FruitName => "Black Choy";
        public override int FruitHue => 2349;
        public override int FruitGraphic => 0x0C5D; // Example fruit graphic
        public override Type SeedType => typeof(BlackChoyFruitSeed);

        [Constructable]
        public BlackChoyFruit() : base()
        {
        }

        [Constructable]
        public BlackChoyFruit(int amount) : base(amount)
        {
        }

        public BlackChoyFruit(Serial serial) : base(serial)
        {
        }

        // Serialization/Deserialization
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            // Additional serialization logic (if needed)
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            // Additional deserialization logic (if needed)
        }
    }

    public class BlackChoyFruitplant : BaseFruitPlant
    {
        public override string PlantName => "a Black Choy plant";
        public override int PlantHue => 2349;
        public override int SeedGraphic => 0x0C45; // Seeds graphic
        public override int HarvestableGraphic => 0x0CA8; // Harvestable plant graphic
        public override Type FruitType => typeof(BlackChoyFruit);

        [Constructable]
        public BlackChoyFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
        {
        }

        public BlackChoyFruitplant(Serial serial) : base(serial)
        {
        }

        // Serialization/Deserialization
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            // Additional serialization logic (if needed)
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            // Additional deserialization logic (if needed)
        }
    }

    public class BlackChoyFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a Black Choy seed";
        public override int SeedHue => 2349;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(BlackChoyFruitplant);

        [Constructable]
        public BlackChoyFruitSeed() : base()
        {
        }

        public BlackChoyFruitSeed(Serial serial) : base(serial)
        {
        }

        // Serialization/Deserialization
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            // Additional serialization logic (if needed)
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            // Additional deserialization logic (if needed)
        }
    }
}
