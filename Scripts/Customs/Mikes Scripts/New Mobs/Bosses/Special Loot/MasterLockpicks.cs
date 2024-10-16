using System;
using Server;

namespace Server.Items
{
    public class MasterLockpicks : Item
    {
        [Constructable]
        public MasterLockpicks() : base(0x14FC)
        {
            Name = "Master Lockpicks";
            Hue = 0x4E7;
            Weight = 1.0;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack to use it.
                return;
            }

            from.SendLocalizedMessage(501879); // You use the master lockpicks and feel more confident in your abilities.
            from.Skills[SkillName.Lockpicking].Base += 10; // Boosts lockpicking skill significantly
            Delete();
        }

        public MasterLockpicks(Serial serial) : base(serial)
        {
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
