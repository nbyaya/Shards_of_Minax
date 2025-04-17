using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class wriggumondFruit : BaseFruit
    {
        public override string FruitName => "wriggumond fruit";
        public override int FruitHue => 1917;
        public override int FruitGraphic => 0x0C75; // Example fruit graphic
        public override Type SeedType => typeof(wriggumondFruitSeed);

        [Constructable]
        public wriggumondFruit() : base()
        {
        }

        [Constructable]
        public wriggumondFruit(int amount) : base(amount)
        {
        }

        public wriggumondFruit(Serial serial) : base(serial)
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
    
	public class wriggumondFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a wriggumond fruit plant";
		public override int PlantHue => 1917;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0CA1; // Harvestable plant graphic
		public override Type FruitType => typeof(wriggumondFruit);

		[Constructable]
		public wriggumondFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public wriggumondFruitplant(Serial serial) : base(serial)
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


    public class wriggumondFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a wriggumond fruit seed";
        public override int SeedHue => 1917;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(wriggumondFruitplant);

        [Constructable]
        public wriggumondFruitSeed() : base()
        {
        }

        public wriggumondFruitSeed(Serial serial) : base(serial)
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
