using System;
using Server;
using Server.Items;
using Server.Network;
using System.Collections.Generic;

namespace Server.Items
{
    public class HarvestDateTree : Item
    {
        public static List<HarvestDateTree> Trees = new List<HarvestDateTree>();
        public bool IsHarvestable;

        [Constructable]
        public HarvestDateTree() : base(0x0D94) // Example tree graphic
        {
            Movable = false;
            Name = "a date tree";
            Hue = 0;
            IsHarvestable = false;

            Trees.Add(this);
        }

        public static void Initialize()
        {
            EventSink.WorldSave += new WorldSaveEventHandler(OnWorldSave);
        }

        private static void OnWorldSave(WorldSaveEventArgs e)
        {
            Random rnd = new Random();
            foreach (HarvestDateTree tree in Trees)
            {
                if (!tree.Deleted)
                {
                    if (rnd.Next(2) == 0) // There's a 50% chance to become harvestable
                    {
                        tree.IsHarvestable = true;
                        tree.Hue = 0xF9; // Change hue to indicate it's harvestable
                    }
                }
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsHarvestable)
            {
                Item dates = new Dates();
                if (from.Backpack != null && from.Backpack.TryDropItem(from, dates, false))
                {
                    from.SendMessage("You harvest a date"); // The item has been placed in your backpack.
                    IsHarvestable = false;
                    Hue = 0; // Return to normal hue
                }
                else
                {
                    dates.Delete();
                    from.SendLocalizedMessage(500720); // You don't have enough room in your backpack!
                }
            }
            else
            {
                from.SendMessage("This is not ready to harvest."); // This is not ready to harvest.
            }
        }

        public HarvestDateTree(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Version
            writer.Write(IsHarvestable);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            IsHarvestable = reader.ReadBool();

            Trees.Add(this);
        }

        public static void Cleanup()
        {
            Trees.RemoveAll(t => t.Deleted);
        }
    }
}
