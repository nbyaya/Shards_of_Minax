using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class EmberLaurelFruit : BaseFruit
    {
        public override string FruitName => "Ember Laurel";
        public override int FruitHue => 2584;
        public override int FruitGraphic => 0x0C75; // Example fruit graphic
        public override Type SeedType => typeof(EmberLaurelFruitSeed);

        [Constructable]
        public EmberLaurelFruit() : base()
        {
        }

        [Constructable]
        public EmberLaurelFruit(int amount) : base(amount)
        {
        }

        public EmberLaurelFruit(Serial serial) : base(serial)
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
    
	public class EmberLaurelFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a Ember Laurel plant";
		public override int PlantHue => 2584;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0CC8; // Harvestable plant graphic
		public override Type FruitType => typeof(EmberLaurelFruit);

		[Constructable]
		public EmberLaurelFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public EmberLaurelFruitplant(Serial serial) : base(serial)
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


    public class EmberLaurelFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a Ember Laurel seed";
        public override int SeedHue => 2584;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(EmberLaurelFruitplant);

        [Constructable]
        public EmberLaurelFruitSeed() : base()
        {
        }

        public EmberLaurelFruitSeed(Serial serial) : base(serial)
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
