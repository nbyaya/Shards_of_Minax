using System;
using Server;

namespace Server.Items
{
    public class PendantOfEchoingFrost : BaseJewel
    {
        [Constructable]
        public PendantOfEchoingFrost() : base(0x1088, Layer.Neck)
        {
            Name = "Pendant of Echoing Frost";
            Hue = 0x482; // Slightly deeper blue
            ItemID = 0x1088;
            Weight = 1.0;

            Attributes.CastSpeed = 1;
            Attributes.RegenMana = 1;
            Attributes.SpellDamage = 4;
            Resistances.Cold = 15;

            LootType = LootType.Blessed;
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add(1070722, "Forged from the voice of winter. Still echoes with purpose.");
        }

        public PendantOfEchoingFrost(Serial serial) : base(serial)
        {
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
