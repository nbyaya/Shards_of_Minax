using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class agleatainFruit : BaseFruit
    {
        public override string FruitName => "agleatain fruit";
        public override int FruitHue => 2470;
        public override int FruitGraphic => 0x26B7; // Example fruit graphic
        public override Type SeedType => typeof(agleatainFruitSeed);

        [Constructable]
        public agleatainFruit() : base()
        {
        }

        [Constructable]
        public agleatainFruit(int amount) : base(amount)
        {
        }

        public agleatainFruit(Serial serial) : base(serial)
        {
        }
    }
    
	public class agleatainFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a agleatain fruit plant";
		public override int PlantHue => 2470;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0CC4; // Harvestable plant graphic
		public override Type FruitType => typeof(agleatainFruit);

		[Constructable]
		public agleatainFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public agleatainFruitplant(Serial serial) : base(serial)
		{
		}
	}


    public class agleatainFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a agleatain fruit seed";
        public override int SeedHue => 2470;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(agleatainFruitplant);

        [Constructable]
        public agleatainFruitSeed() : base()
        {
        }

        public agleatainFruitSeed(Serial serial) : base(serial)
        {
        }
    }	
}
