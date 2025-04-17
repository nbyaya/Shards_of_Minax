using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class krevaFruit : BaseFruit
    {
        public override string FruitName => "kreva fruit";
        public override int FruitHue => 2938;
        public override int FruitGraphic => 0x0C6E; // Example fruit graphic
        public override Type SeedType => typeof(krevaFruitSeed);

        [Constructable]
        public krevaFruit() : base()
        {
        }

        [Constructable]
        public krevaFruit(int amount) : base(amount)
        {
        }

        public krevaFruit(Serial serial) : base(serial)
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
    
	public class krevaFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a kreva fruit plant";
		public override int PlantHue => 2938;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0C9C; // Harvestable plant graphic
		public override Type FruitType => typeof(krevaFruit);

		[Constructable]
		public krevaFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public krevaFruitplant(Serial serial) : base(serial)
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


    public class krevaFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a kreva fruit seed";
        public override int SeedHue => 2938;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(krevaFruitplant);

        [Constructable]
        public krevaFruitSeed() : base()
        {
        }

        public krevaFruitSeed(Serial serial) : base(serial)
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
