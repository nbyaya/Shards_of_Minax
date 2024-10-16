using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Items
{
    public class PlantingGloves : Item
    {
        [Constructable]
        public PlantingGloves() : base(0x13C6) // You can change the item ID here
        {
            Name = "Planting Gloves"; // You can change the item name here
            Weight = 1.0; // You can change the item weight here
        }

        public PlantingGloves(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from == null || !from.Alive || from.Backpack == null)
                return;

            from.SendMessage("Select an item to plant on someone.");
            from.Target = new PlantTarget(this);
        }

        private class PlantTarget : Target
        {
            private PlantingGloves m_Gloves;

            public PlantTarget(PlantingGloves gloves) : base(1, false, TargetFlags.None)
            {
                m_Gloves = gloves;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (from == null || !from.Alive || from.Backpack == null || m_Gloves == null || m_Gloves.Deleted)
                    return;

                Item item = targeted as Item;

                if (item == null || !item.IsChildOf(from.Backpack))
                {
                    from.SendMessage("You can only plant items from your backpack.");
                    return;
                }

                from.SendMessage("Select a player or NPC to plant the item on.");
                from.Target = new PlantTarget2(m_Gloves, item);
            }
        }

        private class PlantTarget2 : Target
        {
            private PlantingGloves m_Gloves;
            private Item m_Item;

            public PlantTarget2(PlantingGloves gloves, Item item) : base(1, false, TargetFlags.None)
            {
                m_Gloves = gloves;
                m_Item = item;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (from == null || !from.Alive || from.Backpack == null || m_Gloves == null || m_Gloves.Deleted || m_Item == null || m_Item.Deleted)
                    return;

                Mobile target = targeted as Mobile;

                if (target == null || target.Backpack == null)
                {
                    from.SendMessage("You can only plant items on players or NPCs.");
                    return;
                }

                if (target == from)
                {
                    from.SendMessage("You cannot plant items on yourself.");
                    return;
                }

                if (target.AccessLevel > from.AccessLevel)
                {
                    from.SendMessage("You cannot plant items on someone with a higher access level than you.");
                    return;
                }

                double stealing = from.Skills[SkillName.Stealing].Value;

                if (stealing < 50.0)
                {
                    from.SendMessage("You need at least 50.0 stealing skill to use these gloves.");
                    return;
                }

                double chance = (stealing - 50.0) / 100.0; // You can change the formula for the success chance here

                if (Utility.RandomDouble() < chance)
                {
                    from.SendMessage("You successfully plant the item on {0}.", target.Name);
                    target.SendMessage("You feel a slight tug on your backpack.");
                    from.Backpack.RemoveItem(m_Item);
                    target.Backpack.DropItem(m_Item);
                }
                else
                {
                    from.SendMessage("You fail to plant the item on {0}.", target.Name);
                    target.SendMessage("You notice {0} trying to plant something on you.", from.Name);
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
