using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class siheonachFruit : BaseFruit
    {
        public override string FruitName => "siheonach fruit";
        public override int FruitHue => 295;
        public override int FruitGraphic => 0x0C73; // Example fruit graphic
        public override Type SeedType => typeof(siheonachFruitSeed);

        [Constructable]
        public siheonachFruit() : base()
        {
        }

        [Constructable]
        public siheonachFruit(int amount) : base(amount)
        {
        }

        public siheonachFruit(Serial serial) : base(serial)
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
    
	public class siheonachFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a siheonach fruit plant";
		public override int PlantHue => 295;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0C51; // Harvestable plant graphic
		public override Type FruitType => typeof(siheonachFruit);

		[Constructable]
		public siheonachFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public siheonachFruitplant(Serial serial) : base(serial)
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


    public class siheonachFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a siheonach fruit seed";
        public override int SeedHue => 295;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(siheonachFruitplant);

        [Constructable]
        public siheonachFruitSeed() : base()
        {
        }

        public siheonachFruitSeed(Serial serial) : base(serial)
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
