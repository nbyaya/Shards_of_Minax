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

        // Constructor used when creating the item in the world
        public BaseFruitPlant(int itemID) : base(itemID)
        {
            Movable = false;
            Name = PlantName;
            Hue = PlantHue;
            IsHarvestable = false;

            ItemID = itemID;
            plants.Add(this);
        }

        // ✅ Deserialization constructor (required by the server)
        public BaseFruitPlant(Serial serial) : base(serial)
        {
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
                if (!plant.Deleted && !plant.IsHarvestable)
                {
                    if (rnd.Next(2) == 0)
                    {
                        plant.IsHarvestable = true;
                        plant.ItemID = plant.HarvestableGraphic;
                    }

                    plant.Hue = plant.PlantHue;
                }
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!from.InRange(GetWorldLocation(), 2))
            {
                from.SendMessage("You are too far away to harvest this.");
                return;
            }

            if (IsHarvestable)
            {
                double tasteIDSkill = from.Skills[SkillName.TasteID].Value;
                int bonusFruits = (int)((tasteIDSkill / 200.0) * 5);
                int totalFruits = 1 + Math.Max(0, bonusFruits);

                for (int i = 0; i < totalFruits; i++)
                {
                    Item fruit = (Item)Activator.CreateInstance(FruitType);
                    if (from.Backpack != null && from.Backpack.TryDropItem(from, fruit, false))
                    {
                        if (i == 0)
                        {
                            from.SendMessage($"You harvest {totalFruits} {fruit.Name}{(totalFruits > 1 ? "s" : "")}.");
                        }
                    }
                    else
                    {
                        fruit.Delete();
                        from.SendLocalizedMessage(500720);
                        break;
                    }
                }

                IsHarvestable = false;
                ItemID = SeedGraphic;
            }
            else
            {
                from.SendMessage("This is not ready to harvest.");
            }

            Hue = PlantHue;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
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
