using System;
using Server;
using Server.Network;
using Server.Prompts;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
    public class LrcTarget : Target
    {
        private LrcDeed m_Deed;

        public LrcTarget(LrcDeed deed) : base(1, false, TargetFlags.None)
        {
            m_Deed = deed;
        }

        protected override void OnTarget(Mobile from, object target)
        {
            if (m_Deed.Deleted || m_Deed.RootParent != from)
            {
                from.SendMessage("Invalid target.");
                return;
            }

            AosAttributes attributes = null;

            if (target is BaseClothing)
            {
                attributes = ((BaseClothing)target).Attributes;
            }
            else if (target is BaseArmor)
            {
                attributes = ((BaseArmor)target).Attributes;
            }
            else if (target is BaseJewel)
            {
                attributes = ((BaseJewel)target).Attributes;
            }

            if (attributes != null)
            {
                if (attributes.LowerRegCost + 20 <= 100) // Assuming max LRC is 100%
                {
                    attributes.LowerRegCost += 20;
                    from.SendMessage("You have added 20% LRC to the item.");
                    m_Deed.Delete();
                }
                else
                {
                    from.SendMessage("The item cannot exceed 100% LRC.");
                }
            }
            else
            {
                from.SendMessage("That is not an equippable item.");
            }
        }
    }

    public class LrcDeed : Item
    {
        [Constructable]
        public LrcDeed() : base(0x14F0)
        {
            Name = "a +20% LRC deed";
            Hue = 1150;
            Weight = 1.0;
        }

        public LrcDeed(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
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
                from.SendMessage("The deed must be in your backpack.");
                return;
            }

            from.SendMessage("Which item do you wish to add LRC to?");
            from.Target = new LrcTarget(this);
        }
    }
}
