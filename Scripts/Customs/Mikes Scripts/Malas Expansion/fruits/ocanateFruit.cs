using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class ocanateFruit : BaseFruit
    {
        public override string FruitName => "ocanate fruit";
        public override int FruitHue => 240;
        public override int FruitGraphic => 0x0F85; // Example fruit graphic
        public override Type SeedType => typeof(ocanateFruitSeed);

        [Constructable]
        public ocanateFruit() : base()
        {
        }

        [Constructable]
        public ocanateFruit(int amount) : base(amount)
        {
        }

        public ocanateFruit(Serial serial) : base(serial)
        {
        }
    }
    
	public class ocanateFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a ocanate fruit plant";
		public override int PlantHue => 240;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0C89; // Harvestable plant graphic
		public override Type FruitType => typeof(ocanateFruit);

		[Constructable]
		public ocanateFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public ocanateFruitplant(Serial serial) : base(serial)
		{
		}
	}


    public class ocanateFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a ocanate fruit seed";
        public override int SeedHue => 240;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(ocanateFruitplant);

        [Constructable]
        public ocanateFruitSeed() : base()
        {
        }

        public ocanateFruitSeed(Serial serial) : base(serial)
        {
        }
    }	
}
