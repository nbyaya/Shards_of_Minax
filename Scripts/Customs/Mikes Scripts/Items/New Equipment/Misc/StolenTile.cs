using System;
using Server;

namespace Server.Items
{
    public class StolenTile : Item
    {
        [Constructable]
        public StolenTile() : base(0x22A4) // Default item ID, you can change this
        {
            Name = "Stolen Ground";
            Weight = 1.0;
        }

        public StolenTile(Serial serial) : base(serial)
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
