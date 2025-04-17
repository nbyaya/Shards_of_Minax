using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class veapeFruit : BaseFruit
    {
        public override string FruitName => "veape fruit";
        public override int FruitHue => 65;
        public override int FruitGraphic => 0x0C7B; // Example fruit graphic
        public override Type SeedType => typeof(veapeFruitSeed);

        [Constructable]
        public veapeFruit() : base()
        {
        }

        [Constructable]
        public veapeFruit(int amount) : base(amount)
        {
        }

        public veapeFruit(Serial serial) : base(serial)
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
    
	public class veapeFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a veape fruit plant";
		public override int PlantHue => 65;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0CC9; // Harvestable plant graphic
		public override Type FruitType => typeof(veapeFruit);

		[Constructable]
		public veapeFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public veapeFruitplant(Serial serial) : base(serial)
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


    public class veapeFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a veape fruit seed";
        public override int SeedHue => 65;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(veapeFruitplant);

        [Constructable]
        public veapeFruitSeed() : base()
        {
        }

        public veapeFruitSeed(Serial serial) : base(serial)
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
