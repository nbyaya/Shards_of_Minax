using System;
using Server;

namespace Server.Items
{
    public class HoudiniCloak : BaseCloak
    {
        [Constructable]
        public HoudiniCloak() : base(0x1515)
        {
            Name = "Houdini's Cloak";
            Hue = 0x455;
            Attributes.DefendChance += 5; // Adds some defense chance
			Attributes.Luck += 500; // Adds luck
        }

        public HoudiniCloak(Serial serial) : base(serial)
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
