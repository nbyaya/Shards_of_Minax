using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class piokinFruit : BaseFruit
    {
        public override string FruitName => "piokin fruit";
        public override int FruitHue => 2751;
        public override int FruitGraphic => 0x0C5D; // Example fruit graphic
        public override Type SeedType => typeof(piokinFruitSeed);

        [Constructable]
        public piokinFruit() : base()
        {
        }

        [Constructable]
        public piokinFruit(int amount) : base(amount)
        {
        }

        public piokinFruit(Serial serial) : base(serial)
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
    
	public class piokinFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a piokin fruit plant";
		public override int PlantHue => 2751;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0D40; // Harvestable plant graphic
		public override Type FruitType => typeof(piokinFruit);

		[Constructable]
		public piokinFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public piokinFruitplant(Serial serial) : base(serial)
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


    public class piokinFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a piokin fruit seed";
        public override int SeedHue => 2751;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(piokinFruitplant);

        [Constructable]
        public piokinFruitSeed() : base()
        {
        }

        public piokinFruitSeed(Serial serial) : base(serial)
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
