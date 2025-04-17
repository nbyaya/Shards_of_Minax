using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class omondFruit : BaseFruit
    {
        public override string FruitName => "omond fruit";
        public override int FruitHue => 2385;
        public override int FruitGraphic => 0x0C79; // Example fruit graphic
        public override Type SeedType => typeof(omondFruitSeed);

        [Constructable]
        public omondFruit() : base()
        {
        }

        [Constructable]
        public omondFruit(int amount) : base(amount)
        {
        }

        public omondFruit(Serial serial) : base(serial)
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
    
	public class omondFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a omond fruit plant";
		public override int PlantHue => 2385;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0C57; // Harvestable plant graphic
		public override Type FruitType => typeof(omondFruit);

		[Constructable]
		public omondFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public omondFruitplant(Serial serial) : base(serial)
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


    public class omondFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a omond fruit seed";
        public override int SeedHue => 2385;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(omondFruitplant);

        [Constructable]
        public omondFruitSeed() : base()
        {
        }

        public omondFruitSeed(Serial serial) : base(serial)
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
