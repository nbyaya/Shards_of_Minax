using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class ziongerFruit : BaseFruit
    {
        public override string FruitName => "zionger fruit";
        public override int FruitHue => 302;
        public override int FruitGraphic => 0x0C5D; // Example fruit graphic
        public override Type SeedType => typeof(ziongerFruitSeed);

        [Constructable]
        public ziongerFruit() : base()
        {
        }

        [Constructable]
        public ziongerFruit(int amount) : base(amount)
        {
        }

        public ziongerFruit(Serial serial) : base(serial)
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
    
	public class ziongerFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a zionger fruit plant";
		public override int PlantHue => 302;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0C88; // Harvestable plant graphic
		public override Type FruitType => typeof(ziongerFruit);

		[Constructable]
		public ziongerFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public ziongerFruitplant(Serial serial) : base(serial)
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


    public class ziongerFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a zionger fruit seed";
        public override int SeedHue => 302;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(ziongerFruitplant);

        [Constructable]
        public ziongerFruitSeed() : base()
        {
        }

        public ziongerFruitSeed(Serial serial) : base(serial)
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
