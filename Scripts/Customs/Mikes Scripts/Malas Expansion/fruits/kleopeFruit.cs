using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class kleopeFruit : BaseFruit
    {
        public override string FruitName => "kleope fruit";
        public override int FruitHue => 259;
        public override int FruitGraphic => 0x0F7B; // Example fruit graphic
        public override Type SeedType => typeof(kleopeFruitSeed);

        [Constructable]
        public kleopeFruit() : base()
        {
        }

        [Constructable]
        public kleopeFruit(int amount) : base(amount)
        {
        }

        public kleopeFruit(Serial serial) : base(serial)
        {
        }
    }
    
	public class kleopeFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a kleope fruit plant";
		public override int PlantHue => 259;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0C84; // Harvestable plant graphic
		public override Type FruitType => typeof(kleopeFruit);

		[Constructable]
		public kleopeFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public kleopeFruitplant(Serial serial) : base(serial)
		{
		}
	}


    public class kleopeFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a kleope fruit seed";
        public override int SeedHue => 259;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(kleopeFruitplant);

        [Constructable]
        public kleopeFruitSeed() : base()
        {
        }

        public kleopeFruitSeed(Serial serial) : base(serial)
        {
        }
    }	
}
