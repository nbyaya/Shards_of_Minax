using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class CliffNectarineFruit : BaseFruit
    {
        public override string FruitName => "Cliff Nectarine";
        public override int FruitHue => 1987;
        public override int FruitGraphic => 0x09B5; // Example fruit graphic
        public override Type SeedType => typeof(CliffNectarineFruitSeed);

        [Constructable]
        public CliffNectarineFruit() : base()
        {
        }

        [Constructable]
        public CliffNectarineFruit(int amount) : base(amount)
        {
        }

        public CliffNectarineFruit(Serial serial) : base(serial)
        {
        }
    }
    
	public class CliffNectarineFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a Cliff Nectarine plant";
		public override int PlantHue => 1987;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0C9C; // Harvestable plant graphic
		public override Type FruitType => typeof(CliffNectarineFruit);

		[Constructable]
		public CliffNectarineFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public CliffNectarineFruitplant(Serial serial) : base(serial)
		{
		}
	}


    public class CliffNectarineFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a Cliff Nectarine seed";
        public override int SeedHue => 1987;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(CliffNectarineFruitplant);

        [Constructable]
        public CliffNectarineFruitSeed() : base()
        {
        }

        public CliffNectarineFruitSeed(Serial serial) : base(serial)
        {
        }
    }	
}
