using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class CinderGingerFruit : BaseFruit
    {
        public override string FruitName => "Cinder Ginger";
        public override int FruitHue => 1986;
        public override int FruitGraphic => 0x1725; // Example fruit graphic
        public override Type SeedType => typeof(CinderGingerFruitSeed);

        [Constructable]
        public CinderGingerFruit() : base()
        {
        }

        [Constructable]
        public CinderGingerFruit(int amount) : base(amount)
        {
        }

        public CinderGingerFruit(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version number, can increase if you change serialization structure
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt(); // read version number
        }
    }

    public class CinderGingerFruitplant : BaseFruitPlant
    {
        public override string PlantName => "a Cinder Ginger plant";
        public override int PlantHue => 1986;
        public override int SeedGraphic => 0x0C45; // Seeds graphic
        public override int HarvestableGraphic => 0x0C85; // Harvestable plant graphic
        public override Type FruitType => typeof(CinderGingerFruit);

        [Constructable]
        public CinderGingerFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
        {
        }

        public CinderGingerFruitplant(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version number
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt(); // read version number
        }
    }

    public class CinderGingerFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a Cinder Ginger seed";
        public override int SeedHue => 1986;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(CinderGingerFruitplant);

        [Constructable]
        public CinderGingerFruitSeed() : base()
        {
        }

        public CinderGingerFruitSeed(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version number
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt(); // read version number
        }
    }
}
