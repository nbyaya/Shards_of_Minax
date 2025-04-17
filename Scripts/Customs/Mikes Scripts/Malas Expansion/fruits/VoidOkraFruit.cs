using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class VoidOkraFruit : BaseFruit
    {
        public override string FruitName => "Void Okra";
        public override int FruitHue => 305;
        public override int FruitGraphic => 0x0C78; // Example fruit graphic
        public override Type SeedType => typeof(VoidOkraFruitSeed);

        [Constructable]
        public VoidOkraFruit() : base()
        {
        }

        [Constructable]
        public VoidOkraFruit(int amount) : base(amount)
        {
        }

        public VoidOkraFruit(Serial serial) : base(serial)
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
    
	public class VoidOkraFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a Void Okra plant";
		public override int PlantHue => 305;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0DEE; // Harvestable plant graphic
		public override Type FruitType => typeof(VoidOkraFruit);

		[Constructable]
		public VoidOkraFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public VoidOkraFruitplant(Serial serial) : base(serial)
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


    public class VoidOkraFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a Void Okra seed";
        public override int SeedHue => 305;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(VoidOkraFruitplant);

        [Constructable]
        public VoidOkraFruitSeed() : base()
        {
        }

        public VoidOkraFruitSeed(Serial serial) : base(serial)
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
