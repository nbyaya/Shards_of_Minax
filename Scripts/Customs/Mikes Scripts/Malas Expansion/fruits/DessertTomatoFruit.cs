using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class DessertTomatoFruit : BaseFruit
    {
        public override string FruitName => "Dessert Tomato";
        public override int FruitHue => 850;
        public override int FruitGraphic => 0x0F8F; // Example fruit graphic
        public override Type SeedType => typeof(DessertTomatoFruitSeed);

        [Constructable]
        public DessertTomatoFruit() : base()
        {
        }

        [Constructable]
        public DessertTomatoFruit(int amount) : base(amount)
        {
        }

        public DessertTomatoFruit(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            // You can add custom serialization code here if needed
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            // You can add custom deserialization code here if needed
        }
    }
    
    public class DessertTomatoFruitplant : BaseFruitPlant
    {
        public override string PlantName => "a Dessert Tomato plant";
        public override int PlantHue => 850;
        public override int SeedGraphic => 0x0C45; // Seeds graphic
        public override int HarvestableGraphic => 0x0C88; // Harvestable plant graphic
        public override Type FruitType => typeof(DessertTomatoFruit);

        [Constructable]
        public DessertTomatoFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
        {
        }

        public DessertTomatoFruitplant(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            // Add any custom serialization here for the plant if needed
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            // Add any custom deserialization here for the plant if needed
        }
    }


    public class DessertTomatoFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a Dessert Tomato seed";
        public override int SeedHue => 850;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(DessertTomatoFruitplant);

        [Constructable]
        public DessertTomatoFruitSeed() : base()
        {
        }

        public DessertTomatoFruitSeed(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            // You can add custom serialization code here for the seed if needed
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            // You can add custom deserialization code here for the seed if needed
        }
    }   
}
