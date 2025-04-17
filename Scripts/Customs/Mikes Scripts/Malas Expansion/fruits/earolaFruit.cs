using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class earolaFruit : BaseFruit
    {
        public override string FruitName => "earola fruit";
        public override int FruitHue => 2768;
        public override int FruitGraphic => 0x1726; // Example fruit graphic
        public override Type SeedType => typeof(earolaFruitSeed);

        [Constructable]
        public earolaFruit() : base()
        {
        }

        [Constructable]
        public earolaFruit(int amount) : base(amount)
        {
        }

        public earolaFruit(Serial serial) : base(serial)
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
    
	public class earolaFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a earola fruit plant";
		public override int PlantHue => 2768;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0C8E; // Harvestable plant graphic
		public override Type FruitType => typeof(earolaFruit);

		[Constructable]
		public earolaFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public earolaFruitplant(Serial serial) : base(serial)
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


    public class earolaFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a earola fruit seed";
        public override int SeedHue => 2768;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(earolaFruitplant);

        [Constructable]
        public earolaFruitSeed() : base()
        {
        }

        public earolaFruitSeed(Serial serial) : base(serial)
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
