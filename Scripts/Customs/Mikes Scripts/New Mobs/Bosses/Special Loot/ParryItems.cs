using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class Shield : BaseShield
    {
        [Constructable]
        public Shield() : base(0x1B76)
        {
            Name = "Standard Shield";
            Weight = 6.0;
        }

        public Shield(Serial serial) : base(serial)
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

    public class HolyShield : BaseShield
    {
        [Constructable]
        public HolyShield() : base(0x1BC3)
        {
            Name = "Holy Shield";
            Weight = 5.0;
            Attributes.DefendChance = 10;
            Attributes.SpellChanneling = 1;
        }

        public HolyShield(Serial serial) : base(serial)
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

    public class SaintsArmor : PlateChest
    {
        [Constructable]
        public SaintsArmor()
        {
            Name = "Saint's Armor";
            Weight = 10.0;
            Attributes.BonusHits = 10;
            Attributes.RegenHits = 5;
        }

        public SaintsArmor(Serial serial) : base(serial)
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

    public class GuardianHelm : PlateHelm
    {
        [Constructable]
        public GuardianHelm()
        {
            Name = "Guardian's Helm";
            Weight = 5.0;
            Attributes.BonusStr = 5;
            Attributes.DefendChance = 5;
        }

        public GuardianHelm(Serial serial) : base(serial)
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

    public class DivineCloak : BaseCloak
    {
        [Constructable]
        public DivineCloak() : base(0x1515)
        {
            Name = "Divine Cloak";
            Weight = 4.0;
            Attributes.RegenMana = 2;
            Attributes.RegenStam = 2;
            Attributes.LowerRegCost = 10;
        }

        public DivineCloak(Serial serial) : base(serial)
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

    public class HolyRelic : Item
    {
        [Constructable]
        public HolyRelic() : base(0x1F1C)
        {
            Name = "Holy Relic";
            Weight = 1.0;
            Hue = 0x482;
        }

        public HolyRelic(Serial serial) : base(serial)
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

    public class BannerOfOrleans : Item
    {
        [Constructable]
        public BannerOfOrleans() : base(0x161D)
        {
            Name = "Banner of Orleans";
            Weight = 5.0;
            Hue = 0x47E;
        }

        public BannerOfOrleans(Serial serial) : base(serial)
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
