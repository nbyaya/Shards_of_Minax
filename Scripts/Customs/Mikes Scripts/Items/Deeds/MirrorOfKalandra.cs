using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
    public class MirrorTarget : Target
    {
        private MirrorOfKalandra m_Mirror;

        public MirrorTarget(MirrorOfKalandra mirror)
            : base(1, false, TargetFlags.None)
        {
            m_Mirror = mirror;
        }

        protected override void OnTarget(Mobile from, object target)
        {
            if (m_Mirror.Deleted || m_Mirror.RootParent != from)
            {
                from.SendMessage("The mirror cannot be used this way.");
                return;
            }

            if (target is Item)
            {
                Item item = (Item)target;
                Type type = item.GetType();

                try
                {
                    Item copiedItem = Activator.CreateInstance(type) as Item;
                    if (copiedItem != null)
                    {
                        from.AddToBackpack(copiedItem);
                        m_Mirror.Delete();
                        from.SendMessage("The item has been duplicated.");
                    }
                }
                catch
                {
                    from.SendMessage("This item cannot be duplicated.");
                }
            }
            else
            {
                from.SendMessage("This cannot be duplicated.");
            }
        }
    }

    public class MirrorOfKalandra : Item
    {
        [Constructable]
        public MirrorOfKalandra()
            : base(0x4044) // Just a sample item ID, you might want to replace with a more suitable graphic.
        {
            Name = "Mirror of Kalandra";
            Weight = 1.0;
        }

        public MirrorOfKalandra(Serial serial)
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

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack))
            {
                from.SendMessage("Select the item you wish to duplicate.");
                from.Target = new MirrorTarget(this);
            }
            else
            {
                from.SendMessage("The mirror must be in your backpack to be used.");
            }
        }
    }
}
