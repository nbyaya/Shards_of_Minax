using System;
using Server;

namespace Server.Items
{
    public class EyeOfCompassion : BaseJewel
    {
        public override int BaseGemTypeNumber { get { return 1044211; } }  // "magic ring"

        [Constructable]
        public EyeOfCompassion()
            : base(0x108a, Layer.Ring)  // 0x108a is a graphic for a ring. You can replace it with any valid item ID.
        {
            Name = "Eye of Compassion";
            Hue = 1157;  // Color of the ring. Change as needed.

            // Attributes of the ring
            Attributes.RegenHits = 2;  // Provides +2 HP regen rate. Adjust as you see fit.
        }

        public EyeOfCompassion(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);  // Version number for the item, for future upgrades.
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
