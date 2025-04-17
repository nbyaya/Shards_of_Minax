using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class fudishFruit : BaseFruit
    {
        public override string FruitName => "fudish fruit";
        public override int FruitHue => 1913;
        public override int FruitGraphic => 0x172B; // Example fruit graphic
        public override Type SeedType => typeof(fudishFruitSeed);

        [Constructable]
        public fudishFruit() : base()
        {
        }

        [Constructable]
        public fudishFruit(int amount) : base(amount)
        {
        }

        public fudishFruit(Serial serial) : base(serial)
        {
        }
    }
    
	public class fudishFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a fudish fruit plant";
		public override int PlantHue => 1913;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0C97; // Harvestable plant graphic
		public override Type FruitType => typeof(fudishFruit);

		[Constructable]
		public fudishFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public fudishFruitplant(Serial serial) : base(serial)
		{
		}
	}


    public class fudishFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a fudish fruit seed";
        public override int SeedHue => 1913;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(fudishFruitplant);

        [Constructable]
        public fudishFruitSeed() : base()
        {
        }

        public fudishFruitSeed(Serial serial) : base(serial)
        {
        }
    }	
}
