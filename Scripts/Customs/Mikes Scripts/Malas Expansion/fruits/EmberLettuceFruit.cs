using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class EmberLettuceFruit : BaseFruit
    {
        public override string FruitName => "Ember Lettuce";
        public override int FruitHue => 2788;
        public override int FruitGraphic => 0x0C6A; // Example fruit graphic
        public override Type SeedType => typeof(EmberLettuceFruitSeed);

        [Constructable]
        public EmberLettuceFruit() : base()
        {
        }

        [Constructable]
        public EmberLettuceFruit(int amount) : base(amount)
        {
        }

        public EmberLettuceFruit(Serial serial) : base(serial)
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
    
	public class EmberLettuceFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a Ember Lettuce plant";
		public override int PlantHue => 2788;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0C94; // Harvestable plant graphic
		public override Type FruitType => typeof(EmberLettuceFruit);

		[Constructable]
		public EmberLettuceFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public EmberLettuceFruitplant(Serial serial) : base(serial)
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


    public class EmberLettuceFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a Ember Lettuce seed";
        public override int SeedHue => 2788;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(EmberLettuceFruitplant);

        [Constructable]
        public EmberLettuceFruitSeed() : base()
        {
        }

        public EmberLettuceFruitSeed(Serial serial) : base(serial)
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
