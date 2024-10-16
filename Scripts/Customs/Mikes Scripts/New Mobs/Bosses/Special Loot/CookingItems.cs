using System;
using Server.Items;

namespace Server.Items
{
    public class ExquisiteCookingTools : Item
    {
        [Constructable]
        public ExquisiteCookingTools() : base(0x9D6)
        {
            Name = "Exquisite Cooking Tools";
            Hue = 0x47E;
            Weight = 2.0;
        }

        public ExquisiteCookingTools(Serial serial) : base(serial)
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

    public class MasterworkCookingApron : BaseOuterTorso
    {
        [Constructable]
        public MasterworkCookingApron() : base(0x153b)
        {
            Name = "Masterwork Cooking Apron";
            Hue = 0x47E;
            Weight = 2.0;
        }

        public MasterworkCookingApron(Serial serial) : base(serial)
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

    public class DecorativePlatter : Item
    {
        [Constructable]
        public DecorativePlatter() : base(0x9D2)
        {
            Name = "Decorative Platter";
            Hue = 0x47E;
            Weight = 1.0;
        }

        public DecorativePlatter(Serial serial) : base(serial)
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

    public class GoldenChefHat : BaseHat
    {
        [Constructable]
        public GoldenChefHat() : base(0x171C)
        {
            Name = "Golden Chef Hat";
            Hue = 0x8A5;
            Weight = 1.0;
        }

        public GoldenChefHat(Serial serial) : base(serial)
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
