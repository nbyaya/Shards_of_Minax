using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class AutumnPomegranateFruit : BaseFruit
    {
        public override string FruitName => "Autumn Pomegranate";
        public override int FruitHue => 1974;
        public override int FruitGraphic => 0x1729; // Example fruit graphic
        public override Type SeedType => typeof(AutumnPomegranateFruitSeed);

        [Constructable]
        public AutumnPomegranateFruit() : base()
        {
        }

        [Constructable]
        public AutumnPomegranateFruit(int amount) : base(amount)
        {
        }

        public AutumnPomegranateFruit(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // versioning for future changes
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt(); // Read version
        }
    }
    
    public class AutumnPomegranateFruitplant : BaseFruitPlant
    {
        public override string PlantName => "a Autumn Pomegranate plant";
        public override int PlantHue => 1974;
        public override int SeedGraphic => 0x0C45; // Seeds graphic
        public override int HarvestableGraphic => 0x0CA9; // Harvestable plant graphic
        public override Type FruitType => typeof(AutumnPomegranateFruit);

        [Constructable]
        public AutumnPomegranateFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
        {
        }

        public AutumnPomegranateFruitplant(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // versioning for future changes
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt(); // Read version
        }
    }

    public class AutumnPomegranateFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a Autumn Pomegranate seed";
        public override int SeedHue => 1974;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(AutumnPomegranateFruitplant);

        [Constructable]
        public AutumnPomegranateFruitSeed() : base()
        {
        }

        public AutumnPomegranateFruitSeed(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // versioning for future changes
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt(); // Read version
        }
    }
}
