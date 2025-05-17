using System;
using Server;

namespace Server.Items
{
    public class StoneheartPendant : BaseJewel
    {
        [Constructable]
        public StoneheartPendant() : base(0x1088, Layer.Neck)
        {
            Name = "Stoneheart Pendant";
            Hue = 1150;
            Attributes.BonusHits = 5;
            Attributes.DefendChance = 5;
			ItemID = 0x1088; // Amulet graphic

        }

        public StoneheartPendant(Serial serial) : base(serial) { }

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
