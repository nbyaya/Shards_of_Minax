using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class RainLaurelFruit : BaseFruit
    {
        public override string FruitName => "Rain Laurel";
        public override int FruitHue => 442;
        public override int FruitGraphic => 0x1729; // Example fruit graphic
        public override Type SeedType => typeof(RainLaurelFruitSeed);

        [Constructable]
        public RainLaurelFruit() : base()
        {
        }

        [Constructable]
        public RainLaurelFruit(int amount) : base(amount)
        {
        }

        public RainLaurelFruit(Serial serial) : base(serial)
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
    
	public class RainLaurelFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a Rain Laurel plant";
		public override int PlantHue => 442;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0C8E; // Harvestable plant graphic
		public override Type FruitType => typeof(RainLaurelFruit);

		[Constructable]
		public RainLaurelFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public RainLaurelFruitplant(Serial serial) : base(serial)
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


    public class RainLaurelFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a Rain Laurel seed";
        public override int SeedHue => 442;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(RainLaurelFruitplant);

        [Constructable]
        public RainLaurelFruitSeed() : base()
        {
        }

        public RainLaurelFruitSeed(Serial serial) : base(serial)
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
