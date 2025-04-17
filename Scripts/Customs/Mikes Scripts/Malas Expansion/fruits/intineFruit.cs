using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class intineFruit : BaseFruit
    {
        public override string FruitName => "intine fruit";
        public override int FruitHue => 2043;
        public override int FruitGraphic => 0x0C7B; // Example fruit graphic
        public override Type SeedType => typeof(intineFruitSeed);

        [Constructable]
        public intineFruit() : base()
        {
        }

        [Constructable]
        public intineFruit(int amount) : base(amount)
        {
        }

        public intineFruit(Serial serial) : base(serial)
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
    
	public class intineFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a intine fruit plant";
		public override int PlantHue => 2043;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0C9F; // Harvestable plant graphic
		public override Type FruitType => typeof(intineFruit);

		[Constructable]
		public intineFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public intineFruitplant(Serial serial) : base(serial)
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


    public class intineFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a intine fruit seed";
        public override int SeedHue => 2043;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(intineFruitplant);

        [Constructable]
        public intineFruitSeed() : base()
        {
        }

        public intineFruitSeed(Serial serial) : base(serial)
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
