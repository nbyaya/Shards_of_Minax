using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class VoidPulasanFruit : BaseFruit
    {
        public override string FruitName => "Void Pulasan";
        public override int FruitHue => 1000;
        public override int FruitGraphic => 0x0F8D; // Example fruit graphic
        public override Type SeedType => typeof(VoidPulasanFruitSeed);

        [Constructable]
        public VoidPulasanFruit() : base()
        {
        }

        [Constructable]
        public VoidPulasanFruit(int amount) : base(amount)
        {
        }

        public VoidPulasanFruit(Serial serial) : base(serial)
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
    
	public class VoidPulasanFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a Void Pulasan plant";
		public override int PlantHue => 1000;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0D0E; // Harvestable plant graphic
		public override Type FruitType => typeof(VoidPulasanFruit);

		[Constructable]
		public VoidPulasanFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public VoidPulasanFruitplant(Serial serial) : base(serial)
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


    public class VoidPulasanFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a Void Pulasan seed";
        public override int SeedHue => 1000;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(VoidPulasanFruitplant);

        [Constructable]
        public VoidPulasanFruitSeed() : base()
        {
        }

        public VoidPulasanFruitSeed(Serial serial) : base(serial)
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
