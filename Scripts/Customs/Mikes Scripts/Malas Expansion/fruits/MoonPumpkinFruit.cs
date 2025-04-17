using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class MoonPumpkinFruit : BaseFruit
    {
        public override string FruitName => "Moon Pumpkin";
        public override int FruitHue => 2257;
        public override int FruitGraphic => 0x1721; // Example fruit graphic
        public override Type SeedType => typeof(MoonPumpkinFruitSeed);

        [Constructable]
        public MoonPumpkinFruit() : base()
        {
        }

        [Constructable]
        public MoonPumpkinFruit(int amount) : base(amount)
        {
        }

        public MoonPumpkinFruit(Serial serial) : base(serial)
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
    
	public class MoonPumpkinFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a Moon Pumpkin plant";
		public override int PlantHue => 2257;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0DC5; // Harvestable plant graphic
		public override Type FruitType => typeof(MoonPumpkinFruit);

		[Constructable]
		public MoonPumpkinFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public MoonPumpkinFruitplant(Serial serial) : base(serial)
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


    public class MoonPumpkinFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a Moon Pumpkin seed";
        public override int SeedHue => 2257;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(MoonPumpkinFruitplant);

        [Constructable]
        public MoonPumpkinFruitSeed() : base()
        {
        }

        public MoonPumpkinFruitSeed(Serial serial) : base(serial)
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
