using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class gigliachokeFruit : BaseFruit
    {
        public override string FruitName => "gigliachoke fruit";
        public override int FruitHue => 2058;
        public override int FruitGraphic => 0x09D0; // Example fruit graphic
        public override Type SeedType => typeof(gigliachokeFruitSeed);

        [Constructable]
        public gigliachokeFruit() : base()
        {
        }

        [Constructable]
        public gigliachokeFruit(int amount) : base(amount)
        {
        }

        public gigliachokeFruit(Serial serial) : base(serial)
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
    
	public class gigliachokeFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a gigliachoke fruit plant";
		public override int PlantHue => 2058;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0D13; // Harvestable plant graphic
		public override Type FruitType => typeof(gigliachokeFruit);

		[Constructable]
		public gigliachokeFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public gigliachokeFruitplant(Serial serial) : base(serial)
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


    public class gigliachokeFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a gigliachoke fruit seed";
        public override int SeedHue => 2058;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(gigliachokeFruitplant);

        [Constructable]
        public gigliachokeFruitSeed() : base()
        {
        }

        public gigliachokeFruitSeed(Serial serial) : base(serial)
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
