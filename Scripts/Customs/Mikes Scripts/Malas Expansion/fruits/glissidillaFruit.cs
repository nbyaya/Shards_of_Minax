using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class glissidillaFruit : BaseFruit
    {
        public override string FruitName => "glissidilla fruit";
        public override int FruitHue => 491;
        public override int FruitGraphic => 0x0C6E; // Example fruit graphic
        public override Type SeedType => typeof(glissidillaFruitSeed);

        [Constructable]
        public glissidillaFruit() : base()
        {
        }

        [Constructable]
        public glissidillaFruit(int amount) : base(amount)
        {
        }

        public glissidillaFruit(Serial serial) : base(serial)
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
    
	public class glissidillaFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a glissidilla fruit plant";
		public override int PlantHue => 491;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0CA0; // Harvestable plant graphic
		public override Type FruitType => typeof(glissidillaFruit);

		[Constructable]
		public glissidillaFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public glissidillaFruitplant(Serial serial) : base(serial)
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


    public class glissidillaFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a glissidilla fruit seed";
        public override int SeedHue => 491;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(glissidillaFruitplant);

        [Constructable]
        public glissidillaFruitSeed() : base()
        {
        }

        public glissidillaFruitSeed(Serial serial) : base(serial)
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
