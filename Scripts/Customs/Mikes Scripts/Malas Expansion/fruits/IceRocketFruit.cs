using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class IceRocketFruit : BaseFruit
    {
        public override string FruitName => "Ice Rocket";
        public override int FruitHue => 2912;
        public override int FruitGraphic => 0x0F84; // Example fruit graphic
        public override Type SeedType => typeof(IceRocketFruitSeed);

        [Constructable]
        public IceRocketFruit() : base()
        {
        }

        [Constructable]
        public IceRocketFruit(int amount) : base(amount)
        {
        }

        public IceRocketFruit(Serial serial) : base(serial)
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
    
	public class IceRocketFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a Ice Rocket plant";
		public override int PlantHue => 2912;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0C9A; // Harvestable plant graphic
		public override Type FruitType => typeof(IceRocketFruit);

		[Constructable]
		public IceRocketFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public IceRocketFruitplant(Serial serial) : base(serial)
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


    public class IceRocketFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a Ice Rocket seed";
        public override int SeedHue => 2912;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(IceRocketFruitplant);

        [Constructable]
        public IceRocketFruitSeed() : base()
        {
        }

        public IceRocketFruitSeed(Serial serial) : base(serial)
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
