using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class PeaceAmaranthFruit : BaseFruit
    {
        public override string FruitName => "Peace Amaranth";
        public override int FruitHue => 978;
        public override int FruitGraphic => 0x0F8F; // Example fruit graphic
        public override Type SeedType => typeof(PeaceAmaranthFruitSeed);

        [Constructable]
        public PeaceAmaranthFruit() : base()
        {
        }

        [Constructable]
        public PeaceAmaranthFruit(int amount) : base(amount)
        {
        }

        public PeaceAmaranthFruit(Serial serial) : base(serial)
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
    
	public class PeaceAmaranthFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a Peace Amaranth plant";
		public override int PlantHue => 978;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0D3F; // Harvestable plant graphic
		public override Type FruitType => typeof(PeaceAmaranthFruit);

		[Constructable]
		public PeaceAmaranthFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public PeaceAmaranthFruitplant(Serial serial) : base(serial)
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


    public class PeaceAmaranthFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a Peace Amaranth seed";
        public override int SeedHue => 978;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(PeaceAmaranthFruitplant);

        [Constructable]
        public PeaceAmaranthFruitSeed() : base()
        {
        }

        public PeaceAmaranthFruitSeed(Serial serial) : base(serial)
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
