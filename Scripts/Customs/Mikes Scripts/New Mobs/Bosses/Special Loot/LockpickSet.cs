using System;
using Server;

namespace Server.Items
{
    public class LockpickSet : Item
    {
        [Constructable]
        public LockpickSet() : base(0x14FC)
        {
            Name = "Lockpick Set";
            Hue = 0x3B2;
            Weight = 1.0;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack to use it.
                return;
            }

            from.SendLocalizedMessage(501879); // You examine the lockpick set and feel more confident in your abilities.
            from.Skills[SkillName.Lockpicking].Base += 5; // Boosts lockpicking skill
            Delete();
        }

        public LockpickSet(Serial serial) : base(serial)
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
