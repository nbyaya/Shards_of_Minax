using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class VoidSproutFruit : BaseFruit
    {
        public override string FruitName => "Void Sprout";
        public override int FruitHue => 185;
        public override int FruitGraphic => 0x0F85; // Example fruit graphic
        public override Type SeedType => typeof(VoidSproutFruitSeed);

        [Constructable]
        public VoidSproutFruit() : base()
        {
        }

        [Constructable]
        public VoidSproutFruit(int amount) : base(amount)
        {
        }

        public VoidSproutFruit(Serial serial) : base(serial)
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
    
	public class VoidSproutFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a Void Sprout plant";
		public override int PlantHue => 185;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0C87; // Harvestable plant graphic
		public override Type FruitType => typeof(VoidSproutFruit);

		[Constructable]
		public VoidSproutFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public VoidSproutFruitplant(Serial serial) : base(serial)
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


    public class VoidSproutFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a Void Sprout seed";
        public override int SeedHue => 185;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(VoidSproutFruitplant);

        [Constructable]
        public VoidSproutFruitSeed() : base()
        {
        }

        public VoidSproutFruitSeed(Serial serial) : base(serial)
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
