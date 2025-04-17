using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class xekraFruit : BaseFruit
    {
        public override string FruitName => "xekra fruit";
        public override int FruitHue => 1101;
        public override int FruitGraphic => 0x1728; // Example fruit graphic
        public override Type SeedType => typeof(xekraFruitSeed);

        [Constructable]
        public xekraFruit() : base()
        {
        }

        [Constructable]
        public xekraFruit(int amount) : base(amount)
        {
        }

        public xekraFruit(Serial serial) : base(serial)
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
    
	public class xekraFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a xekra fruit plant";
		public override int PlantHue => 1101;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0CA7; // Harvestable plant graphic
		public override Type FruitType => typeof(xekraFruit);

		[Constructable]
		public xekraFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public xekraFruitplant(Serial serial) : base(serial)
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


    public class xekraFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a xekra fruit seed";
        public override int SeedHue => 1101;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(xekraFruitplant);

        [Constructable]
        public xekraFruitSeed() : base()
        {
        }

        public xekraFruitSeed(Serial serial) : base(serial)
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
