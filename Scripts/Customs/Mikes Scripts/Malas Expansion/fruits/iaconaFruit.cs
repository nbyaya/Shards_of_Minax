using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class iaconaFruit : BaseFruit
    {
        public override string FruitName => "iacona fruit";
        public override int FruitHue => 1437;
        public override int FruitGraphic => 0x0C72; // Example fruit graphic
        public override Type SeedType => typeof(iaconaFruitSeed);

        [Constructable]
        public iaconaFruit() : base()
        {
        }

        [Constructable]
        public iaconaFruit(int amount) : base(amount)
        {
        }

        public iaconaFruit(Serial serial) : base(serial)
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
    
	public class iaconaFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a iacona fruit plant";
		public override int PlantHue => 1437;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0C95; // Harvestable plant graphic
		public override Type FruitType => typeof(iaconaFruit);

		[Constructable]
		public iaconaFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public iaconaFruitplant(Serial serial) : base(serial)
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


    public class iaconaFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a iacona fruit seed";
        public override int SeedHue => 1437;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(iaconaFruitplant);

        [Constructable]
        public iaconaFruitSeed() : base()
        {
        }

        public iaconaFruitSeed(Serial serial) : base(serial)
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
