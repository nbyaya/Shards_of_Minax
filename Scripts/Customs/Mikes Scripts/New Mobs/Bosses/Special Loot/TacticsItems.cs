using System;
using Server;

namespace Server.Items
{
    public class TacticsScroll : Item
    {
        [Constructable]
        public TacticsScroll() : base(0x1F4E)
        {
            Name = "Tactics Scroll";
            Hue = 0x48D;
            Weight = 1.0;
        }

        public TacticsScroll(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class BattlePlans : Item
    {
        [Constructable]
        public BattlePlans() : base(0x1E5E)
        {
            Name = "Battle Plans";
            Hue = 0x48D;
            Weight = 2.0;
        }

        public BattlePlans(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class WarBanner : Item
    {
        [Constructable]
        public WarBanner() : base(0x1612)
        {
            Name = "War Banner";
            Hue = 0x455;
            Weight = 5.0;
        }

        public WarBanner(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class AncientBattleMap : Item
    {
        [Constructable]
        public AncientBattleMap() : base(0x14EC)
        {
            Name = "Ancient Battle Map";
            Hue = 0x455;
            Weight = 1.0;
        }

        public AncientBattleMap(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
