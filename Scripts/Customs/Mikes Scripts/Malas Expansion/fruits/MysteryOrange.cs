using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class MysteryOrangeFruit : BaseFruit
    {
        public override string FruitName => "Mystery Orange";
        public override int FruitHue => 2258;
        public override int FruitGraphic => 0x0F91; // Example fruit graphic
        public override Type SeedType => typeof(MysteryOrangeFruitSeed);

        [Constructable]
        public MysteryOrangeFruit() : base()
        {
        }

        [Constructable]
        public MysteryOrangeFruit(int amount) : base(amount)
        {
        }

        public MysteryOrangeFruit(Serial serial) : base(serial)
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
    
	public class MysteryOrangeFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a Mystery Orange plant";
		public override int PlantHue => 2258;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0CA6; // Harvestable plant graphic
		public override Type FruitType => typeof(MysteryOrangeFruit);

		[Constructable]
		public MysteryOrangeFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public MysteryOrangeFruitplant(Serial serial) : base(serial)
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


    public class MysteryOrangeFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a Mystery Orange seed";
        public override int SeedHue => 2258;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(MysteryOrangeFruitplant);

        [Constructable]
        public MysteryOrangeFruitSeed() : base()
        {
        }

        public MysteryOrangeFruitSeed(Serial serial) : base(serial)
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
