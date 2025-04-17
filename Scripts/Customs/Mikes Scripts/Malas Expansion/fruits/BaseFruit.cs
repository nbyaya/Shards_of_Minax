using System;
using Server;
using Server.Items;
using Server.Network;

namespace Server.Items
{
    public abstract class BaseFruit : Food
    {
        public abstract string FruitName { get; }
        public abstract int FruitHue { get; }
        public abstract int FruitGraphic { get; }
        public abstract Type SeedType { get; }

        public BaseFruit() : this(1)
        {
        }

        public BaseFruit(int amount) : base(amount, 0x0C73) // Default graphic; can be overridden
        {
            Weight = 1.0;
            FillFactor = 1;
            Name = FruitName;
            Hue = FruitHue;
            ItemID = FruitGraphic;
        }

        // 🔧 Added deserialization constructor
        public BaseFruit(Serial serial) : base(serial)
        {
        }

        public override bool Eat(Mobile from)
        {
            if (!base.Eat(from))
                return false;

            // 20% chance to give a seed
            if (Utility.RandomDouble() < 0.2)
            {
                Item seed = (Item)Activator.CreateInstance(SeedType);
                if (from.Backpack != null && from.Backpack.TryDropItem(from, seed, false))
                {
                    from.SendMessage($"You find a {seed.Name} as you eat the {Name.ToLower()}.");
                }
                else
                {
                    seed.Delete();
                    from.SendLocalizedMessage(500720); // You don't have enough room in your backpack!
                }
            }

            return true;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
