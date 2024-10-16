using System;
using Server;

namespace Server.Items
{
    public class SpellbookOfAvalon : Spellbook
    {
        public override int LabelNumber { get { return 1063471; } } // Spellbook of Avalon

        [Constructable]
        public SpellbookOfAvalon() : base()
        {
            Hue = 0x48E;
            Attributes.SpellDamage = 15;
            Attributes.LowerRegCost = 20;
            Attributes.RegenMana = 3;
            Attributes.BonusInt = 8;
        }

        public SpellbookOfAvalon(Serial serial) : base(serial)
        {
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add("Boosts magical abilities"); // Custom property description
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
