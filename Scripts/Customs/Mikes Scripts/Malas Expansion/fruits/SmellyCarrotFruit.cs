using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class SmellyCarrotFruit : BaseFruit
    {
        public override string FruitName => "Smelly Carrot";
        public override int FruitHue => 1113;
        public override int FruitGraphic => 0x09D2; // Example fruit graphic
        public override Type SeedType => typeof(SmellyCarrotFruitSeed);

        [Constructable]
        public SmellyCarrotFruit() : base()
        {
        }

        [Constructable]
        public SmellyCarrotFruit(int amount) : base(amount)
        {
        }

        public SmellyCarrotFruit(Serial serial) : base(serial)
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
    
	public class SmellyCarrotFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a Smelly Carrot plant";
		public override int PlantHue => 1113;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0D04; // Harvestable plant graphic
		public override Type FruitType => typeof(SmellyCarrotFruit);

		[Constructable]
		public SmellyCarrotFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public SmellyCarrotFruitplant(Serial serial) : base(serial)
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


    public class SmellyCarrotFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a Smelly Carrot seed";
        public override int SeedHue => 1113;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(SmellyCarrotFruitplant);

        [Constructable]
        public SmellyCarrotFruitSeed() : base()
        {
        }

        public SmellyCarrotFruitSeed(Serial serial) : base(serial)
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
