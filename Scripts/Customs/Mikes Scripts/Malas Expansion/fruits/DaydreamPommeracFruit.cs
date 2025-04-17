using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class DaydreamPommeracFruit : BaseFruit
    {
        public override string FruitName => "Daydream Pommerac";
        public override int FruitHue => 1330;
        public override int FruitGraphic => 0x0F91; // Example fruit graphic
        public override Type SeedType => typeof(DaydreamPommeracFruitSeed);

        [Constructable]
        public DaydreamPommeracFruit() : base()
        {
        }

        [Constructable]
        public DaydreamPommeracFruit(int amount) : base(amount)
        {
        }

        public DaydreamPommeracFruit(Serial serial) : base(serial)
        {
        }

        // Add proper serialization
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Versioning: 0 for this version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt(); // Read version, currently not used, but for future proofing
        }
    }

    public class DaydreamPommeracFruitplant : BaseFruitPlant
    {
        public override string PlantName => "a Daydream Pommerac plant";
        public override int PlantHue => 1330;
        public override int SeedGraphic => 0x0C45; // Seeds graphic
        public override int HarvestableGraphic => 0x0D27; // Harvestable plant graphic
        public override Type FruitType => typeof(DaydreamPommeracFruit);

        [Constructable]
        public DaydreamPommeracFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
        {
        }

        public DaydreamPommeracFruitplant(Serial serial) : base(serial)
        {
        }

        // Add proper serialization
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Versioning: 0 for this version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt(); // Read version, currently not used, but for future proofing
        }
    }

    public class DaydreamPommeracFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a Daydream Pommerac seed";
        public override int SeedHue => 1330;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(DaydreamPommeracFruitplant);

        [Constructable]
        public DaydreamPommeracFruitSeed() : base()
        {
        }

        public DaydreamPommeracFruitSeed(Serial serial) : base(serial)
        {
        }

        // Add proper serialization
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Versioning: 0 for this version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt(); // Read version, currently not used, but for future proofing
        }
    }
}
