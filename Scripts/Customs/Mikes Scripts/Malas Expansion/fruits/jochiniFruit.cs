using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class jochiniFruit : BaseFruit
    {
        public override string FruitName => "jochini fruit";
        public override int FruitHue => 700;
        public override int FruitGraphic => 0x0F91; // Example fruit graphic
        public override Type SeedType => typeof(jochiniFruitSeed);

        [Constructable]
        public jochiniFruit() : base()
        {
        }

        [Constructable]
        public jochiniFruit(int amount) : base(amount)
        {
        }

        public jochiniFruit(Serial serial) : base(serial)
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
    
	public class jochiniFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a jochini fruit plant";
		public override int PlantHue => 700;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0C92; // Harvestable plant graphic
		public override Type FruitType => typeof(jochiniFruit);

		[Constructable]
		public jochiniFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public jochiniFruitplant(Serial serial) : base(serial)
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


    public class jochiniFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a jochini fruit seed";
        public override int SeedHue => 700;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(jochiniFruitplant);

        [Constructable]
        public jochiniFruitSeed() : base()
        {
        }

        public jochiniFruitSeed(Serial serial) : base(serial)
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
