using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class ElephantBreadnutFruit : BaseFruit
    {
        public override string FruitName => "Elephant Breadnut";
        public override int FruitHue => 2464;
        public override int FruitGraphic => 0x0C6A; // Example fruit graphic
        public override Type SeedType => typeof(ElephantBreadnutFruitSeed);

        [Constructable]
        public ElephantBreadnutFruit() : base()
        {
        }

        [Constructable]
        public ElephantBreadnutFruit(int amount) : base(amount)
        {
        }

        public ElephantBreadnutFruit(Serial serial) : base(serial)
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
    
	public class ElephantBreadnutFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a Elephant Breadnut plant";
		public override int PlantHue => 2464;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0CCB; // Harvestable plant graphic
		public override Type FruitType => typeof(ElephantBreadnutFruit);

		[Constructable]
		public ElephantBreadnutFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public ElephantBreadnutFruitplant(Serial serial) : base(serial)
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


    public class ElephantBreadnutFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a Elephant Breadnut seed";
        public override int SeedHue => 2464;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(ElephantBreadnutFruitplant);

        [Constructable]
        public ElephantBreadnutFruitSeed() : base()
        {
        }

        public ElephantBreadnutFruitSeed(Serial serial) : base(serial)
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
