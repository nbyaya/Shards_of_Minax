using System;

namespace Server.Items
{
    public class FlintbladeSeal : Item
    {
        [Constructable]
        public FlintbladeSeal() : base(0x14F0) // Coin icon
        {
            Name = "Flintblade's Forged Seal";
            Hue = 0x551;
            Weight = 1.0;
        }

        public FlintbladeSeal(Serial serial) : base(serial) { }

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
