using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class WonderRambutanFruit : BaseFruit
    {
        public override string FruitName => "Wonder Rambutan";
        public override int FruitHue => 2805;
        public override int FruitGraphic => 0x0C6E; // Example fruit graphic
        public override Type SeedType => typeof(WonderRambutanFruitSeed);

        [Constructable]
        public WonderRambutanFruit() : base()
        {
        }

        [Constructable]
        public WonderRambutanFruit(int amount) : base(amount)
        {
        }

        public WonderRambutanFruit(Serial serial) : base(serial)
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
    
	public class WonderRambutanFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a Wonder Rambutan plant";
		public override int PlantHue => 2805;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0C55; // Harvestable plant graphic
		public override Type FruitType => typeof(WonderRambutanFruit);

		[Constructable]
		public WonderRambutanFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public WonderRambutanFruitplant(Serial serial) : base(serial)
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


    public class WonderRambutanFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a WonderRambutan seed";
        public override int SeedHue => 2805;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(WonderRambutanFruitplant);

        [Constructable]
        public WonderRambutanFruitSeed() : base()
        {
        }

        public WonderRambutanFruitSeed(Serial serial) : base(serial)
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
