using System;
using Server.Items;

namespace Server.Items
{
    public abstract class BaseVirtueRune : Item
    {
        public BaseVirtueRune(int itemId) : base(itemId)
        {
            Weight = 1.0;
            Hue = 1153; // You can change this color hue if needed
        }

        public BaseVirtueRune(Serial serial) : base(serial)
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

    public class HonestyRune : BaseVirtueRune
    {
        [Constructable]
        public HonestyRune() : base(6174)
        {
            Name = "HonestyRune";
        }

        public HonestyRune(Serial serial) : base(serial)
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

    public class CompassionRune : BaseVirtueRune
    {
        [Constructable]
        public CompassionRune() : base(6175)
        {
            Name = "CompassionRune";
        }

        public CompassionRune(Serial serial) : base(serial)
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

    public class ValorRune : BaseVirtueRune
    {
        [Constructable]
        public ValorRune() : base(6176)
        {
            Name = "ValorRune";
        }

        public ValorRune(Serial serial) : base(serial)
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

    public class JusticeRune : BaseVirtueRune
    {
        [Constructable]
        public JusticeRune() : base(6177)
        {
            Name = "JusticeRune";
        }

        public JusticeRune(Serial serial) : base(serial)
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

    public class SacrificeRune : BaseVirtueRune
    {
        [Constructable]
        public SacrificeRune() : base(6178)
        {
            Name = "SacrificeRune";
        }

        public SacrificeRune(Serial serial) : base(serial)
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

    public class HonorRune : BaseVirtueRune
    {
        [Constructable]
        public HonorRune() : base(6179)
        {
            Name = "HonorRune";
        }

        public HonorRune(Serial serial) : base(serial)
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

    public class SpiritualityRune : BaseVirtueRune
    {
        [Constructable]
        public SpiritualityRune() : base(6180)
        {
            Name = "SpiritualityRune";
        }

        public SpiritualityRune(Serial serial) : base(serial)
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

    public class HumilityRune : BaseVirtueRune
    {
        [Constructable]
        public HumilityRune() : base(6181)
        {
            Name = "HumilityRune";
        }

        public HumilityRune(Serial serial) : base(serial)
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
}
