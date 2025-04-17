using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class grandaFruit : BaseFruit
    {
        public override string FruitName => "granda fruit";
        public override int FruitHue => 2444;
        public override int FruitGraphic => 0x0C5C; // Example fruit graphic
        public override Type SeedType => typeof(grandaFruitSeed);

        [Constructable]
        public grandaFruit() : base()
        {
        }

        [Constructable]
        public grandaFruit(int amount) : base(amount)
        {
        }

        public grandaFruit(Serial serial) : base(serial)
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
    
	public class grandaFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a granda fruit plant";
		public override int PlantHue => 2444;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0CA9; // Harvestable plant graphic
		public override Type FruitType => typeof(grandaFruit);

		[Constructable]
		public grandaFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public grandaFruitplant(Serial serial) : base(serial)
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


    public class grandaFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a granda fruit seed";
        public override int SeedHue => 2444;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(grandaFruitplant);

        [Constructable]
        public grandaFruitSeed() : base()
        {
        }

        public grandaFruitSeed(Serial serial) : base(serial)
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
