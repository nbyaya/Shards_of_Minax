using System;
using Server;

namespace Server.Items
{
    public class AquamarineNecklace : BaseNecklace
    {
        [Constructable]
        public AquamarineNecklace() : base(0x1088)
        {
            Name = "Aquamarine Necklace";
            Hue = 0x4F2;
            Weight = 2.0;
            
            Attributes.RegenMana = 2;
            Attributes.LowerRegCost = 10;
            Resistances.Cold = 5;
        }

        public AquamarineNecklace(Serial serial) : base(serial)
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