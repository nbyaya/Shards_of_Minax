using System;
using Server.Items;

namespace Server.Items
{
    public class MedicalExaminersBadge : Item
    {
        [Constructable]
        public MedicalExaminersBadge() : base(0x1F14)
        {
            Weight = 1.0;
            Hue = 1367; // Set to a unique color
            Name = "Medical Examiner's Badge";
        }

        public MedicalExaminersBadge(Serial serial) : base(serial)
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
