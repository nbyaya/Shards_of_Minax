using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class otilFruit : BaseFruit
    {
        public override string FruitName => "otil fruit";
        public override int FruitHue => 2778;
        public override int FruitGraphic => 0x1728; // Example fruit graphic
        public override Type SeedType => typeof(otilFruitSeed);

        [Constructable]
        public otilFruit() : base()
        {
        }

        [Constructable]
        public otilFruit(int amount) : base(amount)
        {
        }

        public otilFruit(Serial serial) : base(serial)
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
    
	public class otilFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a otil fruit plant";
		public override int PlantHue => 2778;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0C98; // Harvestable plant graphic
		public override Type FruitType => typeof(otilFruit);

		[Constructable]
		public otilFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public otilFruitplant(Serial serial) : base(serial)
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


    public class otilFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a otil fruit seed";
        public override int SeedHue => 2778;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(otilFruitplant);

        [Constructable]
        public otilFruitSeed() : base()
        {
        }

        public otilFruitSeed(Serial serial) : base(serial)
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
