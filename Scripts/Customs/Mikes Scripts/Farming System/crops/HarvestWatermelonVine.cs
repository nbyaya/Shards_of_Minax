using System;
using Server;
using Server.Items;
using Server.Network;
using System.Collections.Generic;

namespace Server.Items
{
    public class HarvestWatermelonVine : Item
    {
        public static List<HarvestWatermelonVine> Vines = new List<HarvestWatermelonVine>();
        public bool IsHarvestable;

        [Constructable]
        public HarvestWatermelonVine() : base(0x0CEC) // Example vine graphic
        {
            Movable = false;
            Name = "a watermelon vine";
            Hue = 0;
            IsHarvestable = false;

            Vines.Add(this);
        }

        public static void Initialize()
        {
            EventSink.WorldSave += new WorldSaveEventHandler(OnWorldSave);
        }

        private static void OnWorldSave(WorldSaveEventArgs e)
        {
            Random rnd = new Random();
            foreach (HarvestWatermelonVine vine in Vines)
            {
                if (!vine.Deleted)
                {
                    if (rnd.Next(2) == 0) // There's a 50% chance to become harvestable
                    {
                        vine.IsHarvestable = true;
                        vine.Hue = 0x81B; // Change hue to indicate it's harvestable
                    }
                }
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsHarvestable)
            {
                Item watermelon = new Watermelon();
                if (from.Backpack != null && from.Backpack.TryDropItem(from, watermelon, false))
                {
                    from.SendMessage("You harvest a watermelon"); // The item has been placed in your backpack.
                    IsHarvestable = false;
                    Hue = 0; // Return to normal hue
                }
                else
                {
                    watermelon.Delete();
                    from.SendLocalizedMessage(500720); // You don't have enough room in your backpack!
                }
            }
            else
            {
                from.SendMessage("This is not ready to harvest."); // This is not ready to harvest.
            }
        }

        public HarvestWatermelonVine(Serial serial) : base(serial)
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

            Vines.Add(this);
        }

        public static void Cleanup()
        {
            Vines.RemoveAll(v => v.Deleted);
        }
    }
}
