using System;
using Server;
using Server.Items;
using Server.Network;
using System.Collections.Generic;

namespace Server.Items
{
    public class HarvestAppleBush : Item
    {
        public static List<HarvestAppleBush> Bushes = new List<HarvestAppleBush>();
        public bool IsHarvestable;

        [Constructable]
        public HarvestAppleBush() : base(0x0C45) // Seeds graphic by default
        {
            Movable = false;
            Name = "an apple bush";
            Hue = 366; // Constant hue
            IsHarvestable = false;

            Bushes.Add(this);
        }

        public static void Initialize()
        {
            EventSink.WorldSave += new WorldSaveEventHandler(OnWorldSave);
        }

        private static void OnWorldSave(WorldSaveEventArgs e)
        {
            Random rnd = new Random();
            foreach (HarvestAppleBush bush in Bushes)
            {
                if (!bush.Deleted)
                {
                    if (rnd.Next(2) == 0) // 50% chance to become harvestable
                    {
                        bush.IsHarvestable = true;
                        bush.ItemID = 0x0C91; // Bush graphic
                    }
                    else
                    {
                        bush.IsHarvestable = false;
                        bush.ItemID = 0x0C45; // Seeds graphic
                    }

                    bush.Hue = 366; // Ensure hue is always 366
                }
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsHarvestable)
            {
                Item apple = new Apple();
                if (from.Backpack != null && from.Backpack.TryDropItem(from, apple, false))
                {
                    from.SendMessage("You harvest an apple."); // The item has been placed in your backpack.
                    IsHarvestable = false;
                    ItemID = 0x0C45; // Revert to seeds graphic
                }
                else
                {
                    apple.Delete();
                    from.SendLocalizedMessage(500720); // You don't have enough room in your backpack!
                }
            }
            else
            {
                from.SendMessage("This is not ready to harvest."); // Not ready to harvest message
            }

            Hue = 366; // Ensure hue remains 366
        }

        public HarvestAppleBush(Serial serial) : base(serial)
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

            Bushes.Add(this);
        }

        public static void Cleanup()
        {
            Bushes.RemoveAll(b => b.Deleted);
        }
    }
}
