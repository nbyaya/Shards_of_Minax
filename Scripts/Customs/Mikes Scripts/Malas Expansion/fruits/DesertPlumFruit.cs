using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class DesertPlumFruit : BaseFruit
    {
        public override string FruitName => "Desert Plum";
        public override int FruitHue => 135;
        public override int FruitGraphic => 0x0C64; // Example fruit graphic
        public override Type SeedType => typeof(DesertPlumFruitSeed);

        [Constructable]
        public DesertPlumFruit() : base()
        {
        }

        [Constructable]
        public DesertPlumFruit(int amount) : base(amount)
        {
        }

        public DesertPlumFruit(Serial serial) : base(serial)
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
    
	public class DesertPlumFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a Desert Plum plant";
		public override int PlantHue => 135;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0CA2; // Harvestable plant graphic
		public override Type FruitType => typeof(DesertPlumFruit);

		[Constructable]
		public DesertPlumFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public DesertPlumFruitplant(Serial serial) : base(serial)
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


    public class DesertPlumFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a Desert Plum seed";
        public override int SeedHue => 135;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(DesertPlumFruitplant);

        [Constructable]
        public DesertPlumFruitSeed() : base()
        {
        }

        public DesertPlumFruitSeed(Serial serial) : base(serial)
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
