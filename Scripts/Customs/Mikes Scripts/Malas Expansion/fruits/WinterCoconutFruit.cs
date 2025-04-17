using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class WinterCoconutFruit : BaseFruit
    {
        public override string FruitName => "Winter Coconut";
        public override int FruitHue => 447;
        public override int FruitGraphic => 0x09D1; // Example fruit graphic
        public override Type SeedType => typeof(WinterCoconutFruitSeed);

        [Constructable]
        public WinterCoconutFruit() : base()
        {
        }

        [Constructable]
        public WinterCoconutFruit(int amount) : base(amount)
        {
        }

        public WinterCoconutFruit(Serial serial) : base(serial)
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
    
	public class WinterCoconutFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a Winter Coconut plant";
		public override int PlantHue => 447;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0CC1; // Harvestable plant graphic
		public override Type FruitType => typeof(WinterCoconutFruit);

		[Constructable]
		public WinterCoconutFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public WinterCoconutFruitplant(Serial serial) : base(serial)
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


    public class WinterCoconutFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a Winter Coconut seed";
        public override int SeedHue => 447;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(WinterCoconutFruitplant);

        [Constructable]
        public WinterCoconutFruitSeed() : base()
        {
        }

        public WinterCoconutFruitSeed(Serial serial) : base(serial)
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
