using System;
using Server.Items;

namespace Server.Items
{
    public class AnatomistsScalpel : BaseSword
    {
        [Constructable]
        public AnatomistsScalpel()
            : base(0x27A5)
        {
            Weight = 2.0;
            Hue = 0x489;

            WeaponAttributes.HitLeechHits = 40;
            Attributes.WeaponDamage = 50;
            Attributes.WeaponSpeed = 30;
            WeaponAttributes.HitLowerDefend = 50;
        }

        public AnatomistsScalpel(Serial serial)
            : base(serial)
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

    public class VitruvianAmulet : BaseJewel
    {
        [Constructable]
        public VitruvianAmulet()
            : base(0x1088, Layer.Neck)
        {
            Weight = 1.0;
            Hue = 0x48C;

            Attributes.RegenHits = 5;
            Attributes.RegenStam = 5;
            Attributes.RegenMana = 5;
        }

        public VitruvianAmulet(Serial serial)
            : base(serial)
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
