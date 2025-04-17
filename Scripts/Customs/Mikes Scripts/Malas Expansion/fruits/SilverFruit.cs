using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Targeting;
using System.Collections.Generic;

namespace Server.Items
{
    public class SilverFruit : Food
    {
        [Constructable]
        public SilverFruit() : this(1)
        {
        }

        [Constructable]
        public SilverFruit(int amount) : base(amount, 0x1723) // Example fruit graphic
        {
            Weight = 1.0;
            FillFactor = 1;
            Name = "Silver fruit";
            Hue = 1117; // Same hue as the plant
        }

        public SilverFruit(Serial serial) : base(serial)
        {
        }

        public override bool Eat(Mobile from)
        {
            if (!base.Eat(from))
                return false;

            // 10% chance to give a SilverFruitSeed
            if (Utility.RandomDouble() < 0.1) // 0.1 = 10% chance
            {
                Item seed = new SilverFruitSeed();
                if (from.Backpack != null && from.Backpack.TryDropItem(from, seed, false))
                {
                    from.SendMessage("You find a Silver fruit seed as you eat the fruit.");
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
	
    public class SilverFruitplant : Item
    {
        public static List<SilverFruitplant> plantes = new List<SilverFruitplant>();
        public bool IsHarvestable;

        [Constructable]
        public SilverFruitplant() : base(0x0C45) // Seeds graphic by default
        {
            Movable = false;
            Name = "a Silver fruit plant";
            Hue = 1117; // Constant hue
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
            foreach (SilverFruitplant plant in plantes)
            {
                if (!plant.Deleted && !plant.IsHarvestable) // Only make unharvestable plantes harvestable
                {
                    if (rnd.Next(2) == 0) // 50% chance to become harvestable
                    {
                        plant.IsHarvestable = true;
                        plant.ItemID = 0x0D0B; // plant graphic
                    }

                    plant.Hue = 1117; // Ensure hue is always 699
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
					Item SilverFruit = new SilverFruit();
					if (from.Backpack != null && from.Backpack.TryDropItem(from, SilverFruit, false))
					{
						if (i == 0) // Only display the harvest message once
						{
							from.SendMessage($"You harvest {totalFruits} Silver fruit{(totalFruits > 1 ? "s" : "")}.");
						}
					}
					else
					{
						SilverFruit.Delete();
						from.SendLocalizedMessage(500720); // You don't have enough room in your backpack!
						break;
					}
				}

				IsHarvestable = false;
				ItemID = 0x0C45; // Revert to seeds graphic
			}
			else
			{
				from.SendMessage("This is not ready to harvest."); // Not ready to harvest message
			}

			Hue = 1117; // Ensure hue remains 699
		}


        public SilverFruitplant(Serial serial) : base(serial)
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
	
    public class SilverFruitSeed : Item
    {
        [Constructable]
        public SilverFruitSeed() : base(0xF27) // Example seed graphic
        {
            Weight = 0.1;
            Name = "a Silver fruit seed";
			Hue = 1117; // Same hue as the plant
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
                SilverFruitplant plant = new SilverFruitplant();
                plant.MoveToWorld(loc, from.Map);
                from.SendMessage("You plant the seed, and a plant begins to grow.");
            }
            else
            {
                from.SendLocalizedMessage(500722); // You cannot plant this here.
            }
        }

        public SilverFruitSeed(Serial serial) : base(serial)
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
