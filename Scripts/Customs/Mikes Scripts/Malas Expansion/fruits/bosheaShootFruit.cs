using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class bosheaShootFruit : BaseFruit
    {
        public override string FruitName => "bosheaShoot fruit";
        public override int FruitHue => 2602;
        public override int FruitGraphic => 0x0F7B; // Example fruit graphic
        public override Type SeedType => typeof(bosheaShootFruitSeed);

        [Constructable]
        public bosheaShootFruit() : base()
        {
        }

        [Constructable]
        public bosheaShootFruit(int amount) : base(amount)
        {
        }

        public bosheaShootFruit(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // versioning, change when structure changes
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class bosheaShootFruitplant : BaseFruitPlant
    {
        public override string PlantName => "a bosheaShoot fruit plant";
        public override int PlantHue => 2602;
        public override int SeedGraphic => 0x0C45; // Seeds graphic
        public override int HarvestableGraphic => 0x0D0E; // Harvestable plant graphic
        public override Type FruitType => typeof(bosheaShootFruit);

        [Constructable]
        public bosheaShootFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
        {
        }

        public bosheaShootFruitplant(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // versioning
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class bosheaShootFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a bosheaShoot fruit seed";
        public override int SeedHue => 2602;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(bosheaShootFruitplant);

        [Constructable]
        public bosheaShootFruitSeed() : base()
        {
        }

        public bosheaShootFruitSeed(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // versioning
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
