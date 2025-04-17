using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class probbacheeFruit : BaseFruit
    {
        public override string FruitName => "probbachee fruit";
        public override int FruitHue => 835;
        public override int FruitGraphic => 0x0F7A; // Example fruit graphic
        public override Type SeedType => typeof(probbacheeFruitSeed);

        [Constructable]
        public probbacheeFruit() : base()
        {
        }

        [Constructable]
        public probbacheeFruit(int amount) : base(amount)
        {
        }

        public probbacheeFruit(Serial serial) : base(serial)
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
    
	public class probbacheeFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a probbachee fruit plant";
		public override int PlantHue => 835;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0CAB; // Harvestable plant graphic
		public override Type FruitType => typeof(probbacheeFruit);

		[Constructable]
		public probbacheeFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public probbacheeFruitplant(Serial serial) : base(serial)
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


    public class probbacheeFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a probbachee fruit seed";
        public override int SeedHue => 835;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(probbacheeFruitplant);

        [Constructable]
        public probbacheeFruitSeed() : base()
        {
        }

        public probbacheeFruitSeed(Serial serial) : base(serial)
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
