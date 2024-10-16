using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class SpecialtyBread : Food
    {
        [Constructable]
        public SpecialtyBread() : base(0x15F9)
        {
            Weight = 1.0;
            FillFactor = 4;
            Name = "Specialty Bread";
			Hue = 2118;
        }

        public SpecialtyBread(Serial serial) : base(serial)
        {
        }

        public override bool Eat(Mobile from)
        {
            if (from.Skills[SkillName.TasteID].Base < 20.0)
            {
                from.SendMessage("You need at least 20 Taste Identification skill to enjoy this.");
                return false;
            }

            int bonusValue = 15 + (int)((from.Skills[SkillName.TasteID].Value - 65) / 3);
            from.AddSkillMod(new TimedSkillMod(SkillName.Cooking, true, bonusValue, TimeSpan.FromMinutes(30)));
            from.SendMessage("You feel your Cooking skill improving!");

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
