using System;
using Server.Items;

namespace Server.Items
{
    public class BurnedBoneFragment : Item
    {
        [Constructable]
        public BurnedBoneFragment() : base(0x0ECA)
        {
            Name = "Burned Bone Fragment";
            Hue = 1175;
            Weight = 1.0;
        }

        public BurnedBoneFragment(Serial serial) : base(serial) { }

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
