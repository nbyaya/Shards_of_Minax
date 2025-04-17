using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class vropperrotFruit : BaseFruit
    {
        public override string FruitName => "vropperrot fruit";
        public override int FruitHue => 1349;
        public override int FruitGraphic => 0x0C77; // Example fruit graphic
        public override Type SeedType => typeof(vropperrotFruitSeed);

        [Constructable]
        public vropperrotFruit() : base()
        {
        }

        [Constructable]
        public vropperrotFruit(int amount) : base(amount)
        {
        }

        public vropperrotFruit(Serial serial) : base(serial)
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
    
	public class vropperrotFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a vropperrot fruit plant";
		public override int PlantHue => 1349;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0CC8; // Harvestable plant graphic
		public override Type FruitType => typeof(vropperrotFruit);

		[Constructable]
		public vropperrotFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public vropperrotFruitplant(Serial serial) : base(serial)
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


    public class vropperrotFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a vropperrot fruit seed";
        public override int SeedHue => 1349;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(vropperrotFruitplant);

        [Constructable]
        public vropperrotFruitSeed() : base()
        {
        }

        public vropperrotFruitSeed(Serial serial) : base(serial)
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
