using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class blattiovesFruit : BaseFruit
    {
        public override string FruitName => "blattioves fruit";
        public override int FruitHue => 574;
        public override int FruitGraphic => 0x0F86; // Example fruit graphic
        public override Type SeedType => typeof(blattiovesFruitSeed);

        [Constructable]
        public blattiovesFruit() : base()
        {
        }

        [Constructable]
        public blattiovesFruit(int amount) : base(amount)
        {
        }

        public blattiovesFruit(Serial serial) : base(serial)
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
    
	public class blattiovesFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a blattioves fruit plant";
		public override int PlantHue => 574;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0CC9; // Harvestable plant graphic
		public override Type FruitType => typeof(blattiovesFruit);

		[Constructable]
		public blattiovesFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public blattiovesFruitplant(Serial serial) : base(serial)
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


    public class blattiovesFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a blattioves fruit seed";
        public override int SeedHue => 574;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(blattiovesFruitplant);

        [Constructable]
        public blattiovesFruitSeed() : base()
        {
        }

        public blattiovesFruitSeed(Serial serial) : base(serial)
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
