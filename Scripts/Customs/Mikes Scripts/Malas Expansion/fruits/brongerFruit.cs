using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class brongerFruit : BaseFruit
    {
        public override string FruitName => "bronger fruit";
        public override int FruitHue => 1100;
        public override int FruitGraphic => 0x1725; // Example fruit graphic
        public override Type SeedType => typeof(brongerFruitSeed);

        [Constructable]
        public brongerFruit() : base()
        {
        }

        [Constructable]
        public brongerFruit(int amount) : base(amount)
        {
        }

        public brongerFruit(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Versioning if you ever need to change the class structure
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt(); // Read version data for future compatibility
        }
    }

    public class brongerFruitplant : BaseFruitPlant
    {
        public override string PlantName => "a bronger fruit plant";
        public override int PlantHue => 1100;
        public override int SeedGraphic => 0x0C45; // Seeds graphic
        public override int HarvestableGraphic => 0x0DEE; // Harvestable plant graphic
        public override Type FruitType => typeof(brongerFruit);

        [Constructable]
        public brongerFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
        {
        }

        public brongerFruitplant(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Versioning for future class changes
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt(); // Read version data
        }
    }

    public class brongerFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a bronger fruit seed";
        public override int SeedHue => 1100;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(brongerFruitplant);

        [Constructable]
        public brongerFruitSeed() : base()
        {
        }

        public brongerFruitSeed(Serial serial) : base(serial)
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
            int version = reader.ReadInt(); // Read version data
        }
    }
}
