using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class vrecequilaFruit : BaseFruit
    {
        public override string FruitName => "vrecequila fruit";
        public override int FruitHue => 2526;
        public override int FruitGraphic => 0x0C74; // Example fruit graphic
        public override Type SeedType => typeof(vrecequilaFruitSeed);

        [Constructable]
        public vrecequilaFruit() : base()
        {
        }

        [Constructable]
        public vrecequilaFruit(int amount) : base(amount)
        {
        }

        public vrecequilaFruit(Serial serial) : base(serial)
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
    
	public class vrecequilaFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a vrecequila fruit plant";
		public override int PlantHue => 2526;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0CAA; // Harvestable plant graphic
		public override Type FruitType => typeof(vrecequilaFruit);

		[Constructable]
		public vrecequilaFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public vrecequilaFruitplant(Serial serial) : base(serial)
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


    public class vrecequilaFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a vrecequila fruit seed";
        public override int SeedHue => 2526;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(vrecequilaFruitplant);

        [Constructable]
        public vrecequilaFruitSeed() : base()
        {
        }

        public vrecequilaFruitSeed(Serial serial) : base(serial)
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
