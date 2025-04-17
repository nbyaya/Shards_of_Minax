using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class DrakeLentilFruit : BaseFruit
    {
        public override string FruitName => "Drake Lentil";
        public override int FruitHue => 1556;
        public override int FruitGraphic => 0x0C81; // Example fruit graphic
        public override Type SeedType => typeof(DrakeLentilFruitSeed);

        [Constructable]
        public DrakeLentilFruit() : base()
        {
        }

        [Constructable]
        public DrakeLentilFruit(int amount) : base(amount)
        {
        }

        public DrakeLentilFruit(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version number
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt(); // read version number
        }
    }
    
    public class DrakeLentilFruitplant : BaseFruitPlant
    {
        public override string PlantName => "a Drake Lentil plant";
        public override int PlantHue => 1556;
        public override int SeedGraphic => 0x0C45; // Seeds graphic
        public override int HarvestableGraphic => 0x0C91; // Harvestable plant graphic
        public override Type FruitType => typeof(DrakeLentilFruit);

        [Constructable]
        public DrakeLentilFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
        {
        }

        public DrakeLentilFruitplant(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version number
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt(); // read version number
        }
    }

    public class DrakeLentilFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a Drake Lentil seed";
        public override int SeedHue => 1556;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(DrakeLentilFruitplant);

        [Constructable]
        public DrakeLentilFruitSeed() : base()
        {
        }

        public DrakeLentilFruitSeed(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version number
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt(); // read version number
        }
    }	
}
