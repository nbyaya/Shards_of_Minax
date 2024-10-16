using System;
using Server;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
    public class BuildersHammer : Item
    {
        [Constructable]
        public BuildersHammer() : base(0x13E3) // ID for a hammer
        {
            Weight = 1.0;
            Name = "Builder's Hammer";
        }

        public BuildersHammer(Serial serial) : base(serial)
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
            from.SendMessage("Choose an object to toggle its movability.");
            from.Target = new InternalTarget();
        }

        private class InternalTarget : Target
        {
            public InternalTarget() : base(-1, false, TargetFlags.None)
            {
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Item)
                {
                    Item item = (Item)targeted;
                    item.Movable = !item.Movable;
                    from.SendMessage("You have toggled the movability of the item.");
                }
                else
                {
                    from.SendMessage("That is not a valid item.");
                }
            }
        }
    }
}
