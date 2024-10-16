using System;
using Server;

namespace Server.Items
{
    public class TwinPendantOfDespise : BaseNecklace
    {
        public override int LabelNumber { get { return 1150000; } } // Twin Pendant of Despise

        [Constructable]
        public TwinPendantOfDespise() : base(0x1088)
        {
            Hue = 1157; // Adjust the color to your liking
            Weight = 0.1;

            Attributes.BonusStr = 5; // Boosts Strength attribute for better physical damage
            Attributes.RegenMana = 3; // Boosts Mana Regeneration
        }

        public TwinPendantOfDespise(Serial serial) : base(serial) { }

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
