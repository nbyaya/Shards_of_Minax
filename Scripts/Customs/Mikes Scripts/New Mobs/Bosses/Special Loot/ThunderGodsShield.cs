using System;
using Server;

namespace Server.Items
{
    public class ThunderGodsShield : BaseShield
    {
        [Constructable]
        public ThunderGodsShield()
            : base(0x1BC4)
        {
            Name = "Thunder God's Shield";
            Hue = 0x482;
            Weight = 6.0;
            ArmorAttributes.DurabilityBonus = 100;
            Attributes.DefendChance = 15;
            Attributes.ReflectPhysical = 10;
            Attributes.BonusDex = 5;
            Attributes.BonusStam = 5;
        }

        public ThunderGodsShield(Serial serial) : base(serial)
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
