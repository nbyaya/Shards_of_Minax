using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class AncientArtifact : Item
    {
        [Constructable]
        public AncientArtifact() : base(0x194E) // Example item ID
        {
            Name = "Ancient Artifact";
            Hue = 0x4E3; // Example color
            Weight = 0.1;
        }

        public AncientArtifact(Serial serial) : base(serial)
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
