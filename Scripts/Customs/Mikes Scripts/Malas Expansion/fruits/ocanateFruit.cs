using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class ocanateFruit : BaseFruit
    {
        public override string FruitName => "ocanate fruit";
        public override int FruitHue => 240;
        public override int FruitGraphic => 0x0F85; // Example fruit graphic
        public override Type SeedType => typeof(ocanateFruitSeed);

        [Constructable]
        public ocanateFruit() : base()
        {
        }

        [Constructable]
        public ocanateFruit(int amount) : base(amount)
        {
        }

        public ocanateFruit(Serial serial) : base(serial)
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
    
	public class ocanateFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a ocanate fruit plant";
		public override int PlantHue => 240;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0C89; // Harvestable plant graphic
		public override Type FruitType => typeof(ocanateFruit);

		[Constructable]
		public ocanateFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public ocanateFruitplant(Serial serial) : base(serial)
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


    public class ocanateFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a ocanate fruit seed";
        public override int SeedHue => 240;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(ocanateFruitplant);

        [Constructable]
        public ocanateFruitSeed() : base()
        {
        }

        public ocanateFruitSeed(Serial serial) : base(serial)
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
