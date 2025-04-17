using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class strondaFruit : BaseFruit
    {
        public override string FruitName => "stronda fruit";
        public override int FruitHue => 1083;
        public override int FruitGraphic => 0x1722; // Example fruit graphic
        public override Type SeedType => typeof(strondaFruitSeed);

        [Constructable]
        public strondaFruit() : base()
        {
        }

        [Constructable]
        public strondaFruit(int amount) : base(amount)
        {
        }

        public strondaFruit(Serial serial) : base(serial)
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
    
	public class strondaFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a stronda fruit plant";
		public override int PlantHue => 1083;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0C9F; // Harvestable plant graphic
		public override Type FruitType => typeof(strondaFruit);

		[Constructable]
		public strondaFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public strondaFruitplant(Serial serial) : base(serial)
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


    public class strondaFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a stronda fruit seed";
        public override int SeedHue => 1083;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(strondaFruitplant);

        [Constructable]
        public strondaFruitSeed() : base()
        {
        }

        public strondaFruitSeed(Serial serial) : base(serial)
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
