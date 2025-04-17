using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class iddiochokeFruit : BaseFruit
    {
        public override string FruitName => "iddiochoke fruit";
        public override int FruitHue => 2963;
        public override int FruitGraphic => 0x172C; // Example fruit graphic
        public override Type SeedType => typeof(iddiochokeFruitSeed);

        [Constructable]
        public iddiochokeFruit() : base()
        {
        }

        [Constructable]
        public iddiochokeFruit(int amount) : base(amount)
        {
        }

        public iddiochokeFruit(Serial serial) : base(serial)
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
    
	public class iddiochokeFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a iddiochoke fruit plant";
		public override int PlantHue => 2963;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0C9B; // Harvestable plant graphic
		public override Type FruitType => typeof(iddiochokeFruit);

		[Constructable]
		public iddiochokeFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public iddiochokeFruitplant(Serial serial) : base(serial)
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


    public class iddiochokeFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a iddiochoke fruit seed";
        public override int SeedHue => 2963;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(iddiochokeFruitplant);

        [Constructable]
        public iddiochokeFruitSeed() : base()
        {
        }

        public iddiochokeFruitSeed(Serial serial) : base(serial)
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
