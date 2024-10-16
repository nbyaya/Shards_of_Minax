using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class PeaceableWoodenFence : Item
    {
        private static int[] ItemIDs = new int[] { 0x085C, 0x085D, 0x085E, 0x085F, 0x0860, 0x0861 };

        [Constructable]
        public PeaceableWoodenFence() : base(0x085C)
        {
            Movable = true;
            Weight = 100.0;
        }

        public PeaceableWoodenFence(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int) 0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (Movable)
            {
                Rotate();
            }
        }

        private void Rotate()
        {
            for (int i = 0; i < ItemIDs.Length; i++)
            {
                if (this.ItemID == ItemIDs[i])
                {
                    int nextIndex = (i + 1) % ItemIDs.Length;
                    this.ItemID = ItemIDs[nextIndex];
                    break;
                }
            }
        }
    }
}
