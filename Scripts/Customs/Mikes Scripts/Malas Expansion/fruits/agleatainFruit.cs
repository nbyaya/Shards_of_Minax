using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class agleatainFruit : BaseFruit
    {
        public override string FruitName => "agleatain fruit";
        public override int FruitHue => 2470;
        public override int FruitGraphic => 0x26B7; // Example fruit graphic
        public override Type SeedType => typeof(agleatainFruitSeed);

        // This ensures that the serialization works correctly by calling the base constructor
        [Constructable]
        public agleatainFruit() : base()
        {
        }

        [Constructable]
        public agleatainFruit(int amount) : base(amount)
        {
        }

        public agleatainFruit(Serial serial) : base(serial)
        {
            // No extra initialization needed here since the base constructor handles it
        }

        // Adding a method to serialize the object when it's being saved
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);  // Ensures the base class gets serialized properly
            writer.Write(0);  // Add versioning to ensure backwards compatibility
        }

        // Adding a method to deserialize the object when it's being loaded
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);  // Ensures the base class gets deserialized properly
            int version = reader.ReadInt();  // Read the version number for future upgrades
        }
    }

    public class agleatainFruitplant : BaseFruitPlant
    {
        public override string PlantName => "a agleatain fruit plant";
        public override int PlantHue => 2470;
        public override int SeedGraphic => 0x0C45; // Seeds graphic
        public override int HarvestableGraphic => 0x0CC4; // Harvestable plant graphic
        public override Type FruitType => typeof(agleatainFruit);

        [Constructable]
        public agleatainFruitplant() : base(0x0C45) // Pass SeedGraphic to base constructor
        {
        }

        public agleatainFruitplant(Serial serial) : base(serial)
        {
        }

        // Serialize and Deserialize for proper saving/loading
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);  // Ensures the base class gets serialized properly
            writer.Write(0);  // Add versioning
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);  // Ensures the base class gets deserialized properly
            int version = reader.ReadInt();  // Read version for future upgrades
        }
    }

    public class agleatainFruitSeed : BaseFruitSeed
    {
        public override string SeedName => "a agleatain fruit seed";
        public override int SeedHue => 2470;
        public override int SeedGraphic => 0xF27; // Example seed graphic
        public override Type PlantType => typeof(agleatainFruitplant);

        [Constructable]
        public agleatainFruitSeed() : base()
        {
        }

        public agleatainFruitSeed(Serial serial) : base(serial)
        {
        }

        // Serialize and Deserialize for proper saving/loading
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);  // Ensures the base class gets serialized properly
            writer.Write(0);  // Add versioning
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);  // Ensures the base class gets deserialized properly
            int version = reader.ReadInt();  // Read version for future upgrades
        }
    }
}
