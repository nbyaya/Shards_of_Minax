using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.FarmingSystem
{
	public class FarmingSeed : BaseFarmingSeed
	{
		[Constructable]
		public FarmingSeed() : base(0, 3535)
		{
			BaseFarmingSeed seed = ToupzyFarmingSystem.GetSeed(Utility.Random(0, ToupzyFarmingSystem.SeedCount));

			this.Name = seed.Name;
			this.Hue = seed.Hue;
			this.ItemID = seed.ItemID;

			this.SetSeedType(seed.SeedType);

			seed.Delete();
		}

		public FarmingSeed(Serial serial)
			   : base(serial)
		{
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0);
		}
	}
}
