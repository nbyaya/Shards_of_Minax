using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class zanioperFruit : BaseFruit
    {
        public override string FruitName => "zanioper fruit";
        public override int FruitHue => 1654;
        public override int FruitGraphic => 0x1724; // Example fruit graphic
        public override Type SeedType => typeof(zanioperFruitSeed);

        [Constructable]
        public zanioperFruit() : base()
        {
        }

        [Constructable]
        public zanioperFruit(int amount) : base(amount)
        {
        }

        public zanioperFruit(Serial serial) : base(serial)
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
    
	public class zanioperFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a zanioper fruit plant";
		public override int PlantHue => 1654;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0C9D; // Harvestable plant graphic
		public override Type FruitType => typeof(zanioperFruit);

		[Constructable]
		public zanioperFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public zanioperFruitplant(Serial serial) : base(serial)
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


    public class zanioperFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a zanioper fruit seed";
        public override int SeedHue => 1654;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(zanioperFruitplant);

        [Constructable]
        public zanioperFruitSeed() : base()
        {
        }

        public zanioperFruitSeed(Serial serial) : base(serial)
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
