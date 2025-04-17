using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class NativeRambutanFruit : BaseFruit
    {
        public override string FruitName => "Native Rambutan";
        public override int FruitHue => 103;
        public override int FruitGraphic => 0x0F84; // Example fruit graphic
        public override Type SeedType => typeof(NativeRambutanFruitSeed);

        [Constructable]
        public NativeRambutanFruit() : base()
        {
        }

        [Constructable]
        public NativeRambutanFruit(int amount) : base(amount)
        {
        }

        public NativeRambutanFruit(Serial serial) : base(serial)
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
    
	public class NativeRambutanFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a Native Rambutan plant";
		public override int PlantHue => 103;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0CBB; // Harvestable plant graphic
		public override Type FruitType => typeof(NativeRambutanFruit);

		[Constructable]
		public NativeRambutanFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public NativeRambutanFruitplant(Serial serial) : base(serial)
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


    public class NativeRambutanFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a Native Rambutan seed";
        public override int SeedHue => 103;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(NativeRambutanFruitplant);

        [Constructable]
        public NativeRambutanFruitSeed() : base()
        {
        }

        public NativeRambutanFruitSeed(Serial serial) : base(serial)
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
