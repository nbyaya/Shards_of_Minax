using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class SweetBoquilaFruit : BaseFruit
    {
        public override string FruitName => "Sweet Boquila";
        public override int FruitHue => 2401;
        public override int FruitGraphic => 0x0C70; // Example fruit graphic
        public override Type SeedType => typeof(SweetBoquilaFruitSeed);

        [Constructable]
        public SweetBoquilaFruit() : base()
        {
        }

        [Constructable]
        public SweetBoquilaFruit(int amount) : base(amount)
        {
        }

        public SweetBoquilaFruit(Serial serial) : base(serial)
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
    
	public class SweetBoquilaFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a Sweet Boquila plant";
		public override int PlantHue => 2401;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0C9E; // Harvestable plant graphic
		public override Type FruitType => typeof(SweetBoquilaFruit);

		[Constructable]
		public SweetBoquilaFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public SweetBoquilaFruitplant(Serial serial) : base(serial)
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


    public class SweetBoquilaFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a Sweet Boquila seed";
        public override int SeedHue => 2401;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(SweetBoquilaFruitplant);

        [Constructable]
        public SweetBoquilaFruitSeed() : base()
        {
        }

        public SweetBoquilaFruitSeed(Serial serial) : base(serial)
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
