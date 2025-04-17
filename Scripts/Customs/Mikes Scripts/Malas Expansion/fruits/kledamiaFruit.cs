using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class kledamiaFruit : BaseFruit
    {
        public override string FruitName => "kledamia fruit";
        public override int FruitHue => 531;
        public override int FruitGraphic => 0x0C79; // Example fruit graphic
        public override Type SeedType => typeof(kledamiaFruitSeed);

        [Constructable]
        public kledamiaFruit() : base()
        {
        }

        [Constructable]
        public kledamiaFruit(int amount) : base(amount)
        {
        }

        public kledamiaFruit(Serial serial) : base(serial)
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
    
	public class kledamiaFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a kledamia fruit plant";
		public override int PlantHue => 531;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0CA8; // Harvestable plant graphic
		public override Type FruitType => typeof(kledamiaFruit);

		[Constructable]
		public kledamiaFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public kledamiaFruitplant(Serial serial) : base(serial)
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


    public class kledamiaFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a kledamia fruit seed";
        public override int SeedHue => 531;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(kledamiaFruitplant);

        [Constructable]
        public kledamiaFruitSeed() : base()
        {
        }

        public kledamiaFruitSeed(Serial serial) : base(serial)
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
