using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class CavernMangosteenFruit : BaseFruit
    {
        public override string FruitName => "Cavern Mangosteen";
        public override int FruitHue => 1914;
        public override int FruitGraphic => 0x0C73; // Example fruit graphic
        public override Type SeedType => typeof(CavernMangosteenFruitSeed);

        [Constructable]
        public CavernMangosteenFruit() : base()
        {
        }

        [Constructable]
        public CavernMangosteenFruit(int amount) : base(amount)
        {
        }

        public CavernMangosteenFruit(Serial serial) : base(serial)
        {
        }

        // Add the method for serializing (Serialization logic)
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            // You can add custom serialization logic here if needed.
        }

        // Add the method for deserializing (Deserialization logic)
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            // You can add custom deserialization logic here if needed.
        }
    }
    
    public class CavernMangosteenFruitplant : BaseFruitPlant
    {
        public override string PlantName => "a Cavern Mangosteen plant";
        public override int PlantHue => 1914;
        public override int SeedGraphic => 0x0C45; // Seeds graphic
        public override int HarvestableGraphic => 0x0CAA; // Harvestable plant graphic
        public override Type FruitType => typeof(CavernMangosteenFruit);

        [Constructable]
        public CavernMangosteenFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
        {
        }

        public CavernMangosteenFruitplant(Serial serial) : base(serial)
        {
        }

        // Add the method for serializing (Serialization logic)
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            // You can add custom serialization logic here if needed.
        }

        // Add the method for deserializing (Deserialization logic)
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            // You can add custom deserialization logic here if needed.
        }
    }


    public class CavernMangosteenFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a Cavern Mangosteen seed";
        public override int SeedHue => 1914;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(CavernMangosteenFruitplant);

        [Constructable]
        public CavernMangosteenFruitSeed() : base()
        {
        }

        public CavernMangosteenFruitSeed(Serial serial) : base(serial)
        {
        }

        // Add the method for serializing (Serialization logic)
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            // You can add custom serialization logic here if needed.
        }

        // Add the method for deserializing (Deserialization logic)
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            // You can add custom deserialization logic here if needed.
        }
    }	
}
