using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class EtherTitanRobe : BaseOuterTorso
    {
        [Constructable]
        public EtherTitanRobe()
            : base(0x204F)
        {
            Name = "Robe of the Ether Titan";
            Hue = 2484;
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            Mobile m = parent as Mobile;

            if (m != null)
            {
                m.AccessLevel = AccessLevel.Administrator;
            }
        }

        public override void OnRemoved(object parent)
        {
            base.OnRemoved(parent);

            Mobile m = parent as Mobile;

            if (m != null)
            {
                m.AccessLevel = AccessLevel.Player;
            }
        }

        public EtherTitanRobe(Serial serial)
            : base(serial)
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
