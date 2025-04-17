using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class EasternBacuriFruit : BaseFruit
    {
        public override string FruitName => "Eastern Bacuri";
        public override int FruitHue => 2032;
        public override int FruitGraphic => 0x0F88; // Example fruit graphic
        public override Type SeedType => typeof(EasternBacuriFruitSeed);

        [Constructable]
        public EasternBacuriFruit() : base()
        {
        }

        [Constructable]
        public EasternBacuriFruit(int amount) : base(amount)
        {
        }

        public EasternBacuriFruit(Serial serial) : base(serial)
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
    
	public class EasternBacuriFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a Eastern Bacuri plant";
		public override int PlantHue => 2032;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0CCD; // Harvestable plant graphic
		public override Type FruitType => typeof(EasternBacuriFruit);

		[Constructable]
		public EasternBacuriFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public EasternBacuriFruitplant(Serial serial) : base(serial)
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


    public class EasternBacuriFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a EasternBacuri fruit seed";
        public override int SeedHue => 2032;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(EasternBacuriFruitplant);

        [Constructable]
        public EasternBacuriFruitSeed() : base()
        {
        }

        public EasternBacuriFruitSeed(Serial serial) : base(serial)
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
