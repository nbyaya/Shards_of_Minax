using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class UniqueSeafoodDish : Food
    {
        [Constructable]
        public UniqueSeafoodDish() : base(0x15F9)
        {
            Weight = 1.0;
            FillFactor = 5;
            Name = "Unique Seafood Dish";
			Hue = 2634;
        }

        public UniqueSeafoodDish(Serial serial) : base(serial)
        {
        }

        public override bool Eat(Mobile from)
        {
            if (from.Skills[SkillName.TasteID].Base < 30.0)
            {
                from.SendMessage("You need at least 30 Taste Identification skill to enjoy this.");
                return false;
            }

            int bonusValue = 30 + (int)((from.Skills[SkillName.TasteID].Value - 80) / 2);
            from.AddSkillMod(new TimedSkillMod(SkillName.Fishing, true, bonusValue, TimeSpan.FromMinutes(45)));
            from.SendMessage("You feel your Fishing skill improving!");

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
