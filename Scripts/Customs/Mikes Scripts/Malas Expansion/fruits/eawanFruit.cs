using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class eawanFruit : BaseFruit
    {
        public override string FruitName => "eawan fruit";
        public override int FruitHue => 2628;
        public override int FruitGraphic => 0x0F86; // Example fruit graphic
        public override Type SeedType => typeof(eawanFruitSeed);

        [Constructable]
        public eawanFruit() : base()
        {
        }

        [Constructable]
        public eawanFruit(int amount) : base(amount)
        {
        }

        public eawanFruit(Serial serial) : base(serial)
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
    
	public class eawanFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a eawan fruit plant";
		public override int PlantHue => 2628;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0CCA; // Harvestable plant graphic
		public override Type FruitType => typeof(eawanFruit);

		[Constructable]
		public eawanFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public eawanFruitplant(Serial serial) : base(serial)
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


    public class eawanFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a eawan fruit seed";
        public override int SeedHue => 2628;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(eawanFruitplant);

        [Constructable]
        public eawanFruitSeed() : base()
        {
        }

        public eawanFruitSeed(Serial serial) : base(serial)
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
