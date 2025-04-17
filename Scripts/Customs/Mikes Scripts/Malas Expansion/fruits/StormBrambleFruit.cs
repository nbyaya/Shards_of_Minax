using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class StormBrambleFruit : BaseFruit
    {
        public override string FruitName => "Storm Bramble";
        public override int FruitHue => 2655;
        public override int FruitGraphic => 0x09D1; // Example fruit graphic
        public override Type SeedType => typeof(StormBrambleFruitSeed);

        [Constructable]
        public StormBrambleFruit() : base()
        {
        }

        [Constructable]
        public StormBrambleFruit(int amount) : base(amount)
        {
        }

        public StormBrambleFruit(Serial serial) : base(serial)
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
    
	public class StormBrambleFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a Storm Bramble plant";
		public override int PlantHue => 2655;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0C90; // Harvestable plant graphic
		public override Type FruitType => typeof(StormBrambleFruit);

		[Constructable]
		public StormBrambleFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public StormBrambleFruitplant(Serial serial) : base(serial)
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


    public class StormBrambleFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a Storm Bramble seed";
        public override int SeedHue => 2655;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(StormBrambleFruitplant);

        [Constructable]
        public StormBrambleFruitSeed() : base()
        {
        }

        public StormBrambleFruitSeed(Serial serial) : base(serial)
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
