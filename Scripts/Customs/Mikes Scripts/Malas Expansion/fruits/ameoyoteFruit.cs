using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class ameoyoteFruit : BaseFruit
    {
        public override string FruitName => "ameoyote fruit";
        public override int FruitHue => 774;
        public override int FruitGraphic => 0x1727; // Example fruit graphic
        public override Type SeedType => typeof(ameoyoteFruitSeed);

        [Constructable]
        public ameoyoteFruit() : base()
        {
        }

        [Constructable]
        public ameoyoteFruit(int amount) : base(amount)
        {
        }

        public ameoyoteFruit(Serial serial) : base(serial)
        {
        }

        // Serialize method to save the state of the object
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // versioning
        }

        // Deserialize method to load the state of the object
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt(); // Read version
        }
    }
    
    public class ameoyoteFruitplant : BaseFruitPlant
    {
        public override string PlantName => "a ameoyote fruit plant";
        public override int PlantHue => 774;
        public override int SeedGraphic => 0x0C45; // Seeds graphic
        public override int HarvestableGraphic => 0x0CA6; // Harvestable plant graphic
        public override Type FruitType => typeof(ameoyoteFruit);

        [Constructable]
        public ameoyoteFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
        {
        }

        public ameoyoteFruitplant(Serial serial) : base(serial)
        {
        }

        // Serialize method to save the state of the object
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // versioning
        }

        // Deserialize method to load the state of the object
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt(); // Read version
        }
    }

    public class ameoyoteFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a ameoyote fruit seed";
        public override int SeedHue => 774;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(ameoyoteFruitplant);

        [Constructable]
        public ameoyoteFruitSeed() : base()
        {
        }

        public ameoyoteFruitSeed(Serial serial) : base(serial)
        {
        }

        // Serialize method to save the state of the object
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // versioning
        }

        // Deserialize method to load the state of the object
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt(); // Read version
        }
    }
}
