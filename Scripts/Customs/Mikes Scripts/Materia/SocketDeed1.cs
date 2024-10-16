using System;
using Server;
using Server.Network;
using Server.Prompts;
using Server.Items;
using Server.Engines.XmlSpawner2;
using Server.Targeting;

namespace Server.Items
{
    public class SocketDeed1 : Item
    {
        [Constructable]
        public SocketDeed1() : base(0x14F0)
        {
            Weight = 1.0;
            Name = "a tier 1 socket deed";
            LootType = LootType.Blessed;
        }

        public SocketDeed1(Serial serial) : base(serial)
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

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                return;
            }

            from.SendMessage("Which item would you like to make socketable?");
            from.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private SocketDeed1 m_Deed;

            public InternalTarget(SocketDeed1 deed) : base(1, false, TargetFlags.None)
            {
                m_Deed = deed;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (m_Deed.Deleted || !m_Deed.IsChildOf(from.Backpack))
                {
                    from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                    return;
                }

                if (targeted is Item)
                {
                    Item item = (Item)targeted;

                    if (XmlAttach.FindAttachment(item, typeof(XmlSocketable)) != null)
                    {
                        from.SendMessage("That item is already socketable.");
                    }
                    else
                    {
                        // Add the socketable attachment with 1 max socket
                        XmlAttach.AttachTo(item, new XmlSocketable(1));
                        // Add the socket attachment with 1 socket
                        XmlAttach.AttachTo(item, new XmlSockets(1));
                        from.SendMessage("You have added a socket to the item.");

                        // Delete the deed after use
                        m_Deed.Delete();
                    }
                }
                else
                {
                    from.SendMessage("That is not a valid item.");
                }
            }
        }
    }
}
