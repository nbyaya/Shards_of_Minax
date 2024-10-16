using System;
using Server;

namespace Server.Items
{
    public class OceanInABottle : Item
    {
        [Constructable]
        public OceanInABottle() : base(0x0F01)
        {
            Name = "Ocean in a Bottle";
            Hue = 0x5D1;
            Weight = 1.0;
        }

        public OceanInABottle(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!from.InRange(GetWorldLocation(), 3))
            {
                from.SendLocalizedMessage(500446); // That is too far away.
                return;
            }

            from.SendMessage("You gaze into the bottle and see a miniature ocean inside.");
            from.PlaySound(0x249);
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