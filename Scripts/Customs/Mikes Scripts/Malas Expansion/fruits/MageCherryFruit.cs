using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class MageCherryFruit : BaseFruit
    {
        public override string FruitName => "Mage Cherry";
        public override int FruitHue => 2957;
        public override int FruitGraphic => 0x1723; // Example fruit graphic
        public override Type SeedType => typeof(MageCherryFruitSeed);

        [Constructable]
        public MageCherryFruit() : base()
        {
        }

        [Constructable]
        public MageCherryFruit(int amount) : base(amount)
        {
        }

        public MageCherryFruit(Serial serial) : base(serial)
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
    
	public class MageCherryFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a Mage Cherry plant";
		public override int PlantHue => 2957;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0DED; // Harvestable plant graphic
		public override Type FruitType => typeof(MageCherryFruit);

		[Constructable]
		public MageCherryFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public MageCherryFruitplant(Serial serial) : base(serial)
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


    public class MageCherryFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a Mage Cherry seed";
        public override int SeedHue => 2957;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(MageCherryFruitplant);

        [Constructable]
        public MageCherryFruitSeed() : base()
        {
        }

        public MageCherryFruitSeed(Serial serial) : base(serial)
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
