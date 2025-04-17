using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class FalseAlmondFruit : BaseFruit
    {
        public override string FruitName => "False Almond";
        public override int FruitHue => 370;
        public override int FruitGraphic => 0x0F8C; // Example fruit graphic
        public override Type SeedType => typeof(FalseAlmondFruitSeed);

        [Constructable]
        public FalseAlmondFruit() : base()
        {
        }

        [Constructable]
        public FalseAlmondFruit(int amount) : base(amount)
        {
        }

        public FalseAlmondFruit(Serial serial) : base(serial)
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
    
	public class FalseAlmondFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a False Almond plant";
		public override int PlantHue => 370;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0C8C; // Harvestable plant graphic
		public override Type FruitType => typeof(FalseAlmondFruit);

		[Constructable]
		public FalseAlmondFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public FalseAlmondFruitplant(Serial serial) : base(serial)
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


    public class FalseAlmondFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a False Almond seed";
        public override int SeedHue => 370;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(FalseAlmondFruitplant);

        [Constructable]
        public FalseAlmondFruitSeed() : base()
        {
        }

        public FalseAlmondFruitSeed(Serial serial) : base(serial)
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
