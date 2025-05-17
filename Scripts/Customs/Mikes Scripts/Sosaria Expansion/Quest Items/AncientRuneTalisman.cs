using System;
using Server;

namespace Server.Items
{
    public class AncientRuneTalisman : GoldRing
    {
        [Constructable]
        public AncientRuneTalisman()
        {
            Name = "Ancient Rune Talisman";
            Hue = 1152;
            Weight = 1.0;
            SkillBonuses.SetValues(0, SkillName.MagicResist, 5.0);
        }

        public AncientRuneTalisman(Serial serial) : base(serial)
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
