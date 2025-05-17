using System;
using Server.Items;

namespace Server.Items
{
    public class EchoStone : Item
    {
        [Constructable]
        public EchoStone() : base(0x1422)
        {
            Name = "Echo Stone";
            Hue = 1154;
            Weight = 1.0;
        }

        public EchoStone(Serial serial) : base(serial) { }

        public override void OnSingleClick(Mobile from)
        {
            base.OnSingleClick(from);
            from.SendMessage(0x482, "*You hear a faint whisper: 'Don’t dig deeper…'*");
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
