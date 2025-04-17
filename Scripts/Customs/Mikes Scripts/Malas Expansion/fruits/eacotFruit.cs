using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class eacotFruit : BaseFruit
    {
        public override string FruitName => "eacot fruit";
        public override int FruitHue => 2225;
        public override int FruitGraphic => 0x1725; // Example fruit graphic
        public override Type SeedType => typeof(eacotFruitSeed);

        [Constructable]
        public eacotFruit() : base()
        {
        }

        [Constructable]
        public eacotFruit(int amount) : base(amount)
        {
        }

        public eacotFruit(Serial serial) : base(serial)
        {
        }

        // Override the OnSerialize method to define what gets serialized for this class
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            // Add any custom serialization logic here if needed (e.g. properties, fields).
        }

        // Override the OnDeserialize method to define how the object is restored
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            // Add any custom deserialization logic here if needed.
        }
    }
    
    public class eacotFruitplant : BaseFruitPlant
    {
        public override string PlantName => "a eacot fruit plant";
        public override int PlantHue => 2225;
        public override int SeedGraphic => 0x0C45; // Seeds graphic
        public override int HarvestableGraphic => 0x0CA5; // Harvestable plant graphic
        public override Type FruitType => typeof(eacotFruit);

        [Constructable]
        public eacotFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
        {
        }

        public eacotFruitplant(Serial serial) : base(serial)
        {
        }

        // Override the OnSerialize method to define what gets serialized for this class
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            // Add any custom serialization logic here if needed (e.g. properties, fields).
        }

        // Override the OnDeserialize method to define how the object is restored
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            // Add any custom deserialization logic here if needed.
        }
    }

    public class eacotFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a eacot fruit seed";
        public override int SeedHue => 2225;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(eacotFruitplant);

        [Constructable]
        public eacotFruitSeed() : base()
        {
        }

        public eacotFruitSeed(Serial serial) : base(serial)
        {
        }

        // Override the OnSerialize method to define what gets serialized for this class
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            // Add any custom serialization logic here if needed (e.g. properties, fields).
        }

        // Override the OnDeserialize method to define how the object is restored
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            // Add any custom deserialization logic here if needed.
        }
    }
}
