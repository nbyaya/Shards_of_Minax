using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class CaveAsparagusFruit : BaseFruit
    {
        public override string FruitName => "Cave Asparagus";
        public override int FruitHue => 907;
        public override int FruitGraphic => 0x0C7A; // Example fruit graphic
        public override Type SeedType => typeof(CaveAsparagusFruitSeed);

        [Constructable]
        public CaveAsparagusFruit() : base()
        {
        }

        [Constructable]
        public CaveAsparagusFruit(int amount) : base(amount)
        {
        }

        public CaveAsparagusFruit(Serial serial) : base(serial)
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
    
	public class CaveAsparagusFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a Cave Asparagus plant";
		public override int PlantHue => 907;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0CC1; // Harvestable plant graphic
		public override Type FruitType => typeof(CaveAsparagusFruit);

		[Constructable]
		public CaveAsparagusFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public CaveAsparagusFruitplant(Serial serial) : base(serial)
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


    public class CaveAsparagusFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a Cave Asparagus seed";
        public override int SeedHue => 907;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(CaveAsparagusFruitplant);

        [Constructable]
        public CaveAsparagusFruitSeed() : base()
        {
        }

        public CaveAsparagusFruitSeed(Serial serial) : base(serial)
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
