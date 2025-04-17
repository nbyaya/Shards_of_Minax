using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class NightCabbageFruit : BaseFruit
    {
        public override string FruitName => "Night Cabbage";
        public override int FruitHue => 1894;
        public override int FruitGraphic => 0x0C81; // Example fruit graphic
        public override Type SeedType => typeof(NightCabbageFruitSeed);

        [Constructable]
        public NightCabbageFruit() : base()
        {
        }

        [Constructable]
        public NightCabbageFruit(int amount) : base(amount)
        {
        }

        public NightCabbageFruit(Serial serial) : base(serial)
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
    
	public class NightCabbageFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a Night Cabbage plant";
		public override int PlantHue => 1894;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0C51; // Harvestable plant graphic
		public override Type FruitType => typeof(NightCabbageFruit);

		[Constructable]
		public NightCabbageFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public NightCabbageFruitplant(Serial serial) : base(serial)
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


    public class NightCabbageFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a Night Cabbage seed";
        public override int SeedHue => 1894;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(NightCabbageFruitplant);

        [Constructable]
        public NightCabbageFruitSeed() : base()
        {
        }

        public NightCabbageFruitSeed(Serial serial) : base(serial)
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
