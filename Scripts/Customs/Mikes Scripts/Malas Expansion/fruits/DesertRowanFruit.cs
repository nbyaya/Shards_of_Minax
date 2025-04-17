using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class DesertRowanFruit : BaseFruit
    {
        public override string FruitName => "Desert Rowan";
        public override int FruitHue => 2734;
        public override int FruitGraphic => 0x1725; // Example fruit graphic
        public override Type SeedType => typeof(DesertRowanFruitSeed);

        [Constructable]
        public DesertRowanFruit() : base()
        {
        }

        [Constructable]
        public DesertRowanFruit(int amount) : base(amount)
        {
        }

        public DesertRowanFruit(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Version number for future compatibility
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt(); // Read the version (currently version 0)
        }
    }
    
    public class DesertRowanFruitplant : BaseFruitPlant
    {
        public override string PlantName => "a Desert Rowan plant";
        public override int PlantHue => 2734;
        public override int SeedGraphic => 0x0C45; // Seeds graphic
        public override int HarvestableGraphic => 0x0D28; // Harvestable plant graphic
        public override Type FruitType => typeof(DesertRowanFruit);

        [Constructable]
        public DesertRowanFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
        {
        }

        public DesertRowanFruitplant(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Version number for future compatibility
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt(); // Read the version (currently version 0)
        }
    }

    public class DesertRowanFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a Desert Rowan seed";
        public override int SeedHue => 2734;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(DesertRowanFruitplant);

        [Constructable]
        public DesertRowanFruitSeed() : base()
        {
        }

        public DesertRowanFruitSeed(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Version number for future compatibility
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt(); // Read the version (currently version 0)
        }
    }	
}
