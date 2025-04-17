using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class HateCalamansiFruit : BaseFruit
    {
        public override string FruitName => "Hate Calamansi";
        public override int FruitHue => 2627;
        public override int FruitGraphic => 0x172D; // Example fruit graphic
        public override Type SeedType => typeof(HateCalamansiFruitSeed);

        [Constructable]
        public HateCalamansiFruit() : base()
        {
        }

        [Constructable]
        public HateCalamansiFruit(int amount) : base(amount)
        {
        }

        public HateCalamansiFruit(Serial serial) : base(serial)
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
    
	public class HateCalamansiFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a Hate Calamansi plant";
		public override int PlantHue => 2627;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0CCC; // Harvestable plant graphic
		public override Type FruitType => typeof(HateCalamansiFruit);

		[Constructable]
		public HateCalamansiFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public HateCalamansiFruitplant(Serial serial) : base(serial)
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


    public class HateCalamansiFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a Hate Calamansi seed";
        public override int SeedHue => 2627;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(HateCalamansiFruitplant);

        [Constructable]
        public HateCalamansiFruitSeed() : base()
        {
        }

        public HateCalamansiFruitSeed(Serial serial) : base(serial)
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
