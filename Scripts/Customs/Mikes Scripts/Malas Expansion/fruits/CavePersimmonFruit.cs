using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class CavePersimmonFruit : BaseFruit
    {
        public override string FruitName => "Cave Persimmon";
        public override int FruitHue => 1687;
        public override int FruitGraphic => 0x0F8D; // Example fruit graphic
        public override Type SeedType => typeof(CavePersimmonFruitSeed);

        [Constructable]
        public CavePersimmonFruit() : base()
        {
        }

        [Constructable]
        public CavePersimmonFruit(int amount) : base(amount)
        {
        }

        public CavePersimmonFruit(Serial serial) : base(serial)
        {
        }

        // Serialization Logic for CavePersimmonFruit
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Versioning for future changes, start with version 0.
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt(); // Read the version number, used for future migrations.
        }
    }
    
    public class CavePersimmonFruitplant : BaseFruitPlant
    {
        public override string PlantName => "a Cave Persimmon plant";
        public override int PlantHue => 1687;
        public override int SeedGraphic => 0x0C45; // Seeds graphic
        public override int HarvestableGraphic => 0x0D13; // Harvestable plant graphic
        public override Type FruitType => typeof(CavePersimmonFruit);

        [Constructable]
        public CavePersimmonFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
        {
        }

        public CavePersimmonFruitplant(Serial serial) : base(serial)
        {
        }

        // Serialization Logic for CavePersimmonFruitplant
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Versioning for future changes, start with version 0.
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt(); // Read the version number, used for future migrations.
        }
    }

    public class CavePersimmonFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a Cave Persimmon seed";
        public override int SeedHue => 1687;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(CavePersimmonFruitplant);

        [Constructable]
        public CavePersimmonFruitSeed() : base()
        {
        }

        public CavePersimmonFruitSeed(Serial serial) : base(serial)
        {
        }

        // Serialization Logic for CavePersimmonFruitSeed
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Versioning for future changes, start with version 0.
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt(); // Read the version number, used for future migrations.
        }
    }  
}
