using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class LoveZucchiniFruit : BaseFruit
    {
        public override string FruitName => "Love Zucchini";
        public override int FruitHue => 1027;
        public override int FruitGraphic => 0x0C7F; // Example fruit graphic
        public override Type SeedType => typeof(LoveZucchiniFruitSeed);

        [Constructable]
        public LoveZucchiniFruit() : base()
        {
        }

        [Constructable]
        public LoveZucchiniFruit(int amount) : base(amount)
        {
        }

        public LoveZucchiniFruit(Serial serial) : base(serial)
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
    
	public class LoveZucchiniFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a Love Zucchini plant";
		public override int PlantHue => 1027;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0C8C; // Harvestable plant graphic
		public override Type FruitType => typeof(LoveZucchiniFruit);

		[Constructable]
		public LoveZucchiniFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public LoveZucchiniFruitplant(Serial serial) : base(serial)
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


    public class LoveZucchiniFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a Love Zucchini seed";
        public override int SeedHue => 1027;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(LoveZucchiniFruitplant);

        [Constructable]
        public LoveZucchiniFruitSeed() : base()
        {
        }

        public LoveZucchiniFruitSeed(Serial serial) : base(serial)
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
