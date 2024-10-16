using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.FarmingSystem
{
	public enum CropStatus
	{
		Dirt = 0,
		Seedling = 1,
		Budding = 2,
		Flowering = 3,
		Ripening = 4
	}

	public class BaseFarmingCrop : Item
	{
		private CropStatus m_CropStatus;
		private DateTime m_MatureTime;
		public DateTime m_NextUpdate;

		private int m_SeedCount = 0;
		private int m_CropType;

		private int m_Owner;

		[CommandProperty(AccessLevel.GameMaster)]
		public int Owner { get { return m_Owner; } set { m_Owner = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.GameMaster)]
		public CropStatus CropStatus
		{
			get { return m_CropStatus; }
			set
			{
				m_CropStatus = value;
				InvalidateProperties();
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public int SeedCount
		{
			get { return m_SeedCount; }
			set { m_SeedCount = value; }
		}

		public int CropType
		{
			get { return m_CropType; }
		}
		[CommandProperty(AccessLevel.GameMaster)]
		public DateTime MatureTime
		{
			get { return m_MatureTime; }
			set { m_MatureTime = value; }
		}
		[CommandProperty(AccessLevel.GameMaster)]
		public DateTime NextUpdate
		{
			get { return m_NextUpdate; }
			set { m_NextUpdate = value; }
		}

		public BaseFarmingCrop(int croptype, int itemid) : base(itemid)
		{
			m_CropType = croptype;
			this.Movable = false;
		}

		public BaseFarmingCrop(Serial serial) : base(serial)
		{
		}
		public override void AddNameProperties(ObjectPropertyList list)
		{
            list.Add(1060847, string.Format("{0}", "Farming Crop"));

            list.Add(1116560, "{0}\t{1}", "Stage: ", m_CropStatus.ToString());
            if (m_Owner != 0)
                list.Add(1061640, World.Mobiles[m_Owner].Name);
            base.AddNameProperties(list);
        }
        public override void OnDoubleClick(Mobile from)
		{
			if (InRange(from.Location, 2))
			{
				if (m_Owner == 0)//No owner Free LOOT!
				{
					if ((int)m_CropStatus >= 3 && (int)m_CropStatus <= 4)
					{
						this.Harvest(from);
					}
				}
				else
				{
					if (m_Owner == from.Serial)
					{
						if (((int)m_CropStatus >= 3 && (int)m_CropStatus <= 4))
						{
							this.Harvest(from);
						}
						else
						{
							from.PublicOverheadMessage(Network.MessageType.Regular,0,true,"This crop is not ready to be harvested yet.");
						}
					}
					else
					{
						from.PublicOverheadMessage(Network.MessageType.Regular,0,true,"You are not the owner of this crop. I am sure the owner would be upset if you took this crop.");
					}
				}
			}
			else
			{
				from.PublicOverheadMessage(Network.MessageType.Regular,0,true,"You must be closer to use this item.");
			}
		}

		private void Harvest(Mobile from)
		{
			if (m_SeedCount > 0)
			{
				for (int count = 0; count < m_SeedCount; count++)
				{
					from.AddToBackpack(ToupzyFarmingSystem.GetSeed(m_CropType));
				}
			}

			from.AddToBackpack(ToupzyFarmingSystem.GetFruit(m_CropType));

			from.PublicOverheadMessage(Network.MessageType.Regular,0,true,"You harvest the crop.");

                ToupzyFarmingSystem.RemoveCrop(this);

			this.Delete();
		}

		public virtual void UpdateCrop()
		{
			if ((int)m_CropStatus < 4)
			{
				CropStatus += 1;
			}

			if (DateTime.Now >= m_MatureTime && (int)m_CropStatus <= 3)
				m_CropStatus = CropStatus.Flowering;

			if ((int)m_CropStatus >= 4)
				return;

			switch ((int)m_CropStatus)
			{
				case 0://Dirt
					{
						break;
					}
				case 1://25%
					{
						this.ItemID = ToupzyFarmingSystem.GetCropItemID(m_CropType);
						this.Hue = ToupzyFarmingSystem.GetCropHue(m_CropType);
						this.Z = this.Map.GetAverageZ(this.Location.X, this.Location.Y) - 2;

						break;
					}
				case 2://50%
					{
						this.Z = this.Map.GetAverageZ(this.Location.X, this.Location.Y) -1;
						break;
					}
				case 3://FRUIT
					{
						this.Z = this.Map.GetAverageZ(this.Location.X, this.Location.Y);
						m_SeedCount += ToupzyFarmingSystem.CheckGiveSeed(this, 1);//10% chance to spawn seed
						break;
					}
			}
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version

			writer.Write((int)m_CropStatus);
			writer.Write(m_MatureTime);
			writer.Write(m_SeedCount);
			writer.Write(m_CropType);
			writer.Write(m_Owner);
			writer.Write(m_NextUpdate);
			writer.Write(m_MatureTime);
		}
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			m_CropStatus = (CropStatus)reader.ReadInt();
			m_MatureTime = reader.ReadDateTime();
			m_SeedCount = reader.ReadInt();
			m_CropType = reader.ReadInt();
			m_Owner = reader.ReadInt();
			m_NextUpdate = reader.ReadDateTime();
			m_MatureTime = reader.ReadDateTime();

            if (m_Owner != 0)
                ToupzyFarmingSystem.AddCrop(this, World.Mobiles[m_Owner]);
            else
                this.Delete();
		}
	}
}
