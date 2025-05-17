using System;
using Server.Items;

namespace Server.Items
{
    public class TaintedKryss : Kryss
    {
        [Constructable]
        public TaintedKryss()
        {
            Name = "tainted kryss";
            Hue = 2100; // Match the monster's hue
            Attributes.SpellChanneling = 1;
            Attributes.WeaponDamage = 25;
            Attributes.CastSpeed = 1;
        }

        public TaintedKryss(Serial serial) : base(serial) { }

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
