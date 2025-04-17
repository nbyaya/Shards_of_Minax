using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class DewKiwiFruit : BaseFruit
    {
        public override string FruitName => "Dew Kiwi";
        public override int FruitHue => 1201;
        public override int FruitGraphic => 0x0C78; // Example fruit graphic
        public override Type SeedType => typeof(DewKiwiFruitSeed);

        [Constructable]
        public DewKiwiFruit() : base()
        {
        }

        [Constructable]
        public DewKiwiFruit(int amount) : base(amount)
        {
        }

        public DewKiwiFruit(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // versioning - to allow for future updates
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt(); // Read the version
        }
    }

    public class DewKiwiFruitplant : BaseFruitPlant
    {
        public override string PlantName => "a DewKiwi plant";
        public override int PlantHue => 1201;
        public override int SeedGraphic => 0x0C45; // Seeds graphic
        public override int HarvestableGraphic => 0x0D04; // Harvestable plant graphic
        public override Type FruitType => typeof(DewKiwiFruit);

        [Constructable]
        public DewKiwiFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
        {
        }

        public DewKiwiFruitplant(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // versioning - to allow for future updates
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt(); // Read the version
        }
    }

    public class DewKiwiFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a DewKiwi seed";
        public override int SeedHue => 1201;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(DewKiwiFruitplant);

        [Constructable]
        public DewKiwiFruitSeed() : base()
        {
        }

        public DewKiwiFruitSeed(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // versioning - to allow for future updates
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt(); // Read the version
        }
    }
}
