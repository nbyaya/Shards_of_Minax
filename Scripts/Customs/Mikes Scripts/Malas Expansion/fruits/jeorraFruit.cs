using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class jeorraFruit : BaseFruit
    {
        public override string FruitName => "jeorra fruit";
        public override int FruitHue => 1565;
        public override int FruitGraphic => 0x1724; // Example fruit graphic
        public override Type SeedType => typeof(jeorraFruitSeed);

        [Constructable]
        public jeorraFruit() : base()
        {
        }

        [Constructable]
        public jeorraFruit(int amount) : base(amount)
        {
        }

        public jeorraFruit(Serial serial) : base(serial)
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
    
	public class jeorraFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a jeorra fruit plant";
		public override int PlantHue => 1565;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0C98; // Harvestable plant graphic
		public override Type FruitType => typeof(jeorraFruit);

		[Constructable]
		public jeorraFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public jeorraFruitplant(Serial serial) : base(serial)
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


    public class jeorraFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a jeorra fruit seed";
        public override int SeedHue => 1565;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(jeorraFruitplant);

        [Constructable]
        public jeorraFruitSeed() : base()
        {
        }

        public jeorraFruitSeed(Serial serial) : base(serial)
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
