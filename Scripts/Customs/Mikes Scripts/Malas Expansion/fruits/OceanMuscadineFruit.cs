using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class OceanMuscadineFruit : BaseFruit
    {
        public override string FruitName => "Ocean Muscadine";
        public override int FruitHue => 177;
        public override int FruitGraphic => 0x1722; // Example fruit graphic
        public override Type SeedType => typeof(OceanMuscadineFruitSeed);

        [Constructable]
        public OceanMuscadineFruit() : base()
        {
        }

        [Constructable]
        public OceanMuscadineFruit(int amount) : base(amount)
        {
        }

        public OceanMuscadineFruit(Serial serial) : base(serial)
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
    
	public class OceanMuscadineFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a Ocean Muscadine plant";
		public override int PlantHue => 177;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0C8E; // Harvestable plant graphic
		public override Type FruitType => typeof(OceanMuscadineFruit);

		[Constructable]
		public OceanMuscadineFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public OceanMuscadineFruitplant(Serial serial) : base(serial)
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


    public class OceanMuscadineFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a Ocean Muscadine seed";
        public override int SeedHue => 177;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(OceanMuscadineFruitplant);

        [Constructable]
        public OceanMuscadineFruitSeed() : base()
        {
        }

        public OceanMuscadineFruitSeed(Serial serial) : base(serial)
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
