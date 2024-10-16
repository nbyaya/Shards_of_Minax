using System;
using Server;
using Server.Items;
using Server.Services.Virtues;

namespace Server.Items
{
    public class VirtueStone : Item
    {
        // Define virtue types in derived classes
        public VirtueName Virtue { get; set; }

        public VirtueStone(int graphicId, VirtueName virtue) : base(graphicId)
        {
            Weight = 1.0; // Set the weight of the item
            Virtue = virtue;
        }

        public VirtueStone(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write((int)Virtue); // Serialize virtue type
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            Virtue = (VirtueName)reader.ReadInt(); // Deserialize virtue type
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from == null || !IsAccessibleTo(from))
                return;

            bool gainedPath = false;
            if (VirtueHelper.Award(from, Virtue, 5000, ref gainedPath))
            {
                from.SendMessage("You have gained 5000 points in {0}!", Virtue.ToString());
                Delete(); // Consume the stone
            }
            else
            {
                from.SendMessage("You cannot gain any more points in this virtue.");
            }
        }
    }

    public class CompassionStone : VirtueStone
    {
        [Constructable]
		public CompassionStone() : base(0x1869, VirtueName.Compassion)
        {
            Name = "Compassion Stone";
        }

        public CompassionStone(Serial serial) : base(serial)
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

    public class HonestyStone : VirtueStone
    {
        [Constructable]
		public HonestyStone() : base(0x186A, VirtueName.Honesty)
        {
            Name = "Honesty Stone";
        }

        public HonestyStone(Serial serial) : base(serial)
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

    public class HonorStone : VirtueStone
    {
        [Constructable]
		public HonorStone() : base(0x186B, VirtueName.Honor)
        {
            Name = "Honor Stone";
        }

        public HonorStone(Serial serial) : base(serial)
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

    public class JusticeStone : VirtueStone
    {
        [Constructable]
		public JusticeStone() : base(0x186C, VirtueName.Justice)
        {
            Name = "Justice Stone";
        }

        public JusticeStone(Serial serial) : base(serial)
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

    public class SacrificeStone : VirtueStone
    {
        [Constructable]
		public SacrificeStone() : base(0x186D, VirtueName.Sacrifice)
        {
            Name = "Sacrifice Stone";
        }

        public SacrificeStone(Serial serial) : base(serial)
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

    public class SpiritualityStone : VirtueStone
    {
        [Constructable]
		public SpiritualityStone() : base(0x186E, VirtueName.Spirituality)
        {
            Name = "Spirituality Stone";
        }

        public SpiritualityStone(Serial serial) : base(serial)
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

    public class ValorStone : VirtueStone
    {
        [Constructable]
		public ValorStone() : base(0x186F, VirtueName.Valor)
        {
            Name = "Valor Stone";
        }

        public ValorStone(Serial serial) : base(serial)
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

    public class HumilityStone : VirtueStone
    {
        [Constructable]
		public HumilityStone() : base(0x1870, VirtueName.Humility)
        {
            Name = "Humility Stone";
        }

        public HumilityStone(Serial serial) : base(serial)
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
