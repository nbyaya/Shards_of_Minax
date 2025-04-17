using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class kraccaleryFruit : BaseFruit
    {
        public override string FruitName => "kraccalery fruit";
        public override int FruitHue => 2713;
        public override int FruitGraphic => 0x09D2; // Example fruit graphic
        public override Type SeedType => typeof(kraccaleryFruitSeed);

        [Constructable]
        public kraccaleryFruit() : base()
        {
        }

        [Constructable]
        public kraccaleryFruit(int amount) : base(amount)
        {
        }

        public kraccaleryFruit(Serial serial) : base(serial)
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
    
	public class kraccaleryFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a kraccalery fruit plant";
		public override int PlantHue => 2713;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0C59; // Harvestable plant graphic
		public override Type FruitType => typeof(kraccaleryFruit);

		[Constructable]
		public kraccaleryFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public kraccaleryFruitplant(Serial serial) : base(serial)
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


    public class kraccaleryFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a kraccalery fruit seed";
        public override int SeedHue => 2713;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(kraccaleryFruitplant);

        [Constructable]
        public kraccaleryFruitSeed() : base()
        {
        }

        public kraccaleryFruitSeed(Serial serial) : base(serial)
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
