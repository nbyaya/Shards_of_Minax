using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class DessertBroccoliFruit : BaseFruit
    {
        public override string FruitName => "Dessert Broccoli";
        public override int FruitHue => 1463;
        public override int FruitGraphic => 0x172A; // Example fruit graphic
        public override Type SeedType => typeof(DessertBroccoliFruitSeed);

        [Constructable]
        public DessertBroccoliFruit() : base()
        {
        }

        [Constructable]
        public DessertBroccoliFruit(int amount) : base(amount)
        {
        }

        public DessertBroccoliFruit(Serial serial) : base(serial)
        {
        }
    }
    
	public class DessertBroccoliFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a Dessert Broccoli plant";
		public override int PlantHue => 1463;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0C8A; // Harvestable plant graphic
		public override Type FruitType => typeof(DessertBroccoliFruit);

		[Constructable]
		public DessertBroccoliFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public DessertBroccoliFruitplant(Serial serial) : base(serial)
		{
		}
	}


    public class DessertBroccoliFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a Dessert Broccoli seed";
        public override int SeedHue => 1463;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(DessertBroccoliFruitplant);

        [Constructable]
        public DessertBroccoliFruitSeed() : base()
        {
        }

        public DessertBroccoliFruitSeed(Serial serial) : base(serial)
        {
        }
    }	
}
