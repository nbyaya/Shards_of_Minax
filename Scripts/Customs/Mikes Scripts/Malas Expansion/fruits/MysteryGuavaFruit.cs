using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class MysteryGuavaFruit : BaseFruit
    {
        public override string FruitName => "Mystery Guava";
        public override int FruitHue => 1626;
        public override int FruitGraphic => 0x09D0; // Example fruit graphic
        public override Type SeedType => typeof(MysteryGuavaFruitSeed);

        [Constructable]
        public MysteryGuavaFruit() : base()
        {
        }

        [Constructable]
        public MysteryGuavaFruit(int amount) : base(amount)
        {
        }

        public MysteryGuavaFruit(Serial serial) : base(serial)
        {
        }
    }
    
	public class MysteryGuavaFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a Mystery Guava plant";
		public override int PlantHue => 1626;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0D26; // Harvestable plant graphic
		public override Type FruitType => typeof(MysteryGuavaFruit);

		[Constructable]
		public MysteryGuavaFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public MysteryGuavaFruitplant(Serial serial) : base(serial)
		{
		}
	}


    public class MysteryGuavaFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a Mystery Guava seed";
        public override int SeedHue => 1626;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(MysteryGuavaFruitplant);

        [Constructable]
        public MysteryGuavaFruitSeed() : base()
        {
        }

        public MysteryGuavaFruitSeed(Serial serial) : base(serial)
        {
        }
    }	
}
