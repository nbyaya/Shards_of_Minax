using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class slomeloFruit : BaseFruit
    {
        public override string FruitName => "slomelo fruit";
        public override int FruitHue => 303;
        public override int FruitGraphic => 0x0C75; // Example fruit graphic
        public override Type SeedType => typeof(slomeloFruitSeed);

        [Constructable]
        public slomeloFruit() : base()
        {
        }

        [Constructable]
        public slomeloFruit(int amount) : base(amount)
        {
        }

        public slomeloFruit(Serial serial) : base(serial)
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
    
	public class slomeloFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a slomelo fruit plant";
		public override int PlantHue => 303;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0C9B; // Harvestable plant graphic
		public override Type FruitType => typeof(slomeloFruit);

		[Constructable]
		public slomeloFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public slomeloFruitplant(Serial serial) : base(serial)
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


    public class slomeloFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a slomelo fruit seed";
        public override int SeedHue => 303;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(slomeloFruitplant);

        [Constructable]
        public slomeloFruitSeed() : base()
        {
        }

        public slomeloFruitSeed(Serial serial) : base(serial)
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
