using System;

namespace Server.Items
{
    public class EmbercoreFragment : Item
    {
        [Constructable]
        public EmbercoreFragment() : base(0x0FC4) // Coin icon
        {

        }

        public EmbercoreFragment(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
