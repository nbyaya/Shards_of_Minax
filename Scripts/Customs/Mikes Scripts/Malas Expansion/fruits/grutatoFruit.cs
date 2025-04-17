using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class grutatoFruit : BaseFruit
    {
        public override string FruitName => "grutato fruit";
        public override int FruitHue => 1818;
        public override int FruitGraphic => 0x1726; // Example fruit graphic
        public override Type SeedType => typeof(grutatoFruitSeed);

        [Constructable]
        public grutatoFruit() : base()
        {
        }

        [Constructable]
        public grutatoFruit(int amount) : base(amount)
        {
        }

        public grutatoFruit(Serial serial) : base(serial)
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
    
	public class grutatoFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a grutato fruit plant";
		public override int PlantHue => 1818;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0C51; // Harvestable plant graphic
		public override Type FruitType => typeof(grutatoFruit);

		[Constructable]
		public grutatoFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public grutatoFruitplant(Serial serial) : base(serial)
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


    public class grutatoFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a grutato fruit seed";
        public override int SeedHue => 1818;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(grutatoFruitplant);

        [Constructable]
        public grutatoFruitSeed() : base()
        {
        }

        public grutatoFruitSeed(Serial serial) : base(serial)
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
