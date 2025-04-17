using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class hialoupeFruit : BaseFruit
    {
        public override string FruitName => "hialoupe fruit";
        public override int FruitHue => 1910;
        public override int FruitGraphic => 0x0F88; // Example fruit graphic
        public override Type SeedType => typeof(hialoupeFruitSeed);

        [Constructable]
        public hialoupeFruit() : base()
        {
        }

        [Constructable]
        public hialoupeFruit(int amount) : base(amount)
        {
        }

        public hialoupeFruit(Serial serial) : base(serial)
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
    
	public class hialoupeFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a hialoupe fruit plant";
		public override int PlantHue => 1910;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0C8A; // Harvestable plant graphic
		public override Type FruitType => typeof(hialoupeFruit);

		[Constructable]
		public hialoupeFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public hialoupeFruitplant(Serial serial) : base(serial)
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


    public class hialoupeFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a hialoupe fruit seed";
        public override int SeedHue => 1910;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(hialoupeFruitplant);

        [Constructable]
        public hialoupeFruitSeed() : base()
        {
        }

        public hialoupeFruitSeed(Serial serial) : base(serial)
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
