using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class MutantLemonFruit : BaseFruit
    {
        public override string FruitName => "Mutant Lemon";
        public override int FruitHue => 1295;
        public override int FruitGraphic => 0x1728; // Example fruit graphic
        public override Type SeedType => typeof(MutantLemonFruitSeed);

        [Constructable]
        public MutantLemonFruit() : base()
        {
        }

        [Constructable]
        public MutantLemonFruit(int amount) : base(amount)
        {
        }

        public MutantLemonFruit(Serial serial) : base(serial)
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
    
	public class MutantLemonFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a Mutant Lemon plant";
		public override int PlantHue => 1295;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0C86; // Harvestable plant graphic
		public override Type FruitType => typeof(MutantLemonFruit);

		[Constructable]
		public MutantLemonFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public MutantLemonFruitplant(Serial serial) : base(serial)
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


    public class MutantLemonFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a Mutant Lemon seed";
        public override int SeedHue => 1295;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(MutantLemonFruitplant);

        [Constructable]
        public MutantLemonFruitSeed() : base()
        {
        }

        public MutantLemonFruitSeed(Serial serial) : base(serial)
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
