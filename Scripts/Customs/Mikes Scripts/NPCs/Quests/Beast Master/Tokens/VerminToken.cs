using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class VerminToken : Item
    {
        [Constructable]
        public VerminToken() : base(0x2D4E)
        {
            Name = "Vermin Token";
            Hue = 0x8C1;
        }

        public VerminToken(Serial serial) : base(serial)
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
