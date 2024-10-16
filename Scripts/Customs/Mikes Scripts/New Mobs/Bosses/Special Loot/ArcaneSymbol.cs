using System;
using Server;

namespace Server.Items
{
    public class ArcaneSymbol : Item
    {
        public override int LabelNumber { get { return 1063473; } } // Arcane Symbol

        [Constructable]
        public ArcaneSymbol() : base(0x1F18)
        {
            Hue = 0x489;
            Weight = 2.0;
        }

        public ArcaneSymbol(Serial serial) : base(serial)
        {
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add("An ancient symbol of great magical power"); // Custom property description
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
