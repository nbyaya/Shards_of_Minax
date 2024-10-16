using System;
using Server;

namespace Server.Items
{
    public class HoudiniTome : Item
    {
        [Constructable]
        public HoudiniTome() : base(0x1C10)
        {
            Name = "Houdini's Tome";
            Hue = 0x467;
            Weight = 1.0;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack to use it.
                return;
            }

            from.SendLocalizedMessage(501879); // You study the tome and feel more knowledgeable.
            from.Skills[SkillName.Hiding].Base += 5; // Boosts hiding skill
            Delete();
        }

        public HoudiniTome(Serial serial) : base(serial)
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
