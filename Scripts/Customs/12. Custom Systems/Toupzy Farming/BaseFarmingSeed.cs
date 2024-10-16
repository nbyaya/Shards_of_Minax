using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Targeting;

namespace Server.FarmingSystem
{
	public class BaseFarmingSeed : Item
	{
		private int m_SeedType;
		private int m_Owner;
		private bool m_Planted = false;


		[CommandProperty(AccessLevel.GameMaster)]
		public int SeedType
		{
			get { return m_SeedType; }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public int Owner { get { return m_Owner; } set { m_Owner = value; InvalidateProperties(); } }


		public BaseFarmingSeed(int seedid, int itemid) : base(itemid)
		{
			m_SeedType = seedid;
		}

		public BaseFarmingSeed(Serial serial) : base(serial) { }

		public override void OnDoubleClick(Mobile from)
		{
			if (from.Backpack.Items.Contains(this))
			{
				if (ToupzyFarmingSystem.CanPlant(from))
				{
					if (from.Skills[ToupzyFarmingSystem.FARMINGSKILL].Value >= ToupzyFarmingSystem.GetSkillForSeed(m_SeedType))
					{
                        from.PublicOverheadMessage(Network.MessageType.Regular,0,true,"Where would you like to plant this crop?");
						from.Target = new PlantSeedTarget(this);
					}
					else
						from.PublicOverheadMessage(Network.MessageType.Regular, 0, true, String.Format("You need {0} {1} to plant this seed.", ToupzyFarmingSystem.GetSkillForSeed(m_SeedType),ToupzyFarmingSystem.FARMINGSKILL.ToString()));
				}
				else
				{
                    from.PublicOverheadMessage(Network.MessageType.Regular, 0, true, "You already have a seed that needs watering, or you have "+ToupzyFarmingSystem.MAX_PLANT_COUNT + " plants already.");
				}
			}
			else
			{
				if (this.m_Planted)
				{
					//Check for water.
					if (ToupzyFarmingSystem.HasWater(from, m_SeedType))
					{
						ToupzyFarmingSystem.ConsumeWater(from, m_SeedType);
						this.Mature();
					}
					else
                        from.PublicOverheadMessage(Network.MessageType.Regular, 0, true,"You do not have enough water in your backpack.");
				}
				else
				{
                    from.PublicOverheadMessage(Network.MessageType.Regular, 0, true, "This must be in your backpack to use it.");
				}
			}
		}

		public void SetSeedType(int seedtype)
		{
			m_SeedType = seedtype;
		}

		private void Plant(Mobile from, LandTarget target)
		{
			//Animate Player/Dig
			from.Direction = from.GetDirectionTo(target.Location);

			if (!from.Mounted)
			{
				from.Animate(AnimationType.Attack, Utility.RandomList(ToupzyFarmingSystem.EffectActions));
			}

			//Drop Seed Down.

			this.Map = from.Map;
			this.Movable = false;
			this.m_Planted = true;
			this.m_Owner = from.Serial;

			this.MoveToWorld(target.Location);

			if (from.Skills[ToupzyFarmingSystem.FARMINGSKILL].Base + 0.4 <= from.Skills[ToupzyFarmingSystem.FARMINGSKILL].Cap)
				from.Skills[ToupzyFarmingSystem.FARMINGSKILL].Base += 0.4;

			ToupzyFarmingSystem.AddPlayer(m_Owner);

		}
		private void Mature()
		{
			BaseFarmingCrop crop = ToupzyFarmingSystem.GetCrop(m_SeedType);
			crop.Location = this.Location;
			crop.Map = this.Map;
			crop.Owner = this.Owner;

			crop.MatureTime = DateTime.Now + TimeSpan.FromMinutes(2);//2 Minutes until it is ready to pluck
			crop.NextUpdate = DateTime.Now + TimeSpan.FromSeconds(30);//30 Seconds until it makes next move.

            ToupzyFarmingSystem.RemovePlayer(this.Owner);


            ToupzyFarmingSystem.AddCrop(crop,World.Mobiles[crop.Owner]);

			this.Delete();
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
			writer.Write(m_SeedType);

		writer.Write(m_Owner);
		writer.Write(m_Planted);

	}
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
			m_SeedType = reader.ReadInt();

					
		m_Owner = reader.ReadInt();
			m_Planted = reader.ReadBool();


            if (m_Owner != 0)
                ToupzyFarmingSystem.AddPlayer(m_Owner);
            else
                this.Delete();
		}

		internal class PlantSeedTarget : Target
		{
			private BaseFarmingSeed m_Seed;

			public PlantSeedTarget(BaseFarmingSeed seed) : base(2, true, TargetFlags.None)
			{
				this.m_Seed = seed;
			}
			protected override void OnTarget(Mobile from, object targeted)
			{
				if (targeted is LandTarget)
				{
					int tileID = ((LandTarget)targeted).TileID;

					if (ToupzyFarmingSystem.TileHasCrop(from.Map, ((LandTarget)targeted).Location))
					{
                        from.PublicOverheadMessage(Network.MessageType.Regular, 0, true,"Something is blocking you from planting here. Either move the item or it might not be possible to plant here.");
						return;
					}

					if (ToupzyFarmingSystem.ValidTile(tileID))
					{
						//Plant the seed.
						m_Seed.Plant(from, ((LandTarget)targeted));
					}

				}
				else
				{
                    from.PublicOverheadMessage(Network.MessageType.Regular, 0, true, "You can not plant crops here.");
				}
			}
		}
	}
}
