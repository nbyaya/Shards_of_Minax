using System;
using Server;

namespace Server.Items
{
    public class ArchaeologistsHat : BaseHat
    {
        public override int LabelNumber { get { return 1041499; } } // Archaeologist's Hat

        [Constructable]
        public ArchaeologistsHat()
            : base(0x1718)
        {
            Weight = 1.0;
            Hue = 0x47E;
            LootType = LootType.Blessed;
        }

        public ArchaeologistsHat(Serial serial) : base(serial) { }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add("Boosts Item Identification");
        }

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
