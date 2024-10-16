using System;
using Server.Gumps;
using Server.Items;
using Server.Network;
using Server.Targeting;
using Server.Custom;

namespace Server.Items
{
    public class BurglaryMission : Item
    {
        [Constructable]
        public BurglaryMission() : base(0x14F0) // Example item ID, change as needed
        {
            Name = "Burglary Mission";
            Hue = 1143; // Example hue, change as needed
        }

        public BurglaryMission(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                return;
            }

            from.SendGump(new ThievesGuildMissionGump(from));
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