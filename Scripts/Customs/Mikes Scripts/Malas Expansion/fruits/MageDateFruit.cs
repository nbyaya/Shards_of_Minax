using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class MageDateFruit : BaseFruit
    {
        public override string FruitName => "Mage Date";
        public override int FruitHue => 2617;
        public override int FruitGraphic => 0x172A; // Example fruit graphic
        public override Type SeedType => typeof(MageDateFruitSeed);

        [Constructable]
        public MageDateFruit() : base()
        {
        }

        [Constructable]
        public MageDateFruit(int amount) : base(amount)
        {
        }

        public MageDateFruit(Serial serial) : base(serial)
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
    
	public class MageDateFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a Mage Date plant";
		public override int PlantHue => 2617;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0CA3; // Harvestable plant graphic
		public override Type FruitType => typeof(MageDateFruit);

		[Constructable]
		public MageDateFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public MageDateFruitplant(Serial serial) : base(serial)
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


    public class MageDateFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a MageDate seed";
        public override int SeedHue => 2617;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(MageDateFruitplant);

        [Constructable]
        public MageDateFruitSeed() : base()
        {
        }

        public MageDateFruitSeed(Serial serial) : base(serial)
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
