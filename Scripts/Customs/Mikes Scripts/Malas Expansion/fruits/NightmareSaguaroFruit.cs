using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class NightmareSaguaroFruit : BaseFruit
    {
        public override string FruitName => "Nightmare Saguaro";
        public override int FruitHue => 108;
        public override int FruitGraphic => 0x0F83; // Example fruit graphic
        public override Type SeedType => typeof(NightmareSaguaroFruitSeed);

        [Constructable]
        public NightmareSaguaroFruit() : base()
        {
        }

        [Constructable]
        public NightmareSaguaroFruit(int amount) : base(amount)
        {
        }

        public NightmareSaguaroFruit(Serial serial) : base(serial)
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
    
	public class NightmareSaguaroFruitplant : BaseFruitPlant
	{
		public override string PlantName => "a Nightmare Saguaro plant";
		public override int PlantHue => 108;
		public override int SeedGraphic => 0x0C45; // Seeds graphic
		public override int HarvestableGraphic => 0x0C9E; // Harvestable plant graphic
		public override Type FruitType => typeof(NightmareSaguaroFruit);

		[Constructable]
		public NightmareSaguaroFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
		{
		}

		public NightmareSaguaroFruitplant(Serial serial) : base(serial)
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


    public class NightmareSaguaroFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a Nightmare Saguaro seed";
        public override int SeedHue => 108;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(NightmareSaguaroFruitplant);

        [Constructable]
        public NightmareSaguaroFruitSeed() : base()
        {
        }

        public NightmareSaguaroFruitSeed(Serial serial) : base(serial)
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
