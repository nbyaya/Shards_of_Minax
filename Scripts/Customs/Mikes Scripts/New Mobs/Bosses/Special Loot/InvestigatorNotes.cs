using System;
using Server.Items;

namespace Server.Items
{
    public class InvestigatorNotes : Item
    {
        [Constructable]
        public InvestigatorNotes() : base(0x2254)
        {
            Weight = 1.0;
            Hue = 1160; // Set to a unique color
            Name = "Investigator's Notes";
        }

        public InvestigatorNotes(Serial serial) : base(serial)
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
