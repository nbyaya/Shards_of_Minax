using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class uyerdFruit : BaseFruit
    {
        public override string FruitName => "uyerd fruit";
        public override int FruitHue => 2303;
        public override int FruitGraphic => 0x0F7A; // Example fruit graphic
        public override Type SeedType => typeof(uyerdFruitSeed);

        [Constructable]
        public uyerdFruit() : base()
        {
        }

        [Constructable]
        public uyerdFruit(int amount) : base(amount)
        {
        }

        public uyerdFruit(Serial serial) : base(serial)
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
    
	public class uyerdFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a uyerd fruit plant";
		public override int PlantHue => 2303;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0C91; // Harvestable plant graphic
		public override Type FruitType => typeof(uyerdFruit);

		[Constructable]
		public uyerdFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public uyerdFruitplant(Serial serial) : base(serial)
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


    public class uyerdFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a uyerd fruit seed";
        public override int SeedHue => 2303;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(uyerdFruitplant);

        [Constructable]
        public uyerdFruitSeed() : base()
        {
        }

        public uyerdFruitSeed(Serial serial) : base(serial)
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
