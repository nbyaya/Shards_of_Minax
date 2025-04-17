using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class brafulalFruit : BaseFruit
    {
        public override string FruitName => "brafulal fruit";
        public override int FruitHue => 1766;
        public override int FruitGraphic => 0x0C6A; // Example fruit graphic
        public override Type SeedType => typeof(brafulalFruitSeed);

        [Constructable]
        public brafulalFruit() : base()
        {
        }

        [Constructable]
        public brafulalFruit(int amount) : base(amount)
        {
        }

        public brafulalFruit(Serial serial) : base(serial)
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
            int version = reader.ReadInt(); // Read version (this allows for future updates)
        }
    }
    
    public class brafulalFruitplant : BaseFruitPlant
    {
        public override string PlantName => "a brafulal fruit plant";
        public override int PlantHue => 1766;
        public override int SeedGraphic => 0x0C45; // Seeds graphic
        public override int HarvestableGraphic => 0x0CBB; // Harvestable plant graphic
        public override Type FruitType => typeof(brafulalFruit);

        [Constructable]
        public brafulalFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
        {
        }

        public brafulalFruitplant(Serial serial) : base(serial)
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
            int version = reader.ReadInt(); // Read version
        }
    }


    public class brafulalFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a brafulal fruit seed";
        public override int SeedHue => 1766;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(brafulalFruitplant);

        [Constructable]
        public brafulalFruitSeed() : base()
        {
        }

        public brafulalFruitSeed(Serial serial) : base(serial)
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
            int version = reader.ReadInt(); // Read version
        }
    }    
}
