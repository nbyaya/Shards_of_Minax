using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class HairyTomatoFruit : BaseFruit
    {
        public override string FruitName => "Hairy Tomato";
        public override int FruitHue => 2313;
        public override int FruitGraphic => 0x0C81; // Example fruit graphic
        public override Type SeedType => typeof(HairyTomatoFruitSeed);

        [Constructable]
        public HairyTomatoFruit() : base()
        {
        }

        [Constructable]
        public HairyTomatoFruit(int amount) : base(amount)
        {
        }

        public HairyTomatoFruit(Serial serial) : base(serial)
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
    
	public class HairyTomatoFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a Hairy Tomato plant";
		public override int PlantHue => 2313;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0C99; // Harvestable plant graphic
		public override Type FruitType => typeof(HairyTomatoFruit);

		[Constructable]
		public HairyTomatoFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public HairyTomatoFruitplant(Serial serial) : base(serial)
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


    public class HairyTomatoFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a Hairy Tomato seed";
        public override int SeedHue => 2313;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(HairyTomatoFruitplant);

        [Constructable]
        public HairyTomatoFruitSeed() : base()
        {
        }

        public HairyTomatoFruitSeed(Serial serial) : base(serial)
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
