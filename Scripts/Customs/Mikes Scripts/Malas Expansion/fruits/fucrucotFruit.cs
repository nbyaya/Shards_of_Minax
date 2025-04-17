using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class fucrucotFruit : BaseFruit
    {
        public override string FruitName => "fucrucot fruit";
        public override int FruitHue => 2474;
        public override int FruitGraphic => 0x0C65; // Example fruit graphic
        public override Type SeedType => typeof(fucrucotFruitSeed);

        [Constructable]
        public fucrucotFruit() : base()
        {
        }

        [Constructable]
        public fucrucotFruit(int amount) : base(amount)
        {
        }

        public fucrucotFruit(Serial serial) : base(serial)
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
    
	public class fucrucotFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a fucrucot fruit plant";
		public override int PlantHue => 2474;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0D0B; // Harvestable plant graphic
		public override Type FruitType => typeof(fucrucotFruit);

		[Constructable]
		public fucrucotFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public fucrucotFruitplant(Serial serial) : base(serial)
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


    public class fucrucotFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a fucrucot fruit seed";
        public override int SeedHue => 2474;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(fucrucotFruitplant);

        [Constructable]
        public fucrucotFruitSeed() : base()
        {
        }

        public fucrucotFruitSeed(Serial serial) : base(serial)
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
