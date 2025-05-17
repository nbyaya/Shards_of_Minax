using System;
using Server;

namespace Server.Items
{
    public class BrothersMiningBand : GoldRing
    {
        [Constructable]
        public BrothersMiningBand()
        {
            Name = "Brother’s Mining Band";
            Hue = 0x9C4; // Iron with a dull glow
            Attributes.BonusHits = 5;
            Attributes.DefendChance = 5;
            Attributes.Luck = 100;
        }

        public BrothersMiningBand(Serial serial) : base(serial) { }

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
