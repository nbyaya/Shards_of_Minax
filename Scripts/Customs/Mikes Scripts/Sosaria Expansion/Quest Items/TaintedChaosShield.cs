using System;
using Server.Items;

namespace Server.Items
{
    public class TaintedChaosShield : ChaosShield
    {
        [Constructable]
        public TaintedChaosShield()
        {
            Name = "tainted chaos shield";
            Hue = 2100;
            Attributes.DefendChance = 15;
            Attributes.SpellDamage = 5;
        }

        public TaintedChaosShield(Serial serial) : base(serial) { }

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
