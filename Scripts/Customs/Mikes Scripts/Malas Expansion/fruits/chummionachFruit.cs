using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class chummionachFruit : BaseFruit
    {
        public override string FruitName => "chummionach fruit";
        public override int FruitHue => 2743;
        public override int FruitGraphic => 0x172B; // Example fruit graphic
        public override Type SeedType => typeof(chummionachFruitSeed);

        [Constructable]
        public chummionachFruit() : base()
        {
        }

        [Constructable]
        public chummionachFruit(int amount) : base(amount)
        {
        }

        public chummionachFruit(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Versioning for future compatibility
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
    
    public class chummionachFruitplant : BaseFruitPlant
    {
        public override string PlantName => "a chummionach fruit plant";
        public override int PlantHue => 2743;
        public override int SeedGraphic => 0x0C45; // Seeds graphic
        public override int HarvestableGraphic => 0x0CAA; // Harvestable plant graphic
        public override Type FruitType => typeof(chummionachFruit);

        [Constructable]
        public chummionachFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
        {
        }

        public chummionachFruitplant(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Versioning for future compatibility
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class chummionachFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a chummionach fruit seed";
        public override int SeedHue => 2743;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(chummionachFruitplant);

        [Constructable]
        public chummionachFruitSeed() : base()
        {
        }

        public chummionachFruitSeed(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Versioning for future compatibility
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }  
}
