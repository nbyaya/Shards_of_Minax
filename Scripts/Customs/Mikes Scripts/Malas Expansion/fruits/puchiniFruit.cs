using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class puchiniFruit : BaseFruit
    {
        public override string FruitName => "puchini fruit";
        public override int FruitHue => 2014;
        public override int FruitGraphic => 0x0C73; // Example fruit graphic
        public override Type SeedType => typeof(puchiniFruitSeed);

        [Constructable]
        public puchiniFruit() : base()
        {
        }

        [Constructable]
        public puchiniFruit(int amount) : base(amount)
        {
        }

        public puchiniFruit(Serial serial) : base(serial)
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
    
	public class puchiniFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a puchini fruit plant";
		public override int PlantHue => 2014;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0C55; // Harvestable plant graphic
		public override Type FruitType => typeof(puchiniFruit);

		[Constructable]
		public puchiniFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public puchiniFruitplant(Serial serial) : base(serial)
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


    public class puchiniFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a puchini fruit seed";
        public override int SeedHue => 2014;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(puchiniFruitplant);

        [Constructable]
        public puchiniFruitSeed() : base()
        {
        }

        public puchiniFruitSeed(Serial serial) : base(serial)
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
