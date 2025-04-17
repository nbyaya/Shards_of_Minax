using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class satilFruit : BaseFruit
    {
        public override string FruitName => "satil fruit";
        public override int FruitHue => 2570;
        public override int FruitGraphic => 0x172A; // Example fruit graphic
        public override Type SeedType => typeof(satilFruitSeed);

        [Constructable]
        public satilFruit() : base()
        {
        }

        [Constructable]
        public satilFruit(int amount) : base(amount)
        {
        }

        public satilFruit(Serial serial) : base(serial)
        {
        }
		
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Versioning for future changes, start with version 0.
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt(); // Read the version number, used for future migrations.
        }		
    }
    
	public class satilFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a satil fruit plant";
		public override int PlantHue => 2570;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0C85; // Harvestable plant graphic
		public override Type FruitType => typeof(satilFruit);

		[Constructable]
		public satilFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public satilFruitplant(Serial serial) : base(serial)
		{
		}
		
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Versioning for future changes, start with version 0.
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt(); // Read the version number, used for future migrations.
        }		
	}


    public class satilFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a satil fruit seed";
        public override int SeedHue => 2570;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(satilFruitplant);

        [Constructable]
        public satilFruitSeed() : base()
        {
        }

        public satilFruitSeed(Serial serial) : base(serial)
        {
        }
		
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Versioning for future changes, start with version 0.
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt(); // Read the version number, used for future migrations.
        }		
    }	
}
