using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class girinFruit : BaseFruit
    {
        public override string FruitName => "girin fruit";
        public override int FruitHue => 264;
        public override int FruitGraphic => 0x0C7A; // Example fruit graphic
        public override Type SeedType => typeof(girinFruitSeed);

        [Constructable]
        public girinFruit() : base()
        {
        }

        [Constructable]
        public girinFruit(int amount) : base(amount)
        {
        }

        public girinFruit(Serial serial) : base(serial)
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
    
	public class girinFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a girin fruit plant";
		public override int PlantHue => 264;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0CCD; // Harvestable plant graphic
		public override Type FruitType => typeof(girinFruit);

		[Constructable]
		public girinFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public girinFruitplant(Serial serial) : base(serial)
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


    public class girinFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a girin fruit seed";
        public override int SeedHue => 264;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(girinFruitplant);

        [Constructable]
        public girinFruitSeed() : base()
        {
        }

        public girinFruitSeed(Serial serial) : base(serial)
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
