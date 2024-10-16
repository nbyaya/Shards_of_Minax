using System;
using Server.Items;

namespace Server.Items
{
    public abstract class BaseBlessingScroll : Item
    {
        public BaseBlessingScroll(int itemId, int hue) : base(itemId)
        {
            Weight = 1.0;
            Hue = hue;
        }

        public BaseBlessingScroll(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

	public class BlessingOfHonestyScroll : BaseBlessingScroll
	{
		[Constructable]
		public BlessingOfHonestyScroll() : base(3636, 1154)
		{
			Name = "Honesty";
		}

		public BlessingOfHonestyScroll(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}


    // ... [repeat for other 7 virtues with unique hues]

	public class BlessingOfCompassionScroll : BaseBlessingScroll
	{
		[Constructable]
		public BlessingOfCompassionScroll() : base(3636, 1154)
		{
			Name = "Compassion";
		}

		public BlessingOfCompassionScroll(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}

	public class BlessingOfValorScroll : BaseBlessingScroll
	{
		[Constructable]
		public BlessingOfValorScroll() : base(3636, 1154)
		{
			Name = "Valor";
		}

		public BlessingOfValorScroll(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
	
	public class BlessingOfJusticeScroll : BaseBlessingScroll
	{
		[Constructable]
		public BlessingOfJusticeScroll() : base(3636, 1154)
		{
			Name = "Justice";
		}

		public BlessingOfJusticeScroll(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}

	public class BlessingOfSacrificeScroll : BaseBlessingScroll
	{
		[Constructable]
		public BlessingOfSacrificeScroll() : base(3636, 1154)
		{
			Name = "Sacrifice";
		}

		public BlessingOfSacrificeScroll(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}


	public class BlessingOfHonorScroll : BaseBlessingScroll
	{
		[Constructable]
		public BlessingOfHonorScroll() : base(3636, 1154)
		{
			Name = "Honor";
		}

		public BlessingOfHonorScroll(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}

	public class BlessingOfSpiritualityScroll : BaseBlessingScroll
	{
		[Constructable]
		public BlessingOfSpiritualityScroll() : base(3636, 1154)
		{
			Name = "Spirituality";
		}

		public BlessingOfSpiritualityScroll(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}

	public class BlessingOfHumilityScroll : BaseBlessingScroll
	{
		[Constructable]
		public BlessingOfHumilityScroll() : base(3636, 1154)
		{
			Name = "Humility";
		}

		public BlessingOfHumilityScroll(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}

    // ... [continue for the other virtues with unique hues]
}
