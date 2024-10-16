using System;
using Server;

namespace Server.Items
{
    public class KingsCrown : BaseHat
    {
        [Constructable]
        public KingsCrown() : base(0x2B71)
        {
            Name = "King's Crown";
            Hue = 0x8A5; // Royal color

            Attributes.BonusStr = 10;
            Attributes.BonusInt = 10;
            Attributes.RegenMana = 2;
            SkillBonuses.SetValues(0, SkillName.Mining, 5.0);
        }

        public KingsCrown(Serial serial) : base(serial)
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
