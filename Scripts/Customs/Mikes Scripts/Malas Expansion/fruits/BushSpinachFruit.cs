using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class BushSpinachFruit : BaseFruit
    {
        public override string FruitName => "Bush Spinach";
        public override int FruitHue => 1416;
        public override int FruitGraphic => 0x0C6D; // Example fruit graphic
        public override Type SeedType => typeof(BushSpinachFruitSeed);

        [Constructable]
        public BushSpinachFruit() : base()
        {
        }

        [Constructable]
        public BushSpinachFruit(int amount) : base(amount)
        {
        }

        public BushSpinachFruit(Serial serial) : base(serial)
        {
        }

        // Serialization method
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Version number for serialization
        }

        // Deserialization method
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt(); // Read the version number
        }
    }
    
    public class BushSpinachFruitplant : BaseFruitPlant
    {
        public override string PlantName => "a Bush Spinach plant";
        public override int PlantHue => 1416;
        public override int SeedGraphic => 0x0C45; // Seeds graphic
        public override int HarvestableGraphic => 0x0CCB; // Harvestable plant graphic
        public override Type FruitType => typeof(BushSpinachFruit);

        [Constructable]
        public BushSpinachFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
        {
        }

        public BushSpinachFruitplant(Serial serial) : base(serial)
        {
        }

        // Serialization method
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Version number for serialization
        }

        // Deserialization method
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt(); // Read the version number
        }
    }

    public class BushSpinachFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a Bush Spinach seed";
        public override int SeedHue => 1416;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(BushSpinachFruitplant);

        [Constructable]
        public BushSpinachFruitSeed() : base()
        {
        }

        public BushSpinachFruitSeed(Serial serial) : base(serial)
        {
        }

        // Serialization method
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Version number for serialization
        }

        // Deserialization method
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt(); // Read the version number
        }
    }
}
