using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class BittersweetChivesFruit : BaseFruit
    {
        public override string FruitName => "Bittersweet Chives";
        public override int FruitHue => 1134;
        public override int FruitGraphic => 0x0F7C; // Example fruit graphic
        public override Type SeedType => typeof(BittersweetChivesFruitSeed);

        [Constructable]
        public BittersweetChivesFruit() : base()
        {
        }

        [Constructable]
        public BittersweetChivesFruit(int amount) : base(amount)
        {
        }

        public BittersweetChivesFruit(Serial serial) : base(serial)
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
    
    public class BittersweetChivesFruitplant : BaseFruitPlant
    {
        public override string PlantName => "a Bittersweet Chives plant";
        public override int PlantHue => 1134;
        public override int SeedGraphic => 0x0C45; // Seeds graphic
        public override int HarvestableGraphic => 0x0C51; // Harvestable plant graphic
        public override Type FruitType => typeof(BittersweetChivesFruit);

        [Constructable]
        public BittersweetChivesFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
        {
        }

        public BittersweetChivesFruitplant(Serial serial) : base(serial)
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

    public class BittersweetChivesFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a Bittersweet Chives seed";
        public override int SeedHue => 1134;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(BittersweetChivesFruitplant);

        [Constructable]
        public BittersweetChivesFruitSeed() : base()
        {
        }

        public BittersweetChivesFruitSeed(Serial serial) : base(serial)
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
