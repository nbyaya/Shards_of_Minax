using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class vuveFruit : BaseFruit
    {
        public override string FruitName => "vuve fruit";
        public override int FruitHue => 2091;
        public override int FruitGraphic => 0x0F86; // Example fruit graphic
        public override Type SeedType => typeof(vuveFruitSeed);

        [Constructable]
        public vuveFruit() : base()
        {
        }

        [Constructable]
        public vuveFruit(int amount) : base(amount)
        {
        }

        public vuveFruit(Serial serial) : base(serial)
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
    
	public class vuveFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a vuve fruit plant";
		public override int PlantHue => 2091;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0CA4; // Harvestable plant graphic
		public override Type FruitType => typeof(vuveFruit);

		[Constructable]
		public vuveFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public vuveFruitplant(Serial serial) : base(serial)
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


    public class vuveFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a vuve fruit seed";
        public override int SeedHue => 2091;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(vuveFruitplant);

        [Constructable]
        public vuveFruitSeed() : base()
        {
        }

        public vuveFruitSeed(Serial serial) : base(serial)
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
