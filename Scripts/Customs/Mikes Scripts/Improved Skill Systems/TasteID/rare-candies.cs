using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class RareCandies : Food
    {
        [Constructable]
        public RareCandies() : base(0x15FD)
        {
            Weight = 1.0;
            FillFactor = 1;
            Name = "Rare Candies";
			Hue = 2325;
        }

        public RareCandies(Serial serial) : base(serial)
        {
        }

        public override bool Eat(Mobile from)
        {
            if (from.Skills[SkillName.TasteID].Base < 50.0)
            {
                from.SendMessage("You need at least 50 Taste Identification skill to enjoy this.");
                return false;
            }

            int bonusValue = 25 + (int)((from.Skills[SkillName.TasteID].Value - 70) / 2);
            from.AddStatMod(new StatMod(StatType.Str, "RareCandies", bonusValue, TimeSpan.FromMinutes(30)));
            from.SendMessage("You feel your strength increasing!");

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
