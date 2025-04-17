using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class hiorolaFruit : BaseFruit
    {
        public override string FruitName => "hiorola fruit";
        public override int FruitHue => 165;
        public override int FruitGraphic => 0x0C77; // Example fruit graphic
        public override Type SeedType => typeof(hiorolaFruitSeed);

        [Constructable]
        public hiorolaFruit() : base()
        {
        }

        [Constructable]
        public hiorolaFruit(int amount) : base(amount)
        {
        }

        public hiorolaFruit(Serial serial) : base(serial)
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
    
	public class hiorolaFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a hiorola fruit plant";
		public override int PlantHue => 165;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0CA8; // Harvestable plant graphic
		public override Type FruitType => typeof(hiorolaFruit);

		[Constructable]
		public hiorolaFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public hiorolaFruitplant(Serial serial) : base(serial)
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


    public class hiorolaFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a hiorola fruit seed";
        public override int SeedHue => 165;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(hiorolaFruitplant);

        [Constructable]
        public hiorolaFruitSeed() : base()
        {
        }

        public hiorolaFruitSeed(Serial serial) : base(serial)
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
