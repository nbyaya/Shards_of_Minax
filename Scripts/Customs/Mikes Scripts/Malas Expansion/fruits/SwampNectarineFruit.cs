using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class SwampNectarineFruit : BaseFruit
    {
        public override string FruitName => "Swamp Nectarine";
        public override int FruitHue => 2637;
        public override int FruitGraphic => 0x09D1; // Example fruit graphic
        public override Type SeedType => typeof(SwampNectarineFruitSeed);

        [Constructable]
        public SwampNectarineFruit() : base()
        {
        }

        [Constructable]
        public SwampNectarineFruit(int amount) : base(amount)
        {
        }

        public SwampNectarineFruit(Serial serial) : base(serial)
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
    
	public class SwampNectarineFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a Swamp Nectarine plant";
		public override int PlantHue => 2637;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0C8F; // Harvestable plant graphic
		public override Type FruitType => typeof(SwampNectarineFruit);

		[Constructable]
		public SwampNectarineFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public SwampNectarineFruitplant(Serial serial) : base(serial)
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


    public class SwampNectarineFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a Swamp Nectarine seed";
        public override int SeedHue => 2637;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(SwampNectarineFruitplant);

        [Constructable]
        public SwampNectarineFruitSeed() : base()
        {
        }

        public SwampNectarineFruitSeed(Serial serial) : base(serial)
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
