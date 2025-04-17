using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class TropicalCherryFruit : BaseFruit
    {
        public override string FruitName => "Tropical Cherry";
        public override int FruitHue => 2253;
        public override int FruitGraphic => 0x1729; // Example fruit graphic
        public override Type SeedType => typeof(TropicalCherryFruitSeed);

        [Constructable]
        public TropicalCherryFruit() : base()
        {
        }

        [Constructable]
        public TropicalCherryFruit(int amount) : base(amount)
        {
        }

        public TropicalCherryFruit(Serial serial) : base(serial)
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
    
	public class TropicalCherryFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a Tropical Cherry plant";
		public override int PlantHue => 2253;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0D26; // Harvestable plant graphic
		public override Type FruitType => typeof(TropicalCherryFruit);

		[Constructable]
		public TropicalCherryFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public TropicalCherryFruitplant(Serial serial) : base(serial)
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


    public class TropicalCherryFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a Tropical Cherry seed";
        public override int SeedHue => 2253;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(TropicalCherryFruitplant);

        [Constructable]
        public TropicalCherryFruitSeed() : base()
        {
        }

        public TropicalCherryFruitSeed(Serial serial) : base(serial)
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
