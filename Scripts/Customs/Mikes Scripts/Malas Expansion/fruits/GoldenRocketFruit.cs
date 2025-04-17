using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class GoldenRocketFruit : BaseFruit
    {
        public override string FruitName => "Golden Rocket";
        public override int FruitHue => 2158;
        public override int FruitGraphic => 0x0C79; // Example fruit graphic
        public override Type SeedType => typeof(GoldenRocketFruitSeed);

        [Constructable]
        public GoldenRocketFruit() : base()
        {
        }

        [Constructable]
        public GoldenRocketFruit(int amount) : base(amount)
        {
        }

        public GoldenRocketFruit(Serial serial) : base(serial)
        {
        }
		
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Versioning for future changes, start with version 0.
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt(); // Read the version number, used for future migrations.
        }		
    }
    
	public class GoldenRocketFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a Golden Rocket plant";
		public override int PlantHue => 2158;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0C4F; // Harvestable plant graphic
		public override Type FruitType => typeof(GoldenRocketFruit);

		[Constructable]
		public GoldenRocketFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public GoldenRocketFruitplant(Serial serial) : base(serial)
		{
		}
		
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Versioning for future changes, start with version 0.
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt(); // Read the version number, used for future migrations.
        }		
	}


    public class GoldenRocketFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a GoldenRocket seed";
        public override int SeedHue => 2158;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(GoldenRocketFruitplant);

        [Constructable]
        public GoldenRocketFruitSeed() : base()
        {
        }

        public GoldenRocketFruitSeed(Serial serial) : base(serial)
        {
        }
		
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Versioning for future changes, start with version 0.
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt(); // Read the version number, used for future migrations.
        }		
    }	
}
