using System;
using Server.Gumps;
using Server.Items;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
    public class MurderClue : Item
    {
        [Constructable]
        public MurderClue() : base(0x14F0) // Example item ID, change as needed
        {
            Name = "Murder Clue";
            Hue = 1153; // Example hue, change as needed
        }

        public MurderClue(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                return;
            }

            from.SendGump(new ForensicDetectiveGump(from));
            Delete(); // Delete the item after use
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

