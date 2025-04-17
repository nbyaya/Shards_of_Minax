using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class PeaceDateFruit : BaseFruit
    {
        public override string FruitName => "Peace Date";
        public override int FruitHue => 124;
        public override int FruitGraphic => 0x0C6C; // Example fruit graphic
        public override Type SeedType => typeof(PeaceDateFruitSeed);

        [Constructable]
        public PeaceDateFruit() : base()
        {
        }

        [Constructable]
        public PeaceDateFruit(int amount) : base(amount)
        {
        }

        public PeaceDateFruit(Serial serial) : base(serial)
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
    
	public class PeaceDateFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a Peace Date plant";
		public override int PlantHue => 124;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0C96; // Harvestable plant graphic
		public override Type FruitType => typeof(PeaceDateFruit);

		[Constructable]
		public PeaceDateFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public PeaceDateFruitplant(Serial serial) : base(serial)
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


    public class PeaceDateFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a Peace Date seed";
        public override int SeedHue => 124;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(PeaceDateFruitplant);

        [Constructable]
        public PeaceDateFruitSeed() : base()
        {
        }

        public PeaceDateFruitSeed(Serial serial) : base(serial)
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
