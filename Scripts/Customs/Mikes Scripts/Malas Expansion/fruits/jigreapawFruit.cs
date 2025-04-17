using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class jigreapawFruit : BaseFruit
    {
        public override string FruitName => "jigreapaw fruit";
        public override int FruitHue => 924;
        public override int FruitGraphic => 0x0F8F; // Example fruit graphic
        public override Type SeedType => typeof(jigreapawFruitSeed);

        [Constructable]
        public jigreapawFruit() : base()
        {
        }

        [Constructable]
        public jigreapawFruit(int amount) : base(amount)
        {
        }

        public jigreapawFruit(Serial serial) : base(serial)
        {
        }
    }
    
	public class jigreapawFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a jigreapaw fruit plant";
		public override int PlantHue => 924;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0C92; // Harvestable plant graphic
		public override Type FruitType => typeof(jigreapawFruit);

		[Constructable]
		public jigreapawFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public jigreapawFruitplant(Serial serial) : base(serial)
		{
		}
	}


    public class jigreapawFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a jigreapaw fruit seed";
        public override int SeedHue => 924;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(jigreapawFruitplant);

        [Constructable]
        public jigreapawFruitSeed() : base()
        {
        }

        public jigreapawFruitSeed(Serial serial) : base(serial)
        {
        }
    }	
}
