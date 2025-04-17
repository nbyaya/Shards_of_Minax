using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class TigerBeanFruit : BaseFruit
    {
        public override string FruitName => "Tiger Bean";
        public override int FruitHue => 401;
        public override int FruitGraphic => 0x0F83; // Example fruit graphic
        public override Type SeedType => typeof(TigerBeanFruitSeed);

        [Constructable]
        public TigerBeanFruit() : base()
        {
        }

        [Constructable]
        public TigerBeanFruit(int amount) : base(amount)
        {
        }

        public TigerBeanFruit(Serial serial) : base(serial)
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
    
	public class TigerBeanFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a Tiger Bean plant";
		public override int PlantHue => 401;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0CC7; // Harvestable plant graphic
		public override Type FruitType => typeof(TigerBeanFruit);

		[Constructable]
		public TigerBeanFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public TigerBeanFruitplant(Serial serial) : base(serial)
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


    public class TigerBeanFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a Tiger Bean seed";
        public override int SeedHue => 401;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(TigerBeanFruitplant);

        [Constructable]
        public TigerBeanFruitSeed() : base()
        {
        }

        public TigerBeanFruitSeed(Serial serial) : base(serial)
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
