using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class echocadoFruit : BaseFruit
    {
        public override string FruitName => "echocado fruit";
        public override int FruitHue => 698;
        public override int FruitGraphic => 0x0F7C; // Example fruit graphic
        public override Type SeedType => typeof(echocadoFruitSeed);

        [Constructable]
        public echocadoFruit() : base()
        {
        }

        [Constructable]
        public echocadoFruit(int amount) : base(amount)
        {
        }

        public echocadoFruit(Serial serial) : base(serial)
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
    
	public class echocadoFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a echocado fruit plant";
		public override int PlantHue => 698;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0D25; // Harvestable plant graphic
		public override Type FruitType => typeof(echocadoFruit);

		[Constructable]
		public echocadoFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public echocadoFruitplant(Serial serial) : base(serial)
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


    public class echocadoFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a echocado fruit seed";
        public override int SeedHue => 698;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(echocadoFruitplant);

        [Constructable]
        public echocadoFruitSeed() : base()
        {
        }

        public echocadoFruitSeed(Serial serial) : base(serial)
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
