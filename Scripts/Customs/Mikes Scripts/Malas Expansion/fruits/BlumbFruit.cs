using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Targeting;
using System.Collections.Generic;

namespace Server.Items
{
    public class BlumbFruit : Food
    {
        [Constructable]
        public BlumbFruit() : this(1)
        {
        }

        [Constructable]
        public BlumbFruit(int amount) : base(amount, 0x09D0) // Example fruit graphic
        {
            Weight = 1.0;
            FillFactor = 1;
            Name = "blumb fruit";
            Hue = 699; // Same hue as the bush
        }

        public BlumbFruit(Serial serial) : base(serial)
        {
        }

        public override bool Eat(Mobile from)
        {
            if (!base.Eat(from))
                return false;

            // 10% chance to give a BlumbFruitSeed
            if (Utility.RandomDouble() < 0.1) // 0.1 = 10% chance
            {
                Item seed = new BlumbFruitSeed();
                if (from.Backpack != null && from.Backpack.TryDropItem(from, seed, false))
                {
                    from.SendMessage("You find a blumb fruit seed as you eat the fruit.");
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
	
    public class BlumbFruitBush : Item
    {
        public static List<BlumbFruitBush> Bushes = new List<BlumbFruitBush>();
        public bool IsHarvestable;

        [Constructable]
        public BlumbFruitBush() : base(0x0C45) // Seeds graphic by default
        {
            Movable = false;
            Name = "a blumb fruit bush";
            Hue = 699; // Constant hue
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
            foreach (BlumbFruitBush bush in Bushes)
            {
                if (!bush.Deleted && !bush.IsHarvestable) // Only make unharvestable bushes harvestable
                {
                    if (rnd.Next(2) == 0) // 50% chance to become harvestable
                    {
                        bush.IsHarvestable = true;
                        bush.ItemID = 0x0C91; // Bush graphic
                    }

                    bush.Hue = 699; // Ensure hue is always 699
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
                Item blumbFruit = new BlumbFruit();
                if (from.Backpack != null && from.Backpack.TryDropItem(from, blumbFruit, false))
                {
                    from.SendMessage("You harvest a blumb fruit."); // The item has been placed in your backpack.
                    IsHarvestable = false;
                    ItemID = 0x0C45; // Revert to seeds graphic
                }
                else
                {
                    blumbFruit.Delete();
                    from.SendLocalizedMessage(500720); // You don't have enough room in your backpack!
                }
            }
            else
            {
                from.SendMessage("This is not ready to harvest."); // Not ready to harvest message
            }

            Hue = 699; // Ensure hue remains 699
        }

        public BlumbFruitBush(Serial serial) : base(serial)
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
	
    public class BlumbFruitSeed : Item
    {
        [Constructable]
        public BlumbFruitSeed() : base(0xF27) // Example seed graphic
        {
            Weight = 0.1;
            Name = "a blumb fruit seed";
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.Backpack == null || !IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                return;
            }

            from.BeginTarget(-1, true, TargetFlags.None, new TargetStateCallback(PlantTarget), null);
            from.SendMessage("Where do you want to plant the bush?");
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
                BlumbFruitBush bush = new BlumbFruitBush();
                bush.MoveToWorld(loc, from.Map);
                from.SendMessage("You plant the seed, and a bush begins to grow.");
            }
            else
            {
                from.SendLocalizedMessage(500722); // You cannot plant this here.
            }
        }

        public BlumbFruitSeed(Serial serial) : base(serial)
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
