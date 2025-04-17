using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class AngelRootFruit : BaseFruit
    {
        public override string FruitName => "Angel Root";
        public override int FruitHue => 1495;
        public override int FruitGraphic => 0x1723; // Example fruit graphic
        public override Type SeedType => typeof(AngelRootFruitSeed);

        [Constructable]
        public AngelRootFruit() : base()
        {
        }

        [Constructable]
        public AngelRootFruit(int amount) : base(amount)
        {
        }

        public AngelRootFruit(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Version 0
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt(); // Version 0
        }
    }

    public class AngelRootFruitplant : BaseFruitPlant
    {
        public override string PlantName => "a Angel Root plant";
        public override int PlantHue => 1495;
        public override int SeedGraphic => 0x0C45; // Seeds graphic
        public override int HarvestableGraphic => 0x0C8B; // Harvestable plant graphic
        public override Type FruitType => typeof(AngelRootFruit);

        [Constructable]
        public AngelRootFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
        {
        }

        public AngelRootFruitplant(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Version 0
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt(); // Version 0
        }
    }

    public class AngelRootFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a Angel Root seed";
        public override int SeedHue => 1495;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(AngelRootFruitplant);

        [Constructable]
        public AngelRootFruitSeed() : base()
        {
        }

        public AngelRootFruitSeed(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Version 0
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt(); // Version 0
        }
    }
}
