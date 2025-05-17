using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class PlagueSpear : Spear
    {
        [Constructable]
        public PlagueSpear()
        {
            Name = "Plague Spear";
            Hue = 0x455;
            WeaponAttributes.HitPoisonArea = 20; // AoE poison on hit
        }

        public PlagueSpear(Serial serial) : base(serial)
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
