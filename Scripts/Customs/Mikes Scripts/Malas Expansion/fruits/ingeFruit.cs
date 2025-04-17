using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class ingeFruit : BaseFruit
    {
        public override string FruitName => "inge fruit";
        public override int FruitHue => 148;
        public override int FruitGraphic => 0x172B; // Example fruit graphic
        public override Type SeedType => typeof(ingeFruitSeed);

        [Constructable]
        public ingeFruit() : base()
        {
        }

        [Constructable]
        public ingeFruit(int amount) : base(amount)
        {
        }

        public ingeFruit(Serial serial) : base(serial)
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
    
	public class ingeFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a inge fruit plant";
		public override int PlantHue => 148;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0C8B; // Harvestable plant graphic
		public override Type FruitType => typeof(ingeFruit);

		[Constructable]
		public ingeFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public ingeFruitplant(Serial serial) : base(serial)
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


    public class ingeFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a inge fruit seed";
        public override int SeedHue => 148;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(ingeFruitplant);

        [Constructable]
        public ingeFruitSeed() : base()
        {
        }

        public ingeFruitSeed(Serial serial) : base(serial)
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
