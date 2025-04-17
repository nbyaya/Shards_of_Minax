using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class crennealeryFruit : BaseFruit
    {
        public override string FruitName => "crennealery fruit";
        public override int FruitHue => 520;
        public override int FruitGraphic => 0x172C; // Example fruit graphic
        public override Type SeedType => typeof(crennealeryFruitSeed);

        [Constructable]
        public crennealeryFruit() : base()
        {
        }

        [Constructable]
        public crennealeryFruit(int amount) : base(amount)
        {
        }

        public crennealeryFruit(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // versioning, useful if you need future updates
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt(); // versioning, useful for future updates
        }
    }
    
    public class crennealeryFruitplant : BaseFruitPlant
    {
        public override string PlantName => "a crennealery fruit plant";
        public override int PlantHue => 520;
        public override int SeedGraphic => 0x0C45; // Seeds graphic
        public override int HarvestableGraphic => 0x0CA3; // Harvestable plant graphic
        public override Type FruitType => typeof(crennealeryFruit);

        [Constructable]
        public crennealeryFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
        {
        }

        public crennealeryFruitplant(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // versioning, useful if you need future updates
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt(); // versioning, useful for future updates
        }
    }

    public class crennealeryFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a crennealery fruit seed";
        public override int SeedHue => 520;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(crennealeryFruitplant);

        [Constructable]
        public crennealeryFruitSeed() : base()
        {
        }

        public crennealeryFruitSeed(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // versioning, useful if you need future updates
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt(); // versioning, useful for future updates
        }
    }   
}
