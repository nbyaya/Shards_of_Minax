using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class HazelLimeFruit : BaseFruit
    {
        public override string FruitName => "Hazel Lime";
        public override int FruitHue => 611;
        public override int FruitGraphic => 0x0F8D; // Example fruit graphic
        public override Type SeedType => typeof(HazelLimeFruitSeed);

        [Constructable]
        public HazelLimeFruit() : base()
        {
        }

        [Constructable]
        public HazelLimeFruit(int amount) : base(amount)
        {
        }

        public HazelLimeFruit(Serial serial) : base(serial)
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
    
	public class HazelLimeFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a Hazel Lime plant";
		public override int PlantHue => 611;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0C57; // Harvestable plant graphic
		public override Type FruitType => typeof(HazelLimeFruit);

		[Constructable]
		public HazelLimeFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public HazelLimeFruitplant(Serial serial) : base(serial)
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


    public class HazelLimeFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a Hazel Lime seed";
        public override int SeedHue => 611;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(HazelLimeFruitplant);

        [Constructable]
        public HazelLimeFruitSeed() : base()
        {
        }

        public HazelLimeFruitSeed(Serial serial) : base(serial)
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
