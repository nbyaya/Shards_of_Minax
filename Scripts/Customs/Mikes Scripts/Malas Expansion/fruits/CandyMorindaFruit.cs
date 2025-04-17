using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class CandyMorindaFruit : BaseFruit
    {
        public override string FruitName => "Candy Morinda";
        public override int FruitHue => 1999;
        public override int FruitGraphic => 0x09D2; // Example fruit graphic
        public override Type SeedType => typeof(CandyMorindaFruitSeed);

        [Constructable]
        public CandyMorindaFruit() : base()
        {
        }

        [Constructable]
        public CandyMorindaFruit(int amount) : base(amount)
        {
        }

        public CandyMorindaFruit(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Versioning
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt(); // Versioning
        }
    }
    
    public class CandyMorindaFruitplant : BaseFruitPlant
    {
        public override string PlantName => "a Candy Morinda plant";
        public override int PlantHue => 1999;
        public override int SeedGraphic => 0x0C45; // Seeds graphic
        public override int HarvestableGraphic => 0x0DED; // Harvestable plant graphic
        public override Type FruitType => typeof(CandyMorindaFruit);

        [Constructable]
        public CandyMorindaFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
        {
        }

        public CandyMorindaFruitplant(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Versioning
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt(); // Versioning
        }
    }

    public class CandyMorindaFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a Candy Morinda seed";
        public override int SeedHue => 1999;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(CandyMorindaFruitplant);

        [Constructable]
        public CandyMorindaFruitSeed() : base()
        {
        }

        public CandyMorindaFruitSeed(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Versioning
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt(); // Versioning
        }
    }	
}
