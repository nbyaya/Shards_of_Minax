using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class BushCerimanFruit : BaseFruit
    {
        public override string FruitName => "BushCeriman fruit";
        public override int FruitHue => 2034;
        public override int FruitGraphic => 0x1721; // Example fruit graphic
        public override Type SeedType => typeof(BushCerimanFruitSeed);

        [Constructable]
        public BushCerimanFruit() : base()
        {
        }

        [Constructable]
        public BushCerimanFruit(int amount) : base(amount)
        {
        }

        public BushCerimanFruit(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer); // Ensure to call the base Serialize method
            writer.Write((int)0); // Version number for future compatibility
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader); // Ensure to call the base Deserialize method
            int version = reader.ReadInt(); // Read version number
        }
    }
    
    public class BushCerimanFruitplant : BaseFruitPlant
    {
        public override string PlantName => "a BushCeriman fruit plant";
        public override int PlantHue => 2034;
        public override int SeedGraphic => 0x0C45; // Seeds graphic
        public override int HarvestableGraphic => 0x0C90; // Harvestable plant graphic
        public override Type FruitType => typeof(BushCerimanFruit);

        [Constructable]
        public BushCerimanFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
        {
        }

        public BushCerimanFruitplant(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer); // Ensure to call the base Serialize method
            writer.Write((int)0); // Version number for future compatibility
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader); // Ensure to call the base Deserialize method
            int version = reader.ReadInt(); // Read version number
        }
    }

    public class BushCerimanFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a BushCeriman fruit seed";
        public override int SeedHue => 2034;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(BushCerimanFruitplant);

        [Constructable]
        public BushCerimanFruitSeed() : base()
        {
        }

        public BushCerimanFruitSeed(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer); // Ensure to call the base Serialize method
            writer.Write((int)0); // Version number for future compatibility
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader); // Ensure to call the base Deserialize method
            int version = reader.ReadInt(); // Read version number
        }
    }	
}
