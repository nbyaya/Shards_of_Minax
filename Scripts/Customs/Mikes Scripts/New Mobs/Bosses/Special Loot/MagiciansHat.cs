using System;
using Server;

namespace Server.Items
{
    public class MagiciansHat : BaseHat
    {
        [Constructable]
        public MagiciansHat() : base(0x1718)
        {
            Name = "Magician's Hat";
            Hue = 0x481;
            Attributes.Luck += 500; // Adds luck
        }

        public MagiciansHat(Serial serial) : base(serial)
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
