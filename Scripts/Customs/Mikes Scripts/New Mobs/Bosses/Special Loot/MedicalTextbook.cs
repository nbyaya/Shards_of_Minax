using System;
using Server;

namespace Server.Items
{
    public class MedicalTextbook : Item
    {
        [Constructable]
        public MedicalTextbook() : base(0xF5C)
        {
            Movable = true;
            Hue = 0x48B;
            Name = "Medical Textbook";
        }

        public MedicalTextbook(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(this.GetWorldLocation(), 2))
            {
                from.SendMessage("You read the medical textbook and feel more knowledgeable.");

                from.Skills.Healing.Base += 0.5; // Increase Healing skill by 0.5 points

                this.Consume();
            }
            else
            {
                from.SendLocalizedMessage(500295); // You are too far away to do that.
            }
        }

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
