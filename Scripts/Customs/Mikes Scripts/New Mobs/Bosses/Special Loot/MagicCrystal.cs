using System;
using Server;

namespace Server.Items
{
    public class MagicCrystal : Item
    {
        public override int LabelNumber { get { return 1063474; } } // Magic Crystal

        [Constructable]
        public MagicCrystal() : base(0x1F19)
        {
            Hue = 0x48D;
            Weight = 1.0;
        }

        public MagicCrystal(Serial serial) : base(serial)
        {
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add("A crystal pulsating with magical energy"); // Custom property description
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
