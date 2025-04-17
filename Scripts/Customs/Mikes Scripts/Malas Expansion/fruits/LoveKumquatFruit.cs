using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class LoveKumquatFruit : BaseFruit
    {
        public override string FruitName => "Love Kumquat";
        public override int FruitHue => 2682;
        public override int FruitGraphic => 0x1721; // Example fruit graphic
        public override Type SeedType => typeof(LoveKumquatFruitSeed);

        [Constructable]
        public LoveKumquatFruit() : base()
        {
        }

        [Constructable]
        public LoveKumquatFruit(int amount) : base(amount)
        {
        }

        public LoveKumquatFruit(Serial serial) : base(serial)
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
    
	public class LoveKumquatFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a Love Kumquat plant";
		public override int PlantHue => 2682;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0CA4; // Harvestable plant graphic
		public override Type FruitType => typeof(LoveKumquatFruit);

		[Constructable]
		public LoveKumquatFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public LoveKumquatFruitplant(Serial serial) : base(serial)
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


    public class LoveKumquatFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a Love Kumquat seed";
        public override int SeedHue => 2682;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(LoveKumquatFruitplant);

        [Constructable]
        public LoveKumquatFruitSeed() : base()
        {
        }

        public LoveKumquatFruitSeed(Serial serial) : base(serial)
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
