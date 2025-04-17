using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class gropioveFruit : BaseFruit
    {
        public override string FruitName => "gropiove fruit";
        public override int FruitHue => 1500;
        public override int FruitGraphic => 0x0F83; // Example fruit graphic
        public override Type SeedType => typeof(gropioveFruitSeed);

        [Constructable]
        public gropioveFruit() : base()
        {
        }

        [Constructable]
        public gropioveFruit(int amount) : base(amount)
        {
        }

        public gropioveFruit(Serial serial) : base(serial)
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
    
	public class gropioveFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a gropiove fruit plant";
		public override int PlantHue => 1500;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0C86; // Harvestable plant graphic
		public override Type FruitType => typeof(gropioveFruit);

		[Constructable]
		public gropioveFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public gropioveFruitplant(Serial serial) : base(serial)
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


    public class gropioveFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a gropiove fruit seed";
        public override int SeedHue => 1500;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(gropioveFruitplant);

        [Constructable]
        public gropioveFruitSeed() : base()
        {
        }

        public gropioveFruitSeed(Serial serial) : base(serial)
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
