using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class ippiocressFruit : BaseFruit
    {
        public override string FruitName => "ippiocress fruit";
        public override int FruitHue => 1208;
        public override int FruitGraphic => 0x0C7F; // Example fruit graphic
        public override Type SeedType => typeof(ippiocressFruitSeed);

        [Constructable]
        public ippiocressFruit() : base()
        {
        }

        [Constructable]
        public ippiocressFruit(int amount) : base(amount)
        {
        }

        public ippiocressFruit(Serial serial) : base(serial)
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
    
	public class ippiocressFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a ippiocress fruit plant";
		public override int PlantHue => 1208;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0CC4; // Harvestable plant graphic
		public override Type FruitType => typeof(ippiocressFruit);

		[Constructable]
		public ippiocressFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public ippiocressFruitplant(Serial serial) : base(serial)
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


    public class ippiocressFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a ippiocress fruit seed";
        public override int SeedHue => 1208;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(ippiocressFruitplant);

        [Constructable]
        public ippiocressFruitSeed() : base()
        {
        }

        public ippiocressFruitSeed(Serial serial) : base(serial)
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
