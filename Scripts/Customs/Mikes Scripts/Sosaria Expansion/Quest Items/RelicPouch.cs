using System;
using Server;

namespace Server.Items
{
    public class RelicPouch : Bag
    {
        [Constructable]
        public RelicPouch()
        {
            Name = "Alric’s Relic Pouch";
            Hue = 1152;
            Weight = 2.0;
            LootType = LootType.Blessed;
        }

        public RelicPouch(Serial serial) : base(serial) { }

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
