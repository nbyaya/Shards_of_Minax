using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class AngelTurnipFruit : BaseFruit
    {
        public override string FruitName => "Angel Turnip";
        public override int FruitHue => 2275;
        public override int FruitGraphic => 0x26B7; // Example fruit graphic
        public override Type SeedType => typeof(AngelTurnipFruitSeed);

        [Constructable]
        public AngelTurnipFruit() : base()
        {
        }

        [Constructable]
        public AngelTurnipFruit(int amount) : base(amount)
        {
        }

        public AngelTurnipFruit(Serial serial) : base(serial)
        {
        }
    }
    
	public class AngelTurnipFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a Angel Turnip plant";
		public override int PlantHue => 2275;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0C84; // Harvestable plant graphic
		public override Type FruitType => typeof(AngelTurnipFruit);

		[Constructable]
		public AngelTurnipFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public AngelTurnipFruitplant(Serial serial) : base(serial)
		{
		}
	}


    public class AngelTurnipFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a Angel Turnip seed";
        public override int SeedHue => 2275;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(AngelTurnipFruitplant);

        [Constructable]
        public AngelTurnipFruitSeed() : base()
        {
        }

        public AngelTurnipFruitSeed(Serial serial) : base(serial)
        {
        }
    }	
}
