using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class PeaceNectarineFruit : BaseFruit
    {
        public override string FruitName => "Peace Nectarine";
        public override int FruitHue => 1147;
        public override int FruitGraphic => 0x0C77; // Example fruit graphic
        public override Type SeedType => typeof(PeaceNectarineFruitSeed);

        [Constructable]
        public PeaceNectarineFruit() : base()
        {
        }

        [Constructable]
        public PeaceNectarineFruit(int amount) : base(amount)
        {
        }

        public PeaceNectarineFruit(Serial serial) : base(serial)
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
    
	public class PeaceNectarineFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a Peace Nectarine plant";
		public override int PlantHue => 1147;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0C59; // Harvestable plant graphic
		public override Type FruitType => typeof(PeaceNectarineFruit);

		[Constructable]
		public PeaceNectarineFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public PeaceNectarineFruitplant(Serial serial) : base(serial)
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


    public class PeaceNectarineFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a Peace Nectarine seed";
        public override int SeedHue => 1147;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(PeaceNectarineFruitplant);

        [Constructable]
        public PeaceNectarineFruitSeed() : base()
        {
        }

        public PeaceNectarineFruitSeed(Serial serial) : base(serial)
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
