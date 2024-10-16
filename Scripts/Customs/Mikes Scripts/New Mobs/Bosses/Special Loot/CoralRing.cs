using System;
using Server;

namespace Server.Items
{
    public class CoralRing : BaseRing
    {
        [Constructable]
        public CoralRing() : base(0x108A)
        {
            Name = "Coral Ring";
            Hue = 0x489;
            Weight = 1.0;
            
            Attributes.SpellDamage = 5;
            Attributes.CastRecovery = 1;
            Resistances.Fire = 5;
        }

        public CoralRing(Serial serial) : base(serial)
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