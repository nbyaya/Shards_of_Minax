using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class unaFruit : BaseFruit
    {
        public override string FruitName => "una fruit";
        public override int FruitHue => 1095;
        public override int FruitGraphic => 0x1724; // Example fruit graphic
        public override Type SeedType => typeof(unaFruitSeed);

        [Constructable]
        public unaFruit() : base()
        {
        }

        [Constructable]
        public unaFruit(int amount) : base(amount)
        {
        }

        public unaFruit(Serial serial) : base(serial)
        {
        }
    }
    
	public class unaFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a una fruit plant";
		public override int PlantHue => 1095;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0CA7; // Harvestable plant graphic
		public override Type FruitType => typeof(unaFruit);

		[Constructable]
		public unaFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public unaFruitplant(Serial serial) : base(serial)
		{
		}
	}


    public class unaFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a una fruit seed";
        public override int SeedHue => 1095;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(unaFruitplant);

        [Constructable]
        public unaFruitSeed() : base()
        {
        }

        public unaFruitSeed(Serial serial) : base(serial)
        {
        }
    }	
}
