using System;
using Server.Items;

namespace Server.Items
{
    public class WhisperingPendant : BaseJewel
    {
        [Constructable]
        public WhisperingPendant() : base(0x1088, Layer.Neck)
        {
            Name = "Whispering Pendant";
            Hue = 1154;
            Attributes.SpellDamage = 5;
            Attributes.LowerManaCost = 8;
			ItemID = 0x1088; // Amulet graphic
        }

        public WhisperingPendant(Serial serial) : base(serial) { }

        public override void OnSingleClick(Mobile from)
        {
            base.OnSingleClick(from);
            from.SendMessage(0x482, "*The pendant hums faintly, echoing with ghostly breath.*");
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
