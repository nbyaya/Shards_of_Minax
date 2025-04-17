using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class slirindFruit : BaseFruit
    {
        public override string FruitName => "slirind fruit";
        public override int FruitHue => 1018;
        public override int FruitGraphic => 0x09B5; // Example fruit graphic
        public override Type SeedType => typeof(slirindFruitSeed);

        [Constructable]
        public slirindFruit() : base()
        {
        }

        [Constructable]
        public slirindFruit(int amount) : base(amount)
        {
        }

        public slirindFruit(Serial serial) : base(serial)
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
    
	public class slirindFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a slirind fruit plant";
		public override int PlantHue => 1018;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0C93; // Harvestable plant graphic
		public override Type FruitType => typeof(slirindFruit);

		[Constructable]
		public slirindFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public slirindFruitplant(Serial serial) : base(serial)
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


    public class slirindFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a slirind fruit seed";
        public override int SeedHue => 1018;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(slirindFruitplant);

        [Constructable]
        public slirindFruitSeed() : base()
        {
        }

        public slirindFruitSeed(Serial serial) : base(serial)
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
