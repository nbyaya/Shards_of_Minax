using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class blearelFruit : BaseFruit
    {
        public override string FruitName => "blearel fruit";
        public override int FruitHue => 1514;
        public override int FruitGraphic => 0x0C7A; // Example fruit graphic
        public override Type SeedType => typeof(blearelFruitSeed);

        [Constructable]
        public blearelFruit() : base()
        {
        }

        [Constructable]
        public blearelFruit(int amount) : base(amount)
        {
        }

        public blearelFruit(Serial serial) : base(serial)
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
    
	public class blearelFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a blearel fruit plant";
		public override int PlantHue => 1514;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0CCA; // Harvestable plant graphic
		public override Type FruitType => typeof(blearelFruit);

		[Constructable]
		public blearelFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public blearelFruitplant(Serial serial) : base(serial)
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


    public class blearelFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a blearel fruit seed";
        public override int SeedHue => 1514;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(blearelFruitplant);

        [Constructable]
        public blearelFruitSeed() : base()
        {
        }

        public blearelFruitSeed(Serial serial) : base(serial)
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
