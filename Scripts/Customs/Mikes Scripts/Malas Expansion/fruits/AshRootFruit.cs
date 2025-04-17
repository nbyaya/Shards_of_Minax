using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class AshRootFruit : BaseFruit
    {
        public override string FruitName => "Ash Root";
        public override int FruitHue => 1344;
        public override int FruitGraphic => 0x1724; // Example fruit graphic
        public override Type SeedType => typeof(AshRootFruitSeed);

        [Constructable]
        public AshRootFruit() : base()
        {
        }

        [Constructable]
        public AshRootFruit(int amount) : base(amount)
        {
        }

        public AshRootFruit(Serial serial) : base(serial)
        {
        }
    }
    
	public class AshRootFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a Ash Root plant";
		public override int PlantHue => 1344;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0D40; // Harvestable plant graphic
		public override Type FruitType => typeof(AshRootFruit);

		[Constructable]
		public AshRootFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public AshRootFruitplant(Serial serial) : base(serial)
		{
		}
	}


    public class AshRootFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a Ash Root seed";
        public override int SeedHue => 1344;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(AshRootFruitplant);

        [Constructable]
        public AshRootFruitSeed() : base()
        {
        }

        public AshRootFruitSeed(Serial serial) : base(serial)
        {
        }
    }	
}
