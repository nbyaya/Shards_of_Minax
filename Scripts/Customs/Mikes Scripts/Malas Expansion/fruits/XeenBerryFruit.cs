using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class XeenBerryFruit : BaseFruit
    {
        public override string FruitName => "Xeen Berry";
        public override int FruitHue => 1585;
        public override int FruitGraphic => 0x0F85; // Example fruit graphic
        public override Type SeedType => typeof(XeenBerryFruitSeed);

        [Constructable]
        public XeenBerryFruit() : base()
        {
        }

        [Constructable]
        public XeenBerryFruit(int amount) : base(amount)
        {
        }

        public XeenBerryFruit(Serial serial) : base(serial)
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
    
	public class XeenBerryFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a Xeen Berry plant";
		public override int PlantHue => 1585;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0CA0; // Harvestable plant graphic
		public override Type FruitType => typeof(XeenBerryFruit);

		[Constructable]
		public XeenBerryFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public XeenBerryFruitplant(Serial serial) : base(serial)
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


    public class XeenBerryFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a Xeen Berry seed";
        public override int SeedHue => 1585;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(XeenBerryFruitplant);

        [Constructable]
        public XeenBerryFruitSeed() : base()
        {
        }

        public XeenBerryFruitSeed(Serial serial) : base(serial)
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
