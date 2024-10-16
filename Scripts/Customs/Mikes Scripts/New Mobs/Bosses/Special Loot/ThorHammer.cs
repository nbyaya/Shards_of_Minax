using System;
using Server;

namespace Server.Items
{
    public class ThorHammer : BaseWeapon
    {
        [Constructable]
        public ThorHammer()
            : base(0x13B4)
        {
            Name = "Thor's Hammer";
            Hue = 0x482;
            Weight = 10.0;
            Layer = Layer.TwoHanded;
            WeaponAttributes.HitLightning = 50;
            Attributes.WeaponSpeed = 30;
            Attributes.WeaponDamage = 50;
            Attributes.BonusStr = 10;
            Attributes.BonusStam = 10;
        }

        public ThorHammer(Serial serial) : base(serial)
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
