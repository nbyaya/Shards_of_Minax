using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class GroundPearFruit : BaseFruit
    {
        public override string FruitName => "GroundPear fruit";
        public override int FruitHue => 2681;
        public override int FruitGraphic => 0x172C; // Example fruit graphic
        public override Type SeedType => typeof(GroundPearFruitSeed);

        [Constructable]
        public GroundPearFruit() : base()
        {
        }

        [Constructable]
        public GroundPearFruit(int amount) : base(amount)
        {
        }

        public GroundPearFruit(Serial serial) : base(serial)
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
    
	public class GroundPearFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a GroundPear fruit plant";
		public override int PlantHue => 2681;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0C96; // Harvestable plant graphic
		public override Type FruitType => typeof(GroundPearFruit);

		[Constructable]
		public GroundPearFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public GroundPearFruitplant(Serial serial) : base(serial)
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


    public class GroundPearFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a GroundPear fruit seed";
        public override int SeedHue => 2681;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(GroundPearFruitplant);

        [Constructable]
        public GroundPearFruitSeed() : base()
        {
        }

        public GroundPearFruitSeed(Serial serial) : base(serial)
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
