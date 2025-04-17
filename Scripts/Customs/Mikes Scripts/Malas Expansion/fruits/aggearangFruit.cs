using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class aggearangFruit : BaseFruit
    {
        public override string FruitName => "aggearang fruit";
        public override int FruitHue => 1466;
        public override int FruitGraphic => 0x0C5C; // Example fruit graphic
        public override Type SeedType => typeof(aggearangFruitSeed);

        [Constructable]
        public aggearangFruit() : base()
        {
        }

        [Constructable]
        public aggearangFruit(int amount) : base(amount)
        {
        }

        public aggearangFruit(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
            // Add any custom serialization logic here, if needed
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            // Add any custom deserialization logic here, if needed
        }
    }
    
    public class aggearangFruitplant : BaseFruitPlant
    {
        public override string PlantName => "a aggearang fruit plant";
        public override int PlantHue => 1466;
        public override int SeedGraphic => 0x0C45; // Seeds graphic
        public override int HarvestableGraphic => 0x0C87; // Harvestable plant graphic
        public override Type FruitType => typeof(aggearangFruit);

        [Constructable]
        public aggearangFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
        {
        }

        public aggearangFruitplant(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
            // Add any custom serialization logic here, if needed
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            // Add any custom deserialization logic here, if needed
        }
    }

    public class aggearangFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a aggearang fruit seed";
        public override int SeedHue => 1466;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(aggearangFruitplant);

        [Constructable]
        public aggearangFruitSeed() : base()
        {
        }

        public aggearangFruitSeed(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
            // Add any custom serialization logic here, if needed
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            // Add any custom deserialization logic here, if needed
        }
    }
}
