using System;
using Server.Items;

namespace Server.Items
{
    public class TaintedKatana : Katana
    {
        [Constructable]
        public TaintedKatana()
        {
            Name = "tainted katana";
            Hue = 2100;
            Attributes.WeaponSpeed = 20;
            Attributes.WeaponDamage = 15;
        }

        public TaintedKatana(Serial serial) : base(serial) { }

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
