using System;
using Server;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Items
{
    public class TileExcavatorShovel : Item
    {
        [Constructable]
        public TileExcavatorShovel() : base(0xF39) // ItemID for a shovel
        {
            Name = "Excavator Shovel";
            Hue = 1152;
            Weight = 5.0;
        }

        public TileExcavatorShovel(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from is PlayerMobile)
            {
                from.SendMessage("Target the ground tile or static object you wish to excavate.");
                from.Target = new InternalTarget(this);
            }
        }

        private class InternalTarget : Target
        {
            private readonly TileExcavatorShovel m_Shovel;

            public InternalTarget(TileExcavatorShovel shovel)
                : base(1, true, TargetFlags.None)
            {
                m_Shovel = shovel;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                Item copiedItem = null;

                if (targeted is LandTarget)
                {
                    LandTarget landTarget = (LandTarget)targeted;
                    copiedItem = new Item(landTarget.TileID);
                    copiedItem.Name = "Excavated Tile";
                }
                else if (targeted is StaticTarget)
                {
                    StaticTarget staticTarget = (StaticTarget)targeted;
                    copiedItem = new Item(staticTarget.ItemID);
                    copiedItem.Name = "Excavated Static Object";
                }

                if (copiedItem != null)
                {
                    copiedItem.Movable = true;
                    from.AddToBackpack(copiedItem);
                    from.SendMessage("You successfully excavated the object!");
                    m_Shovel.Delete(); // Destroy the shovel after use
                }
                else
                {
                    from.SendMessage("You must target a ground tile or a static object.");
                }
            }
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
