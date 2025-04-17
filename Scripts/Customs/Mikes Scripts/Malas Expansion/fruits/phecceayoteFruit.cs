using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class phecceayoteFruit : BaseFruit
    {
        public override string FruitName => "phecceayote fruit";
        public override int FruitHue => 972;
        public override int FruitGraphic => 0x0F88; // Example fruit graphic
        public override Type SeedType => typeof(phecceayoteFruitSeed);

        [Constructable]
        public phecceayoteFruit() : base()
        {
        }

        [Constructable]
        public phecceayoteFruit(int amount) : base(amount)
        {
        }

        public phecceayoteFruit(Serial serial) : base(serial)
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
    
	public class phecceayoteFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a phecceayote fruit plant";
		public override int PlantHue => 972;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0CA5; // Harvestable plant graphic
		public override Type FruitType => typeof(phecceayoteFruit);

		[Constructable]
		public phecceayoteFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public phecceayoteFruitplant(Serial serial) : base(serial)
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


    public class phecceayoteFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a phecceayote fruit seed";
        public override int SeedHue => 972;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(phecceayoteFruitplant);

        [Constructable]
        public phecceayoteFruitSeed() : base()
        {
        }

        public phecceayoteFruitSeed(Serial serial) : base(serial)
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
