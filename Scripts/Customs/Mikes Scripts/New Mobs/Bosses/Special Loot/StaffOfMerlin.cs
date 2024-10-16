using System;
using Server;

namespace Server.Items
{
    public class StaffOfMerlin : BaseStaff
    {
        public override int LabelNumber { get { return 1063470; } } // Staff of Merlin

        [Constructable]
        public StaffOfMerlin() : base(0xDF0)
        {
            Hue = 0x489;
            Weight = 5.0;
            Attributes.SpellDamage = 20;
            Attributes.CastSpeed = 1;
            Attributes.CastRecovery = 2;
            Attributes.LowerManaCost = 10;
        }

        public StaffOfMerlin(Serial serial) : base(serial)
        {
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add("Enhances spell power"); // Custom property description
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
