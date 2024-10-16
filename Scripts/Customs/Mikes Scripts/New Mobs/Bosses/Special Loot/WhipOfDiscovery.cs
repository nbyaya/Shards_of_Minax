using System;
using Server;

namespace Server.Items
{
    public class WhipOfDiscovery : BaseMeleeWeapon
    {
        public override int LabelNumber { get { return 1041500; } } // Whip of Discovery

        [Constructable]
        public WhipOfDiscovery()
            : base(0x27A1)
        {
            Weight = 5.0;
            Hue = 0x47E;
            LootType = LootType.Blessed;
        }

        public WhipOfDiscovery(Serial serial) : base(serial) { }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add("Increases Loot Quality");
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
