using System;
using Server;

namespace Server.Items
{
    public class IceboundAmulet : BaseJewel
    {
        [Constructable]
        public IceboundAmulet() : base(0x1088, Layer.Neck)
        {
            Name = "Icebound Amulet";
            Hue = 0x47E; // Icy hue
            ItemID = 0x1088; // Amulet graphic
            Weight = 1.0;

            Attributes.DefendChance = 5;
            Attributes.RegenHits = 2;
            Attributes.SpellDamage = 5;
            Resistances.Cold = 10;

            LootType = LootType.Cursed;
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add(1070722, "Bound in eternal frost. Whispers at night."); // Additional description
        }

        public override void OnSingleClick(Mobile from)
        {
            base.OnSingleClick(from);
            LabelTo(from, "(Cursed)");
        }

        public IceboundAmulet(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
