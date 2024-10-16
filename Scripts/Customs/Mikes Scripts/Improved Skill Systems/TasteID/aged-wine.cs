using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class AgedWine : Food
    {
        [Constructable]
        public AgedWine() : base(0x15FC)
        {
            Weight = 1.0;
            FillFactor = 1;
            Name = "Aged Wine";
			Hue = 2884;
        }

        public AgedWine(Serial serial) : base(serial)
        {
        }

        public override bool Eat(Mobile from)
        {
            if (from.Skills[SkillName.TasteID].Base < 75.0)
            {
                from.SendMessage("You need at least 75 Taste Identification skill to enjoy this.");
                return false;
            }

            int bonusValue = 25 + (int)((from.Skills[SkillName.TasteID].Value - 75) / 2.5);
            from.AddStatMod(new StatMod(StatType.Int, "AgedWine", bonusValue, TimeSpan.FromMinutes(40)));
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
