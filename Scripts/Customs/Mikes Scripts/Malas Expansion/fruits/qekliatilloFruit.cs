using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class qekliatilloFruit : BaseFruit
    {
        public override string FruitName => "qekliatillo fruit";
        public override int FruitHue => 2369;
        public override int FruitGraphic => 0x0C7F; // Example fruit graphic
        public override Type SeedType => typeof(qekliatilloFruitSeed);

        [Constructable]
        public qekliatilloFruit() : base()
        {
        }

        [Constructable]
        public qekliatilloFruit(int amount) : base(amount)
        {
        }

        public qekliatilloFruit(Serial serial) : base(serial)
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
    
	public class qekliatilloFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a qekliatillo fruit plant";
		public override int PlantHue => 2369;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0CAB; // Harvestable plant graphic
		public override Type FruitType => typeof(qekliatilloFruit);

		[Constructable]
		public qekliatilloFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public qekliatilloFruitplant(Serial serial) : base(serial)
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


    public class qekliatilloFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a qekliatillo fruit seed";
        public override int SeedHue => 2369;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(qekliatilloFruitplant);

        [Constructable]
        public qekliatilloFruitSeed() : base()
        {
        }

        public qekliatilloFruitSeed(Serial serial) : base(serial)
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
