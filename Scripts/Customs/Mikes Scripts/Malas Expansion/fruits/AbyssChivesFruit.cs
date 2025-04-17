using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class AbyssChivesFruit : BaseFruit
    {
        public override string FruitName => "Abyss Chives";
        public override int FruitHue => 939;
        public override int FruitGraphic => 0x0F88; // Example fruit graphic
        public override Type SeedType => typeof(AbyssChivesFruitSeed);

        [Constructable]
        public AbyssChivesFruit() : base()
        {
        }

        [Constructable]
        public AbyssChivesFruit(int amount) : base(amount)
        {
        }

        public AbyssChivesFruit(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // versioning, can be incremented if needed for future changes
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt(); // versioning to manage changes over time
        }
    }

    public class AbyssChivesFruitplant : BaseFruitPlant
    {
        public override string PlantName => "a Abyss Chives plant";
        public override int PlantHue => 939;
        public override int SeedGraphic => 0x0C45; // Seeds graphic
        public override int HarvestableGraphic => 0x0CA6; // Harvestable plant graphic
        public override Type FruitType => typeof(AbyssChivesFruit);

        [Constructable]
        public AbyssChivesFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
        {
        }

        public AbyssChivesFruitplant(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // versioning, can be incremented if needed for future changes
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt(); // versioning to manage changes over time
        }
    }

    public class AbyssChivesFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a Abyss Chives seed";
        public override int SeedHue => 939;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(AbyssChivesFruitplant);

        [Constructable]
        public AbyssChivesFruitSeed() : base()
        {
        }

        public AbyssChivesFruitSeed(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // versioning, can be incremented if needed for future changes
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt(); // versioning to manage changes over time
        }
    }
}
