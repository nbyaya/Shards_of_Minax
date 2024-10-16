using System;
using Server;
using Server.Items;
using Server.Network;
using System.Collections.Generic;

namespace Server.Items
{
    public class HarvestAppleTree : Item
    {
        public static List<HarvestAppleTree> Trees = new List<HarvestAppleTree>();
        public bool IsHarvestable;

        [Constructable]
        public HarvestAppleTree() : base(0x0DAA) // Example tree graphic
        {
            Movable = false;
            Name = "an apple tree";
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
			foreach (HarvestAppleTree tree in Trees)
			{
				if (!tree.Deleted)
				{
					if (rnd.Next(2) == 0) // There's a 50% chance to become harvestable
					{
						tree.IsHarvestable = true;
						tree.Hue = 0x8D; // Change hue to indicate it's harvestable
					}
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
                    from.SendMessage("You harvest an apple"); // The item has been placed in your backpack.
                    IsHarvestable = false;
                    Hue = 0; // Return to normal hue
                }
                else
                {
                    apple.Delete();
                    from.SendLocalizedMessage(500720); // You don't have enough room in your backpack!
                }
            }
            else
            {
                from.SendMessage("This is not ready to harvest."); // This is not ready to harvest.
            }
        }

        public HarvestAppleTree(Serial serial) : base(serial)
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
