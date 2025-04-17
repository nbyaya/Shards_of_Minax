using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class MellowGourdFruit : BaseFruit
    {
        public override string FruitName => "Mellow Gourd";
        public override int FruitHue => 246;
        public override int FruitGraphic => 0x0F7C; // Example fruit graphic
        public override Type SeedType => typeof(MellowGourdFruitSeed);

        [Constructable]
        public MellowGourdFruit() : base()
        {
        }

        [Constructable]
        public MellowGourdFruit(int amount) : base(amount)
        {
        }

        public MellowGourdFruit(Serial serial) : base(serial)
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
    
	public class MellowGourdFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a Mellow Gourd plant";
		public override int PlantHue => 246;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0C89; // Harvestable plant graphic
		public override Type FruitType => typeof(MellowGourdFruit);

		[Constructable]
		public MellowGourdFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public MellowGourdFruitplant(Serial serial) : base(serial)
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


    public class MellowGourdFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a Mellow Gourd seed";
        public override int SeedHue => 246;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(MellowGourdFruitplant);

        [Constructable]
        public MellowGourdFruitSeed() : base()
        {
        }

        public MellowGourdFruitSeed(Serial serial) : base(serial)
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
