using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Targeting;
using System.Collections.Generic;

namespace Server.Items
{
    public class LillypillyFruit : Food
    {
        [Constructable]
        public LillypillyFruit() : this(1)
        {
        }

        [Constructable]
        public LillypillyFruit(int amount) : base(amount, 0x0C64) // Example fruit graphic
        {
            Weight = 1.0;
            FillFactor = 1;
            Name = "Lillypilly fruit";
            Hue = 951; // Same hue as the plant
        }

        public LillypillyFruit(Serial serial) : base(serial)
        {
        }

        public override bool Eat(Mobile from)
        {
            if (!base.Eat(from))
                return false;

            // 10% chance to give a LillypillyFruitSeed
            if (Utility.RandomDouble() < 0.1) // 0.1 = 10% chance
            {
                Item seed = new LillypillyFruitSeed();
                if (from.Backpack != null && from.Backpack.TryDropItem(from, seed, false))
                {
                    from.SendMessage("You find a Lillypilly fruit seed as you eat the fruit.");
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
	
    public class LillypillyFruitplant : Item
    {
        public static List<LillypillyFruitplant> plantes = new List<LillypillyFruitplant>();
        public bool IsHarvestable;

        [Constructable]
        public LillypillyFruitplant() : base(0x0C45) // Seeds graphic by default
        {
            Movable = false;
            Name = "a Lillypilly fruit plant";
            Hue = 951; // Constant hue
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
            foreach (LillypillyFruitplant plant in plantes)
            {
                if (!plant.Deleted && !plant.IsHarvestable) // Only make unharvestable plantes harvestable
                {
                    if (rnd.Next(2) == 0) // 50% chance to become harvestable
                    {
                        plant.IsHarvestable = true;
                        plant.ItemID = 0x0C93; // plant graphic
                    }

                    plant.Hue = 951; // Ensure hue is always 699
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
					Item LillypillyFruit = new LillypillyFruit();
					if (from.Backpack != null && from.Backpack.TryDropItem(from, LillypillyFruit, false))
					{
						if (i == 0) // Only display the harvest message once
						{
							from.SendMessage($"You harvest {totalFruits} Lillypilly fruit{(totalFruits > 1 ? "s" : "")}.");
						}
					}
					else
					{
						LillypillyFruit.Delete();
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

			Hue = 951; // Ensure hue remains 699
		}


        public LillypillyFruitplant(Serial serial) : base(serial)
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
	
    public class LillypillyFruitSeed : Item
    {
        [Constructable]
        public LillypillyFruitSeed() : base(0xF27) // Example seed graphic
        {
            Weight = 0.1;
            Name = "a Lillypilly fruit seed";
			Hue = 951; // Same hue as the plant
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
                LillypillyFruitplant plant = new LillypillyFruitplant();
                plant.MoveToWorld(loc, from.Map);
                from.SendMessage("You plant the seed, and a plant begins to grow.");
            }
            else
            {
                from.SendLocalizedMessage(500722); // You cannot plant this here.
            }
        }

        public LillypillyFruitSeed(Serial serial) : base(serial)
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
