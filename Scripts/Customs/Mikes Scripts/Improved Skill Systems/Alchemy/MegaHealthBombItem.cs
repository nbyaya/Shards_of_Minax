using System;
using Server.Targeting;

namespace Server.Items
{
    public class MegaHealthBomb : BaseMegaHealthBomb
    {

        [Constructable]
        public MegaHealthBomb()
            : base(PotionEffect.ExplosionLesser)
        {
            Hue = 2721; // A distinct purple color
			Name = "Mega Health Bomb";
        }

        public MegaHealthBomb(Serial serial)
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