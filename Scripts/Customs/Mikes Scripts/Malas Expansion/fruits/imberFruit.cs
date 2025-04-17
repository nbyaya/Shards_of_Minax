using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class imberFruit : BaseFruit
    {
        public override string FruitName => "imber fruit";
        public override int FruitHue => 2181;
        public override int FruitGraphic => 0x0C65; // Example fruit graphic
        public override Type SeedType => typeof(imberFruitSeed);

        [Constructable]
        public imberFruit() : base()
        {
        }

        [Constructable]
        public imberFruit(int amount) : base(amount)
        {
        }

        public imberFruit(Serial serial) : base(serial)
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
    
	public class imberFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a imber fruit plant";
		public override int PlantHue => 2181;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0C97; // Harvestable plant graphic
		public override Type FruitType => typeof(imberFruit);

		[Constructable]
		public imberFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public imberFruitplant(Serial serial) : base(serial)
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


    public class imberFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a imber fruit seed";
        public override int SeedHue => 2181;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(imberFruitplant);

        [Constructable]
        public imberFruitSeed() : base()
        {
        }

        public imberFruitSeed(Serial serial) : base(serial)
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
