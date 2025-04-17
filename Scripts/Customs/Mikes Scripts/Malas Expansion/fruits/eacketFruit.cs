using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class eacketFruit : BaseFruit
    {
        public override string FruitName => "eacket fruit";
        public override int FruitHue => 2502;
        public override int FruitGraphic => 0x0F8C; // Example fruit graphic
        public override Type SeedType => typeof(eacketFruitSeed);

        [Constructable]
        public eacketFruit() : base()
        {
        }

        [Constructable]
        public eacketFruit(int amount) : base(amount)
        {
        }

        // Deserialize constructor
        public eacketFruit(Serial serial) : base(serial)
        {
        }

        // Properly serialize the object (if necessary for this type)
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // versioning, can be incremented when breaking backward compatibility
        }

        // Properly deserialize the object
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt(); // Read version
        }
    }

    public class eacketFruitplant : BaseFruitPlant
    {
        public override string PlantName => "a eacket fruit plant";
        public override int PlantHue => 2502;
        public override int SeedGraphic => 0x0C45; // Seeds graphic
        public override int HarvestableGraphic => 0x0C94; // Harvestable plant graphic
        public override Type FruitType => typeof(eacketFruit);

        [Constructable]
        public eacketFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
        {
        }

        // Deserialize constructor
        public eacketFruitplant(Serial serial) : base(serial)
        {
        }

        // Properly serialize the object (if necessary for this type)
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // versioning, can be incremented when breaking backward compatibility
        }

        // Properly deserialize the object
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt(); // Read version
        }
    }

    public class eacketFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a eacket fruit seed";
        public override int SeedHue => 2502;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(eacketFruitplant);

        [Constructable]
        public eacketFruitSeed() : base()
        {
        }

        // Deserialize constructor
        public eacketFruitSeed(Serial serial) : base(serial)
        {
        }

        // Properly serialize the object (if necessary for this type)
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // versioning, can be incremented when breaking backward compatibility
        }

        // Properly deserialize the object
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt(); // Read version
        }
    }
}
