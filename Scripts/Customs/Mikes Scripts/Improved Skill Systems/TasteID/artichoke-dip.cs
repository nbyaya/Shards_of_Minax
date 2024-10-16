using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class ArtichokeDip : Food
    {
        [Constructable]
        public ArtichokeDip() : base(0x15F9)
        {
            Weight = 1.0;
            FillFactor = 5;
            Name = "Artichoke Dip";
			Hue = 2134;
        }

        public ArtichokeDip(Serial serial) : base(serial)
        {
        }

        public override bool Eat(Mobile from)
        {
            if (from.Skills[SkillName.TasteID].Base < 70.0)
            {
                from.SendMessage("You need at least 70 Taste Identification skill to enjoy this.");
                return false;
            }

            int bonusValue = 20 + (int)((from.Skills[SkillName.TasteID].Value - 70) / 3);
            from.AddStatMod(new StatMod(StatType.Int, "ArtichokeDip", bonusValue, TimeSpan.FromMinutes(30)));
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
