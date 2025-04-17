using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class MysteryFruit : BaseFruit
    {
        public override string FruitName => "Mystery fruit";
        public override int FruitHue => 916;
        public override int FruitGraphic => 0x0C74; // Example fruit graphic
        public override Type SeedType => typeof(MysteryFruitSeed);

        [Constructable]
        public MysteryFruit() : base()
        {
        }

        [Constructable]
        public MysteryFruit(int amount) : base(amount)
        {
        }

        public MysteryFruit(Serial serial) : base(serial)
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
    
	public class MysteryFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a Mystery fruit plant";
		public override int PlantHue => 916;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0C99; // Harvestable plant graphic
		public override Type FruitType => typeof(MysteryFruit);

		[Constructable]
		public MysteryFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public MysteryFruitplant(Serial serial) : base(serial)
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


    public class MysteryFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a Mystery fruit seed";
        public override int SeedHue => 916;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(MysteryFruitplant);

        [Constructable]
        public MysteryFruitSeed() : base()
        {
        }

        public MysteryFruitSeed(Serial serial) : base(serial)
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
