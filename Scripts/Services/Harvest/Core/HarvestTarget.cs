using System;
using Server.Engines.Quests;
using Server.Engines.Quests.Hag;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Engines.Harvest
{
    public class HarvestTarget : Target
    {
        private readonly Item m_Tool;
        private readonly HarvestSystem m_System;

        public HarvestTarget(Item tool, HarvestSystem system)
            : base(-1, true, TargetFlags.None)
        {
            m_Tool = tool;
            m_System = system;

            DisallowMultis = true;
        }

		protected override void OnTarget(Mobile from, object targeted)
		{
			// Check if the player clicked on themselves
			if (targeted is Mobile && from == (Mobile)targeted)
			{
				// Handle Mining self-targeting
				if (m_System is Mining miningSystem)
				{
					// Attempt to find a valid adjacent mining tile
					if (HarvestSystem.FindValidTile(from, miningSystem.OreAndStone, out object toHarvest))
					{
						miningSystem.StartHarvesting(from, m_Tool, toHarvest);
						return;
					}
				}
				// Handle Lumberjacking self-targeting
				else if (m_System is Lumberjacking lumberSystem)
				{
					// Attempt to find a valid adjacent tree tile
					if (HarvestSystem.FindValidTile(from, lumberSystem.Definition, out object toHarvest))
					{
						lumberSystem.StartHarvesting(from, m_Tool, toHarvest);
						return;
					}
				}

				// No valid adjacent tile was found; notify the player
				from.SendLocalizedMessage(501862); // "You can't harvest there."
				return;
			}

			// --- Standard handling for mining and lumberjacking follows ---

			if (m_System is Mining)
			{
				if (targeted is StaticTarget)
				{
					int itemID = ((StaticTarget)targeted).ItemID;

					// Check if it's a grave for a specific quest
					if (itemID == 0xED3 || itemID == 0xEDF || itemID == 0xEE0 || itemID == 0xEE1 || itemID == 0xEE2 || itemID == 0xEE8)
					{
						if (from is PlayerMobile player)
						{
							QuestSystem qs = player.Quest;

							if (qs is WitchApprenticeQuest)
							{
								if (qs.FindObjective(typeof(FindIngredientObjective)) is FindIngredientObjective obj && !obj.Completed && obj.Ingredient == Ingredient.Bones)
								{
									player.SendLocalizedMessage(1055037); // "You find some of the specific bones listed in the Hag's recipe."
									obj.Complete();
									return;
								}
							}
						}
					}
				}
				else if (targeted is LandTarget land && land.TileID >= 113 && land.TileID <= 120)
				{
					// Handle special cases like The Great Volcano Quest
					if (Server.Engines.Quests.TheGreatVolcanoQuest.OnHarvest(from, m_Tool))
						return;
				}
			}

			// Lumberjacking special cases
			if (m_System is Lumberjacking)
			{
				if (targeted is IChopable chopable)
				{
					chopable.OnChop(from);
					return;
				}
				else if (targeted is IAxe && m_Tool is BaseAxe)
				{
					IAxe obj = (IAxe)targeted;
					Item item = (Item)targeted;

					if (!item.IsChildOf(from.Backpack))
						from.SendLocalizedMessage(1062334); // "This item must be in your backpack to be used."
					else if (obj.Axe(from, (BaseAxe)m_Tool))
						from.PlaySound(0x13E);

					return;
				}
				else if (targeted is ICarvable carvable)
				{
					carvable.Carve(from, (Item)m_Tool);
					return;
				}
				else if (FurnitureAttribute.Check(targeted as Item))
				{
					DestroyFurniture(from, (Item)targeted);
					return;
				}
			}

			// Handle Treasure Maps for Mining
			if (m_System is Mining && targeted is TreasureMap)
			{
				((TreasureMap)targeted).OnBeginDig(from);
				return;
			}

			// Handle High Seas mining cases (Niter Deposits)
			if (m_System is Mining && targeted is NiterDeposit)
			{
				((NiterDeposit)targeted).OnMine(from, m_Tool);
				return;
			}

			// Handle High Seas lumberjacking cases (Cracked Lava Rocks)
			if (m_System is Lumberjacking)
			{
				if (targeted is CrackedLavaRockEast)
				{
					((CrackedLavaRockEast)targeted).OnCrack(from);
					return;
				}
				else if (targeted is CrackedLavaRockSouth)
				{
					((CrackedLavaRockSouth)targeted).OnCrack(from);
					return;
				}
			}

			// Check if Lumberjacking requires the axe to be equipped
			if (m_System is Lumberjacking && m_Tool.Parent != from)
			{
				from.SendLocalizedMessage(500487); // "The axe must be equipped for any serious wood chopping."
				return;
			}

			// Start the default harvesting process
			m_System.StartHarvesting(from, m_Tool, targeted);
		}

		// Utility function to destroy furniture when chopping it
		private void DestroyFurniture(Mobile from, Item item)
		{
			if (!from.InRange(item.GetWorldLocation(), 3))
			{
				from.SendLocalizedMessage(500446); // "That is too far away."
				return;
			}
			else if (!item.IsChildOf(from.Backpack) && !item.Movable)
			{
				from.SendLocalizedMessage(500462); // "You can't destroy that while it is here."
				return;
			}

			from.SendLocalizedMessage(500461); // "You destroy the item."
			Effects.PlaySound(item.GetWorldLocation(), item.Map, 0x3B3);

			if (item is Container container)
			{
				if (item is TrapableContainer trapable)
					trapable.ExecuteTrap(from);

				container.Destroy();
			}
			else
			{
				item.Delete();
			}
		}

    }
}