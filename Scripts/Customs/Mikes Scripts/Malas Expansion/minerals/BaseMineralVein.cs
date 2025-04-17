using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Items
{
    public abstract class BaseMineralVein : Item
    {
        public abstract string VeinName { get; }
        public abstract int VeinHue { get; }
        public abstract int VeinGraphic { get; }
        public abstract int HarvestedGraphic { get; }
        public abstract Type MineralType { get; }

        public static List<BaseMineralVein> veins = new List<BaseMineralVein>();
        public bool IsHarvestable;

        // Constructor for new instances
        public BaseMineralVein(int itemID) : base(itemID)
        {
            Movable = false;
            Name = VeinName;
            Hue = VeinHue;
            IsHarvestable = false;

            ItemID = itemID; // Set the initial graphic
            veins.Add(this);
        }

        // ✅ Required constructor for deserialization
        public BaseMineralVein(Serial serial) : base(serial)
        {
        }

        public static void Initialize()
        {
            EventSink.WorldSave += new WorldSaveEventHandler(OnWorldSave);
        }

        private static void OnWorldSave(WorldSaveEventArgs e)
        {
            Random rnd = new Random();
            foreach (BaseMineralVein vein in veins)
            {
                if (!vein.Deleted && !vein.IsHarvestable)
                {
                    if (rnd.Next(2) == 0) // 50% chance to become harvestable
                    {
                        vein.IsHarvestable = true;
                        vein.ItemID = vein.HarvestedGraphic;
                    }

                    vein.Hue = vein.VeinHue;
                }
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!from.InRange(GetWorldLocation(), 2))
            {
                from.SendMessage("You are too far away to mine this.");
                return;
            }

            if (!IsHarvestable)
            {
                from.SendMessage("This mineral vein has already been harvested.");
                return;
            }

            double miningSkill = from.Skills[SkillName.Mining].Value;
            if (miningSkill < 50.0) // Minimum skill requirement
            {
                from.SendMessage("You lack the mining skill to gather minerals from this vein.");
                return;
            }

            int baseAmount = 1;
            int bonusMinerals = (int)((miningSkill / 200.0) * 5); // Scale from 0 to 5
            int totalMinerals = baseAmount + Math.Max(0, bonusMinerals);

            for (int i = 0; i < totalMinerals; i++)
            {
                Item mineral = (Item)Activator.CreateInstance(MineralType);
                if (from.Backpack != null && from.Backpack.TryDropItem(from, mineral, false))
                {
                    if (i == 0)
                    {
                        from.SendMessage($"You mine {totalMinerals} {mineral.Name}{(totalMinerals > 1 ? "s" : "")}.");
                    }
                }
                else
                {
                    mineral.Delete();
                    from.SendLocalizedMessage(500720); // You don't have enough room in your backpack!
                    break;
                }
            }

            IsHarvestable = false;
            ItemID = VeinGraphic; // Revert to unharvestable graphic
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

            veins.Add(this);
        }

        public static void Cleanup()
        {
            veins.RemoveAll(v => v.Deleted);
        }
    }
}
