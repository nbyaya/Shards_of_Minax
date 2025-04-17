using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class moyiarlanFruit : BaseFruit
    {
        public override string FruitName => "moyiarlan fruit";
        public override int FruitHue => 2292;
        public override int FruitGraphic => 0x0C72; // Example fruit graphic
        public override Type SeedType => typeof(moyiarlanFruitSeed);

        [Constructable]
        public moyiarlanFruit() : base()
        {
        }

        [Constructable]
        public moyiarlanFruit(int amount) : base(amount)
        {
        }

        public moyiarlanFruit(Serial serial) : base(serial)
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
    
	public class moyiarlanFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a moyiarlan fruit plant";
		public override int PlantHue => 2292;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0C8F; // Harvestable plant graphic
		public override Type FruitType => typeof(moyiarlanFruit);

		[Constructable]
		public moyiarlanFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public moyiarlanFruitplant(Serial serial) : base(serial)
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


    public class moyiarlanFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a moyiarlan fruit seed";
        public override int SeedHue => 2292;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(moyiarlanFruitplant);

        [Constructable]
        public moyiarlanFruitSeed() : base()
        {
        }

        public moyiarlanFruitSeed(Serial serial) : base(serial)
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
