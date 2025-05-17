using System;
using System.Collections.Generic;
using Server.Mobiles;

namespace Server.Engines.MiniChamps
{
	public class MiniChampSpawnInfo
	{
		private MiniChamp Owner;
		public List<Mobile> Creatures;

		public Type MonsterType { get; set; }
		public int Killed { get; set; }
		public int Spawned { get; set; }
		public int Required { get; set; }
		public int MaxSpawned { get { return (Required * 2) - 1; } }
		public bool Done { get { return Killed >= Required; } }

		public MiniChampSpawnInfo(MiniChamp controller, MiniChampTypeInfo typeInfo)
		{
			Owner = controller ?? throw new ArgumentNullException(nameof(controller));

			Required = typeInfo.Required;
			MonsterType = typeInfo.SpawnType;

			Creatures = new List<Mobile>();
			Killed = 0;
			Spawned = 0;
		}

		public bool Slice()
		{
			if (Owner == null || Owner.Deleted || Creatures == null)
			{
				return false; // Nothing to process
			}

			bool killed = false;
			var list = new List<Mobile>(Creatures);

			foreach (var creature in list)
			{
				if (creature == null || creature.Deleted)
				{
					Creatures.Remove(creature);
					Killed++;
					killed = true;
				}
				else if (!creature.InRange(Owner.Location, Owner.SpawnRange + 10))
				{
					// Ensure Owner.Map is valid
					if (Owner.Map == null)
					{
						Console.WriteLine("Owner.Map is null during Slice operation.");
						continue;
					}

					// Bring creature to home
					Point3D loc = Owner.Map.GetSpawnPosition(Owner.Location, Owner.SpawnRange);
					creature.MoveToWorld(loc, Owner.Map);
					
				}
			}

			ColUtility.Free(list);
			return killed;
		}

		public bool Respawn()
		{
			if (Owner == null || Owner.Deleted || MonsterType == null || Creatures == null)
			{
				Console.WriteLine("Respawn skipped due to null references.");
				return false;
			}

			bool spawned = false;

			while (Creatures.Count < Required && Spawned < MaxSpawned)
			{
				try
				{
					// Validate MonsterType and create instance
					if (MonsterType == null)
					{
						Console.WriteLine("MonsterType is null in Respawn.");
						break;
					}

					BaseCreature bc = Activator.CreateInstance(MonsterType) as BaseCreature;
					if (bc == null)
					{
						Console.WriteLine($"Failed to create instance of {MonsterType.Name}.");
						break;
					}

					// Ensure Owner.Map is valid
					if (Owner.Map == null)
					{
						Console.WriteLine("Owner.Map is null during Respawn.");
						break;
					}

					Point3D loc = Owner.BossSpawnPoint != Point3D.Zero
						? Owner.BossSpawnPoint
						: Owner.Map.GetSpawnPosition(Owner.Location, Owner.SpawnRange);

					bc.Home = Owner.Location;
					bc.RangeHome = Owner.SpawnRange;
					bc.Tamable = false;
					bc.OnBeforeSpawn(loc, Owner.Map);
					bc.MoveToWorld(loc, Owner.Map);
					Owner.NotifyCreatureSpawned(bc);

					if (bc.Fame > Utility.Random(100000) || bc is BaseRenowned)
					{
						DropEssence(bc);
					}

					Creatures.Add(bc);
					Spawned++;
					spawned = true;
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Error during Respawn: {ex.Message}");
					break;
				}
			}

			return spawned;
		}

		private void DropEssence(BaseCreature bc)
		{
			if (bc == null || Owner == null)
				return;

			Type essenceType = MiniChampInfo.GetInfo(Owner.Type)?.EssenceType;

			if (essenceType == null)
				return;

			try
			{
				Item essence = Activator.CreateInstance(essenceType) as Item;
				if (essence != null)
				{
					bc.PackItem(essence);
				}
			}
			catch
			{
				Console.WriteLine($"Failed to create essence of type {essenceType.Name}.");
			}
		}

		public void AddProperties(ObjectPropertyList list, int cliloc)
		{
			list.Add(cliloc, "{0}: Killed {1}/{2}, Spawned {3}/{4}",
				MonsterType?.Name ?? "Unknown", Killed, Required, Spawned, MaxSpawned);
		}

		public void Serialize(GenericWriter writer)
		{
			writer.WriteItem<MiniChamp>(Owner);
			writer.Write(Killed);
			writer.Write(Spawned);
			writer.Write(Required);
			writer.Write(MonsterType?.FullName ?? string.Empty);
			writer.Write(Creatures);
		}

		public MiniChampSpawnInfo(GenericReader reader)
		{
			Creatures = new List<Mobile>();

			Owner = reader.ReadItem<MiniChamp>();
			Killed = reader.ReadInt();
			Spawned = reader.ReadInt();
			Required = reader.ReadInt();
			MonsterType = ScriptCompiler.FindTypeByFullName(reader.ReadString());
			Creatures = reader.ReadStrongMobileList();

			// Ensure Creatures is initialized
			if (Creatures == null)
			{
				Creatures = new List<Mobile>();
			}
		}
	}

}