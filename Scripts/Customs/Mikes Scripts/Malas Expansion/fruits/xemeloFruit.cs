using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class xemeloFruit : BaseFruit
    {
        public override string FruitName => "xemelo fruit";
        public override int FruitHue => 1364;
        public override int FruitGraphic => 0x172D; // Example fruit graphic
        public override Type SeedType => typeof(xemeloFruitSeed);

        [Constructable]
        public xemeloFruit() : base()
        {
        }

        [Constructable]
        public xemeloFruit(int amount) : base(amount)
        {
        }

        public xemeloFruit(Serial serial) : base(serial)
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
    
	public class xemeloFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a xemelo fruit plant";
		public override int PlantHue => 1364;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0C9D; // Harvestable plant graphic
		public override Type FruitType => typeof(xemeloFruit);

		[Constructable]
		public xemeloFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public xemeloFruitplant(Serial serial) : base(serial)
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


    public class xemeloFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a xemelo fruit seed";
        public override int SeedHue => 1364;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(xemeloFruitplant);

        [Constructable]
        public xemeloFruitSeed() : base()
        {
        }

        public xemeloFruitSeed(Serial serial) : base(serial)
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
