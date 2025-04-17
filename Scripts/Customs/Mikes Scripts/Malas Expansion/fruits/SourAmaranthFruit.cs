using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class SourAmaranthFruit : BaseFruit
    {
        public override string FruitName => "Sour Amaranth";
        public override int FruitHue => 1343;
        public override int FruitGraphic => 0x0C6C; // Example fruit graphic
        public override Type SeedType => typeof(SourAmaranthFruitSeed);

        [Constructable]
        public SourAmaranthFruit() : base()
        {
        }

        [Constructable]
        public SourAmaranthFruit(int amount) : base(amount)
        {
        }

        public SourAmaranthFruit(Serial serial) : base(serial)
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
    
	public class SourAmaranthFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a Sour Amaranth plant";
		public override int PlantHue => 1343;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0C4F; // Harvestable plant graphic
		public override Type FruitType => typeof(SourAmaranthFruit);

		[Constructable]
		public SourAmaranthFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public SourAmaranthFruitplant(Serial serial) : base(serial)
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


    public class SourAmaranthFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a Sour Amaranth seed";
        public override int SeedHue => 1343;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(SourAmaranthFruitplant);

        [Constructable]
        public SourAmaranthFruitSeed() : base()
        {
        }

        public SourAmaranthFruitSeed(Serial serial) : base(serial)
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
