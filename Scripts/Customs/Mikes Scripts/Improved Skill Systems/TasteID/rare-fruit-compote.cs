using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class RareFruitCompote : Food
    {
        [Constructable]
        public RareFruitCompote() : base(0x15FE)
        {
            Weight = 1.0;
            FillFactor = 4;
            Name = "Rare Fruit Compote";
			Hue = 2414;
        }

        public RareFruitCompote(Serial serial) : base(serial)
        {
        }

        public override bool Eat(Mobile from)
        {
            if (from.Skills[SkillName.TasteID].Base < 65.0)
            {
                from.SendMessage("You need at least 65 Taste Identification skill to enjoy this.");
                return false;
            }

            int bonusValue = 15 + (int)((from.Skills[SkillName.TasteID].Value - 65) / 3);
            from.AddStatMod(new StatMod(StatType.Int, "RareFruitCompote", bonusValue, TimeSpan.FromMinutes(30)));
            from.SendMessage("You feel your intelligence increasing!");

            return base.Eat(from);
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
