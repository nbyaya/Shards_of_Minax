using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class PygmyOrangeFruit : BaseFruit
    {
        public override string FruitName => "Pygmy Orange";
        public override int FruitHue => 1262;
        public override int FruitGraphic => 0x0C7B; // Example fruit graphic
        public override Type SeedType => typeof(PygmyOrangeFruitSeed);

        [Constructable]
        public PygmyOrangeFruit() : base()
        {
        }

        [Constructable]
        public PygmyOrangeFruit(int amount) : base(amount)
        {
        }

        public PygmyOrangeFruit(Serial serial) : base(serial)
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
    
	public class PygmyOrangeFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a Pygmy Orange plant";
		public override int PlantHue => 1262;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0D3F; // Harvestable plant graphic
		public override Type FruitType => typeof(PygmyOrangeFruit);

		[Constructable]
		public PygmyOrangeFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public PygmyOrangeFruitplant(Serial serial) : base(serial)
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


    public class PygmyOrangeFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a Pygmy Orange seed";
        public override int SeedHue => 1262;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(PygmyOrangeFruitplant);

        [Constructable]
        public PygmyOrangeFruitSeed() : base()
        {
        }

        public PygmyOrangeFruitSeed(Serial serial) : base(serial)
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
