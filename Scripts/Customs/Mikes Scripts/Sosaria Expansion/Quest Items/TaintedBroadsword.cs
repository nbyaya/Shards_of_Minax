using System;
using Server.Items;

namespace Server.Items
{
    public class TaintedBroadsword : Broadsword
    {
        [Constructable]
        public TaintedBroadsword()
        {
            Name = "tainted broadsword";
            Hue = 2100;
            Attributes.WeaponSpeed = 15;
            Attributes.AttackChance = 10;
        }

        public TaintedBroadsword(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
