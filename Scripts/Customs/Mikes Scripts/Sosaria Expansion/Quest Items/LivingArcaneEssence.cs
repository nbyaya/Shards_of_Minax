using System;
using Server;

namespace Server.Items
{
    public class LivingArcaneEssence : Item
    {
        [Constructable]
        public LivingArcaneEssence() : base(0x3188)
        {
            Hue = 1153;
            Name = "Living Arcane Essence";
            Weight = 0.1;
        }

        public LivingArcaneEssence(Serial serial) : base(serial) { }

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
