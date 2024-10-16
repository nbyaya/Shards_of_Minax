using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class HealingHerb : Item
    {
        [Constructable]
        public HealingHerb() : base(0x18EB) // Example item ID
        {
            Name = "Healing Herb";
            Hue = 0x4E3; // Example color
            Weight = 0.1;
        }

        public HealingHerb(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
