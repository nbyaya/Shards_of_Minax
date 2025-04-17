using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Targeting;
using System.Collections.Generic;

namespace Server.Items
{
    public class FluxxFruit : Food
    {
        [Constructable]
        public FluxxFruit() : this(1)
        {
        }

        [Constructable]
        public FluxxFruit(int amount) : base(amount, 0x09D0) // Example fruit graphic
        {
            Weight = 1.0;
            FillFactor = 1;
            Name = "Fluxx fruit";
            Hue = 799; // Same hue as the plant
        }

        public FluxxFruit(Serial serial) : base(serial)
        {
        }

        public override bool Eat(Mobile from)
        {
            if (!base.Eat(from))
                return false;

            // 10% chance to give a FluxxFruitSeed
            if (Utility.RandomDouble() < 0.1) // 0.1 = 10% chance
            {
                Item seed = new FluxxFruitSeed();
                if (from.Backpack != null && from.Backpack.TryDropItem(from, seed, false))
                {
                    from.SendMessage("You find a Fluxx fruit seed as you eat the fruit.");
                }
                else
                {
                    seed.Delete();
                    from.SendLocalizedMessage(500720); // You don't have enough room in your backpack!
                }
            }

            return true;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
	
    public class FluxxFruitplant : Item
    {
        public static List<FluxxFruitplant> plantes = new List<FluxxFruitplant>();
        public bool IsHarvestable;

        [Constructable]
        public FluxxFruitplant() : base(0x0C45) // Seeds graphic by default
        {
            Movable = false;
            Name = "a Fluxx fruit plant";
            Hue = 799; // Constant hue
            IsHarvestable = false;

            plantes.Add(this);
        }

        public static void Initialize()
        {
            EventSink.WorldSave += new WorldSaveEventHandler(OnWorldSave);
        }

        private static void OnWorldSave(WorldSaveEventArgs e)
        {
            Random rnd = new Random();
            foreach (FluxxFruitplant plant in plantes)
            {
                if (!plant.Deleted && !plant.IsHarvestable) // Only make unharvestable plantes harvestable
                {
                    if (rnd.Next(2) == 0) // 50% chance to become harvestable
                    {
                        plant.IsHarvestable = true;
                        plant.ItemID = 0x0C91; // plant graphic
                    }

                    plant.Hue = 799; // Ensure hue is always 699
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
                Item FluxxFruit = new FluxxFruit();
                if (from.Backpack != null && from.Backpack.TryDropItem(from, FluxxFruit, false))
                {
                    from.SendMessage("You harvest a Fluxx fruit."); // The item has been placed in your backpack.
                    IsHarvestable = false;
                    ItemID = 0x0C45; // Revert to seeds graphic
                }
                else
                {
                    FluxxFruit.Delete();
                    from.SendLocalizedMessage(500720); // You don't have enough room in your backpack!
                }
            }
            else
            {
                from.SendMessage("This is not ready to harvest."); // Not ready to harvest message
            }

            Hue = 799; // Ensure hue remains 699
        }

        public FluxxFruitplant(Serial serial) : base(serial)
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

            plantes.Add(this);
        }

        public static void Cleanup()
        {
            plantes.RemoveAll(b => b.Deleted);
        }
    }
	
    public class FluxxFruitSeed : Item
    {
        [Constructable]
        public FluxxFruitSeed() : base(0xF27) // Example seed graphic
        {
            Weight = 0.1;
            Name = "a Fluxx fruit seed";
			Hue = 799; // Same hue as the plant
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.Backpack == null || !IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                return;
            }

            from.BeginTarget(-1, true, TargetFlags.None, new TargetStateCallback(PlantTarget), null);
            from.SendMessage("Where do you want to plant the plant?");
        }

        private void PlantTarget(Mobile from, object targeted, object state)
        {
            IPoint3D p = targeted as IPoint3D;
            if (p == null)
                return;

            if (from.Map == null)
                return;

            Point3D loc = new Point3D(p);
            if (from.Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, true))
            {
                this.Delete(); // Consume the seed
                FluxxFruitplant plant = new FluxxFruitplant();
                plant.MoveToWorld(loc, from.Map);
                from.SendMessage("You plant the seed, and a plant begins to grow.");
            }
            else
            {
                from.SendLocalizedMessage(500722); // You cannot plant this here.
            }
        }

        public FluxxFruitSeed(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
