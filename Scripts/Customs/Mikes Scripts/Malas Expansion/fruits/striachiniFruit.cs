using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class striachiniFruit : BaseFruit
    {
        public override string FruitName => "striachini fruit";
        public override int FruitHue => 2624;
        public override int FruitGraphic => 0x0C6C; // Example fruit graphic
        public override Type SeedType => typeof(striachiniFruitSeed);

        [Constructable]
        public striachiniFruit() : base()
        {
        }

        [Constructable]
        public striachiniFruit(int amount) : base(amount)
        {
        }

        public striachiniFruit(Serial serial) : base(serial)
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
    
	public class striachiniFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a striachini fruit plant";
		public override int PlantHue => 2624;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0CB7; // Harvestable plant graphic
		public override Type FruitType => typeof(striachiniFruit);

		[Constructable]
		public striachiniFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public striachiniFruitplant(Serial serial) : base(serial)
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


    public class striachiniFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a striachini fruit seed";
        public override int SeedHue => 2624;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(striachiniFruitplant);

        [Constructable]
        public striachiniFruitSeed() : base()
        {
        }

        public striachiniFruitSeed(Serial serial) : base(serial)
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
