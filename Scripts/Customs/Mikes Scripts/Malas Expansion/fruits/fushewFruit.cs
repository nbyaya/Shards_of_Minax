using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class fushewFruit : BaseFruit
    {
        public override string FruitName => "fushew fruit";
        public override int FruitHue => 1598;
        public override int FruitGraphic => 0x0C64; // Example fruit graphic
        public override Type SeedType => typeof(fushewFruitSeed);

        [Constructable]
        public fushewFruit() : base()
        {
        }

        [Constructable]
        public fushewFruit(int amount) : base(amount)
        {
        }

        public fushewFruit(Serial serial) : base(serial)
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
    
	public class fushewFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a fushew fruit plant";
		public override int PlantHue => 1598;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0CC1; // Harvestable plant graphic
		public override Type FruitType => typeof(fushewFruit);

		[Constructable]
		public fushewFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public fushewFruitplant(Serial serial) : base(serial)
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


    public class fushewFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a fushew fruit seed";
        public override int SeedHue => 1598;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(fushewFruitplant);

        [Constructable]
        public fushewFruitSeed() : base()
        {
        }

        public fushewFruitSeed(Serial serial) : base(serial)
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
