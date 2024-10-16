using System;
using Server;

namespace Server.Items
{
    public class MysticalScroll : Item
    {
        public override int LabelNumber { get { return 1063472; } } // Mystical Scroll

        [Constructable]
        public MysticalScroll() : base(0x1F4E)
        {
            Hue = 0x481;
            Weight = 1.0;
        }

        public MysticalScroll(Serial serial) : base(serial)
        {
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add("A scroll containing ancient magical secrets"); // Custom property description
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
