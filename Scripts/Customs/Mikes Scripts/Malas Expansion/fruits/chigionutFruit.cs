using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class chigionutFruit : BaseFruit
    {
        public override string FruitName => "chigionut fruit";
        public override int FruitHue => 1634;
        public override int FruitGraphic => 0x0C7A; // Example fruit graphic
        public override Type SeedType => typeof(chigionutFruitSeed);

        [Constructable]
        public chigionutFruit() : base()
        {
        }

        [Constructable]
        public chigionutFruit(int amount) : base(amount)
        {
        }

        public chigionutFruit(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Versioning, can be used for future updates
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt(); // Read version for future changes
        }
    }
    
    public class chigionutFruitplant : BaseFruitPlant
    {
        public override string PlantName => "a chigionut fruit plant";
        public override int PlantHue => 1634;
        public override int SeedGraphic => 0x0C45; // Seeds graphic
        public override int HarvestableGraphic => 0x0D28; // Harvestable plant graphic
        public override Type FruitType => typeof(chigionutFruit);

        [Constructable]
        public chigionutFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
        {
        }

        public chigionutFruitplant(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Versioning, can be used for future updates
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt(); // Read version for future changes
        }
    }

    public class chigionutFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a chigionut fruit seed";
        public override int SeedHue => 1634;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(chigionutFruitplant);

        [Constructable]
        public chigionutFruitSeed() : base()
        {
        }

        public chigionutFruitSeed(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Versioning, can be used for future updates
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt(); // Read version for future changes
        }
    }
}
