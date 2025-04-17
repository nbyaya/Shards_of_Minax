using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class RainPommeracFruit : BaseFruit
    {
        public override string FruitName => "Rain Pommerac";
        public override int FruitHue => 1233;
        public override int FruitGraphic => 0x0F7B; // Example fruit graphic
        public override Type SeedType => typeof(RainPommeracFruitSeed);

        [Constructable]
        public RainPommeracFruit() : base()
        {
        }

        [Constructable]
        public RainPommeracFruit(int amount) : base(amount)
        {
        }

        public RainPommeracFruit(Serial serial) : base(serial)
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
    
	public class RainPommeracFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a Rain Pommerac plant";
		public override int PlantHue => 1233;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0C95; // Harvestable plant graphic
		public override Type FruitType => typeof(RainPommeracFruit);

		[Constructable]
		public RainPommeracFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public RainPommeracFruitplant(Serial serial) : base(serial)
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


    public class RainPommeracFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a Rain Pommerac seed";
        public override int SeedHue => 1233;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(RainPommeracFruitplant);

        [Constructable]
        public RainPommeracFruitSeed() : base()
        {
        }

        public RainPommeracFruitSeed(Serial serial) : base(serial)
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
