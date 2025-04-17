using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
    public abstract class BaseFruitPlant : Item
    {
        public abstract string PlantName { get; }
        public abstract int PlantHue { get; }
        public abstract int SeedGraphic { get; }
        public abstract int HarvestableGraphic { get; }
        public abstract Type FruitType { get; }

        public static List<BaseFruitPlant> plants = new List<BaseFruitPlant>();
        public bool IsHarvestable;

		public BaseFruitPlant(int itemID) : base(itemID)
		{
			Movable = false;
			Name = PlantName;
			Hue = PlantHue;
			IsHarvestable = false;

			ItemID = itemID;
			plants.Add(this);
		}

        public static void Initialize()
        {
            EventSink.WorldSave += new WorldSaveEventHandler(OnWorldSave);
        }

        private static void OnWorldSave(WorldSaveEventArgs e)
        {
            Random rnd = new Random();
            foreach (BaseFruitPlant plant in plants)
            {
                if (!plant.Deleted && !plant.IsHarvestable) // Only make unharvestable plants harvestable
                {
                    if (rnd.Next(2) == 0) // 50% chance to become harvestable
                    {
                        plant.IsHarvestable = true;
                        plant.ItemID = plant.HarvestableGraphic;
                    }

                    plant.Hue = plant.PlantHue; // Ensure hue is always correct
                }
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!from.InRange(GetWorldLocation(), 2)) // Check if the player is within 2 tiles
            {
                from.SendMessage("You are too far away to harvest this.");
                return;
            }

            if (IsHarvestable)
            {
                // Calculate the number of fruits to harvest
                double tasteIDSkill = from.Skills[SkillName.TasteID].Value; // Get TasteID skill value
                int bonusFruits = (int)((tasteIDSkill / 200.0) * 5); // Scale from 0 to 5 based on skill
                int totalFruits = 1 + Math.Max(0, bonusFruits); // At least 1 fruit is always harvested

                for (int i = 0; i < totalFruits; i++)
                {
                    Item fruit = (Item)Activator.CreateInstance(FruitType);
                    if (from.Backpack != null && from.Backpack.TryDropItem(from, fruit, false))
                    {
                        if (i == 0) // Only display the harvest message once
                        {
                            from.SendMessage($"You harvest {totalFruits} {fruit.Name}{(totalFruits > 1 ? "s" : "")}.");
                        }
                    }
                    else
                    {
                        fruit.Delete();
                        from.SendLocalizedMessage(500720); // You don't have enough room in your backpack!
                        break;
                    }
                }

                IsHarvestable = false;
                ItemID = SeedGraphic; // Revert to seed graphic
            }
            else
            {
                from.SendMessage("This is not ready to harvest."); // Not ready to harvest message
            }

            Hue = PlantHue; // Ensure hue remains correct
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

            plants.Add(this);
        }

        public static void Cleanup()
        {
            plants.RemoveAll(b => b.Deleted);
        }
    }
}
