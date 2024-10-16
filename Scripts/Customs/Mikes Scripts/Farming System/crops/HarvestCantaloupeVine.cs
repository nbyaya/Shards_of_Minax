using System;
using Server;
using Server.Items;
using Server.Network;
using System.Collections.Generic;

namespace Server.Items
{
    public class HarvestCantaloupeVine : Item
    {
        public static List<HarvestCantaloupeVine> Vines = new List<HarvestCantaloupeVine>();
        public bool IsHarvestable;

        [Constructable]
        public HarvestCantaloupeVine() : base(0x0CEC) // Example vine graphic
        {
            Movable = false;
            Name = "a cantaloupe vine";
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
            foreach (HarvestCantaloupeVine vine in Vines)
            {
                if (!vine.Deleted)
                {
                    if (rnd.Next(2) == 0) // There's a 50% chance to become harvestable
                    {
                        vine.IsHarvestable = true;
                        vine.Hue = 0x8C2; // Change hue to indicate it's harvestable
                    }
                }
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsHarvestable)
            {
                Item cantaloupe = new Cantaloupe();
                if (from.Backpack != null && from.Backpack.TryDropItem(from, cantaloupe, false))
                {
                    from.SendMessage("You harvest a cantaloupe"); // The item has been placed in your backpack.
                    IsHarvestable = false;
                    Hue = 0; // Return to normal hue
                }
                else
                {
                    cantaloupe.Delete();
                    from.SendLocalizedMessage(500720); // You don't have enough room in your backpack!
                }
            }
            else
            {
                from.SendMessage("This is not ready to harvest."); // This is not ready to harvest.
            }
        }

        public HarvestCantaloupeVine(Serial serial) : base(serial)
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
